using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Base
{
    public class PlayerController : MonoBehaviour
    {
        public interface ILocalComponent
        {
            public void Init(Transform player);
            public void Update();
        }
        
        [System.Serializable]
        public class Look:ILocalComponent
        {
            [SerializeField] private Camera camera;
            [SerializeField] private float lookSpeed;
            [SerializeField] private float cameraJoint = 0.2f;
            private Vector3 maxY, minY;
            
            private float angle = 0;
            private float cameraWorldY;
            private Vector3 startCameraLocalPos;
            
            private Transform player;
            private UnityEvent<float> onChangeLook = new UnityEvent<float>();

            private bool isOnGround;

            private bool cameraStabilization = true;
            
            public UnityEvent<float> OnChangeLook => onChangeLook;
            public Camera Camera => camera;

            public void Init(Transform player)
            {
                this.player = player;

                startCameraLocalPos = camera.transform.localPosition;
                maxY = startCameraLocalPos + Vector3.up * cameraJoint;
                minY = startCameraLocalPos - Vector3.up * cameraJoint;
                
                cameraWorldY = camera.transform.position.y;
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            public void Update()
            {
                float lastAngle = angle;
                float delta = lookSpeed * Time.deltaTime;
                player.transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * delta);
                angle += Input.GetAxis("Mouse Y") * delta;
                angle = Mathf.Clamp(angle, -90, 90);

                camera.transform.localEulerAngles = new Vector3(-angle, 0, 0);
                
                if (lastAngle != angle)
                {
                    OnChangeLook.Invoke(angle);
                }
            }

            public void PhysUpdate()
            {
                if (!cameraStabilization) return;
                
                var camPos = camera.transform.position;
                if (isOnGround)
                {
                    cameraWorldY = Mathf.Clamp(cameraWorldY, player.TransformPoint(minY).y, player.TransformPoint(maxY).y);
                }
                else
                {
                    cameraWorldY = player.TransformPoint(startCameraLocalPos).y;
                }
                cameraWorldY = Mathf.Lerp(cameraWorldY, player.TransformPoint(startCameraLocalPos).y, Time.fixedDeltaTime * 5);
                camera.transform.position = new Vector3(camPos.x, cameraWorldY, camPos.z);
            }

            public void Fall(bool state)
            {
                isOnGround = state;
            }
        }

        [System.Serializable]
        public class Move:ILocalComponent
        {
            [System.Serializable]
            public class Jump
            {
                [SerializeField] private float gravityModifier = 8;
                [SerializeField] private float jumpForce;
                [SerializeField] private float yOffcet;
                [SerializeField] private float radius;

                public float Radius => radius;
                public float YOffcet => yOffcet;
                public float JumpForce => jumpForce;
                public float GravityModifier => gravityModifier;
            }
            [SerializeField] private float speed;
            [SerializeField] private CapsuleCollider capsule;
            [SerializeField] private Rigidbody rigidbody;
            [SerializeField] private float gravityUseOffcet = 0.2f;
            [SerializeField] private bool isOnGround;
            [SerializeField] private Jump jump;
            [SerializeField] private Vector3 worldVelocity;
            private bool gravityAdded = false;
            private Transform player;
            private LayerMask jumpMask;


            private UnityEvent<bool> onJumpChange = new UnityEvent<bool>();

            public UnityEvent<bool> OnJumpChange => onJumpChange;
            public Vector3 WorldVelocity => worldVelocity;


            public void Init(Transform player)
            {
                this.player = player;
                rigidbody.useGravity = false;

                rigidbody.solverIterations = 5;
                rigidbody.solverVelocityIterations = 10;
                
                jumpMask = LayerMask.GetMask("Default");

                isOnGround = JumpSphere().Item1;
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            public void BaseMove()
            {
                var rawDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                Vector3 dir = rawDir * speed;
                dir = player.TransformDirection(dir);

                rigidbody.velocity = new Vector3(dir.x, rigidbody.velocity.y, dir.z);
            }

            public void Gravity()
            {
                    gravityAdded = false;
                    if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit))
                    {
                        if (hit.distance >= (capsule.height / 2f) + gravityUseOffcet)
                        {
                            AddGravity();
                        }
                        else
                        {
                            player.transform.position = Vector3.Lerp(player.transform.position, hit.point + (hit.distance * Vector3.up), Time.fixedDeltaTime);
                           //rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                        }
                    }
                    else
                    {
                        if (player.transform.position.y < -100)
                        {
                            player.transform.position = Vector3.zero;
                        }
                        isOnGround = true;
                        onJumpChange.Invoke(isOnGround);
                        AddGravity();
                    }

                    if (rigidbody.velocity.y > 0.01)
                    {
                        AddGravity();
                    }
            }

            public void BaseJump(bool jumpState)
            {
                if (Input.GetAxis("Jump") != 0 && isOnGround)
                {
                    if (jumpState)
                    {
                        rigidbody.AddForce(Vector3.up * jump.JumpForce, ForceMode.Impulse);
                        isOnGround = false;
                        onJumpChange.Invoke(isOnGround);
                    }
                }else if (!isOnGround)
                {
                    if (jumpState)
                    {
                        isOnGround = true;
                        onJumpChange.Invoke(isOnGround);
                    }
                }
            }


            public void CalculateWorldVelocity(RaycastHit jumpHit)
            {
                if (jumpHit.rigidbody != null)
                {
                    worldVelocity = jumpHit.rigidbody.velocity;
                    if (worldVelocity.y > 0)
                    {
                        worldVelocity.y = 0;
                    }
                    else
                    {
                        worldVelocity.y *= 0.1f; //чтобы когда чел едет фниз его он успефал чуть падать
                    }
                }
                else
                {
                    if (jumpHit.transform != null)
                    {
                        worldVelocity = Vector3.zero;
                    }
                    else
                    {
                        worldVelocity = Vector3.Lerp(worldVelocity, Vector3.zero, Time.fixedDeltaTime * rigidbody.drag);
                    }
                }
                
                rigidbody.velocity += worldVelocity;
            }
            
            public void Update()
            {
                var (jumpState, jumpHit) = JumpSphere();
                BaseMove();
                Gravity();
                BaseJump(jumpState);
                CalculateWorldVelocity(jumpHit);
            }


            public (bool, RaycastHit) JumpSphere()
            {
                var state = Physics.SphereCast(player.transform.position + (Vector3.down * ((capsule.height / 2f) + jump.YOffcet)), jump.Radius, Vector3.down, out RaycastHit jumpHit, 0.2f, jumpMask);

                return (state, jumpHit);
            }
            
            public void AddGravity()
            {
                if (!gravityAdded)
                {
                    if (!JumpSphere().Item1)
                    {
                        rigidbody.velocity += Physics.gravity * Time.fixedDeltaTime * jump.GravityModifier;
                        gravityAdded = true;
                    }
                }
            }

            public void Gizmo()
            {
                if (player != null)
                {
                    Debug.DrawLine(player.transform.position, player.transform.position + (Vector3.down * ((capsule.height/2f) + gravityUseOffcet)));
                    Gizmos.DrawWireSphere(player.transform.position + (Vector3.down * ((capsule.height / 2f) + jump.YOffcet)), jump.Radius);
                }
            }
        }
        
        [System.Serializable]
        public class Animate:ILocalComponent
        {
            [SerializeField] private Animator animator;
            [SerializeField] private Animator hands;
            [SerializeField] private MultiAimConstraint spineAim;
            [SerializeField] private float maxVelocity;
            [SerializeField] private float blendSpeed;
            private static readonly int Horizontal = Animator.StringToHash("Horizontal");
            private static readonly int Vertical = Animator.StringToHash("Vertical");
            private static readonly int IsFall = Animator.StringToHash("IsFall");

            private Rigidbody rb;
            private Move move;

            private float horizontalBlend, verticalBlend;
            
            public void Init(Transform player)
            {
                rb = player.GetComponent<Rigidbody>();
                horizontalBlend = 0;
                verticalBlend = 0;
            }

            public void Update()
            {
                var local = rb.transform.InverseTransformDirection(rb.velocity - move.WorldVelocity);

                var hor = -Mathf.Clamp(local.x / maxVelocity, -1f, 1f);
                var vert = Mathf.Clamp(local.z/maxVelocity, -1f, 1f);

                verticalBlend = Mathf.Lerp(verticalBlend, vert, blendSpeed * Time.deltaTime);
                horizontalBlend = Mathf.Lerp(horizontalBlend, hor, blendSpeed * Time.deltaTime);
                
                
                animator.SetFloat(Horizontal, horizontalBlend);
                animator.SetFloat(Vertical, verticalBlend);
                
                
                hands.SetFloat(Horizontal, horizontalBlend);
                hands.SetFloat(Vertical, verticalBlend);
            }

            public void Fall(bool state)
            {
                hands.SetBool(IsFall, !state);
                animator.SetBool(IsFall, !state);
            }

            public void UpdateLook(float angle)
            {
                spineAim.data.offset = new Vector3(-angle * 0.8f, 0, 0);
            }

            public void SetMove(Move move)
            {
                this.move = move;
            }
        }
        
        [System.Serializable]
        public class Hands:ILocalComponent
        {
            [SerializeField] private Transform hands;
            [SerializeField] private Transform look;
            [SerializeField] private float speed;
            private Quaternion rotation;

            public void Init(Transform player)
            {
                rotation = hands.rotation;
            }
            
            public void Update()
            {
                rotation = Quaternion.Lerp(rotation, look.rotation, speed * Time.deltaTime);
                hands.rotation = rotation;
            }
        }
        
        [SerializeField] private Look look;
        [SerializeField] private Hands hands;
        [SerializeField] private Move move;
        [SerializeField] private Animate animate;

        private void Awake()
        {
            look.Init(transform);
            move.Init(transform);
            animate.Init(transform);
            hands.Init(transform);

            animate.SetMove(move);
            move.OnJumpChange.AddListener(animate.Fall);
            move.OnJumpChange.AddListener(look.Fall);
            look.OnChangeLook.AddListener(animate.UpdateLook);
        }

        private void Update()
        {
            hands.Update();
            look.Update();
            animate.Update();
        }

        private void FixedUpdate()
        {
            move.Update();
            look.PhysUpdate();
        }

        private void OnDrawGizmos()
        {
            move.Gizmo();
        }
    }
}

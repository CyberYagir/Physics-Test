using Base;
using Builder.UI;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;

namespace Builder
{
    public class BuilderController : MonoBehaviour
    {
        [System.Serializable]
        public class Move: PlayerController.ILocalComponent
        {
            [SerializeField] private float speed;
            [SerializeField] private Transform camera;
            private Transform player;
            public void Init(Transform player)
            {
                this.player = player;
            }

            public void Update()
            {
                float sp = speed * Time.deltaTime;
                
                player.Translate(camera.forward * KeyboardService.GetAxis(Keymap.Fly_Forward) * sp, Space.World);
                player.Translate(camera.right * KeyboardService.GetAxis(Keymap.Fly_Right) * sp, Space.World);
                player.Translate(Vector3.up * KeyboardService.GetAxis(Keymap.Fly_Up) * sp, Space.World);


                speed += 5 * Input.GetAxis("Mouse ScrollWheel");
            }
        }
        
        [System.Serializable]
        public class OutlineOptions
        {
            [SerializeField] private Outlinable.OutlineProperties front;
            [SerializeField] private Outlinable.OutlineProperties back;
            public void Set(Outlinable outlinable)
            {
                SetParameters(outlinable.FrontParameters, front);
                SetParameters(outlinable.BackParameters, back);
                outlinable.RenderStyle = RenderStyle.FrontBack;
                outlinable.enabled = false;
            }

            public void SetParameters(Outlinable.OutlineProperties outlineParameters, Outlinable.OutlineProperties change)
            {
                outlineParameters.Color = change.Color;
                outlineParameters.Enabled = change.Enabled;
                outlineParameters.BlurShift = change.BlurShift;
                outlineParameters.DilateShift = change.DilateShift;
                outlineParameters.FillPass.Shader = change.FillPass.Shader;
            }
        }
        
        
        [SerializeField] private PlayerController.Look look;
        [SerializeField] private Move move;
        [SerializeField] private OutlineOptions outlines;


        public UnityEvent<GameObject> CreateObject = new UnityEvent<GameObject>();
        private UIService uiService;
        public Camera Camera => look.Camera;

        public void Init(UIService window)
        {
            look.Init(transform);
            move.Init(transform);

            this.uiService = window;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            var canMove = Input.GetKey(KeyCode.Mouse1) && !uiService.HaveOpenedWindowsOrUI();
            Cursor.visible = !canMove;
            
            
            if (!Cursor.visible)
            {
                move.Update();
                look.Update();
            }
        }

        public GameObject SpawnItem(Item item)
        {
            var camera = look.Camera.transform;
            if (Physics.Raycast(look.Camera.transform.position, look.Camera.transform.forward, out RaycastHit hit))
            {
                if (hit.distance > 20)
                {
                    return Create(camera.position + camera.forward * 5);
                }
                else
                {
                    return Create(hit.point);
                }
            }
            else
            {
                return Create(camera.position + camera.forward * 5);
            }


            GameObject Create(Vector3 point)
            {
                var obj = Instantiate(item.Prefab, point, item.Prefab.transform.rotation);
                var outlinable = obj.AddComponent<Outlinable>();

                foreach (var rn in obj.GetComponentsInChildren<Renderer>())
                {
                    outlinable.TryAddTarget(new OutlineTarget(rn));
                }

                outlines.Set(outlinable);

                obj.name = item.Name;
                CreateObject.Invoke(obj);
                return obj;
            }

        }
    }
}

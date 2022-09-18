using System;
using UnityEngine;
using UnityEngine.Events;

namespace Control
{
    public class Controller : MonoBehaviour
    {
        [System.Serializable]
        public class Control
        {
            [SerializeField] private Transform transform;
            [SerializeField] private Transform camera;
            [Space]
            [SerializeField] private float speed = 1f;
            [SerializeField] private float sence = 1f;
            [SerializeField] private float transitionsSpeed;
            [SerializeField] private float boostSpeed = 5;
            [Space]
            [SerializeField] private float currentSpeed;
            
            private float xAxis;


            public bool IsLook => Input.GetKey(KeyCode.Mouse1);
            public bool IsMove => Input.GetKey(KeyCode.Mouse2) || (Input.GetKey(KeyCode.LeftAlt) && IsLook);

            public float CalculatedSpeed
            {
                get
                {
                    float endSpeed = speed;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        endSpeed *= boostSpeed;
                    }

                    return endSpeed;
                }
            }


            public void Update()
            {
                Scroll();
                if (IsLook && !IsMove)
                {
                    Move();
                    Look();
                }else
                if (IsMove)
                {
                    SideMove();
                }
                else
                {
                    currentSpeed = 0;
                }
            }

            public void Scroll()
            {
                transform.Translate(camera.forward * boostSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * 100f, Space.World);
            }
            public void SideMove()
            {
                currentSpeed = Mathf.Lerp(currentSpeed, -CalculatedSpeed, transitionsSpeed * Time.deltaTime);
                
                
                transform.Translate(camera.up * currentSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"), Space.World);
                transform.Translate(camera.right * currentSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), Space.World);
            }
            private void Move()
            {
                

                currentSpeed = Mathf.Lerp(currentSpeed, CalculatedSpeed, transitionsSpeed * Time.deltaTime);
                
                transform.Translate(camera.forward * currentSpeed * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);
                transform.Translate(camera.right * currentSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), Space.World);
            }

            public void Look()
            {
                transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * sence;
                
                
                xAxis += Input.GetAxis("Mouse Y") * Time.deltaTime * sence;

                xAxis = Mathf.Clamp(xAxis, -90, 90);
                camera.localEulerAngles = new Vector3(-xAxis, 0, 0) ;
            }
        }

        public static UnityEvent OnLookClick = new UnityEvent();
        
        [SerializeField] private Control controller;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                OnLookClick.Invoke();
            }
            controller.Update();
        }
    }
}

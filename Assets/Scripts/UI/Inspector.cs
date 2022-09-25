using System;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class Inspector : MonoBehaviour
    {
        [System.Serializable]
        public class InputVector
        {

            [SerializeField] private TMP_InputField x, y, z;

            [SerializeField] private Vector3 lastVector;

            public UnityEvent<Vector3> OnChange = new UnityEvent<Vector3>();
            
            
            public void Init()
            {
                OnChange.RemoveAllListeners();
                
                x.onEndEdit.RemoveAllListeners();
                y.onEndEdit.RemoveAllListeners();
                z.onEndEdit.RemoveAllListeners();
                
                x.onEndEdit.AddListener(GenerateNewVector);
                y.onEndEdit.AddListener(GenerateNewVector);
                z.onEndEdit.AddListener(GenerateNewVector);
            }

            public void GenerateNewVector(string str)
            {
                Convert(new Vector3(float.Parse(x.text), float.Parse(y.text), float.Parse(z.text)));
            }
            
            public void Convert(Vector3 vector3)
            {
                x.text = vector3.x.ToString("F3");
                y.text = vector3.y.ToString("F3");
                z.text = vector3.z.ToString("F3");

                if (lastVector != vector3)
                {
                    OnChange.Invoke(vector3);
                }

                lastVector = vector3;
            }

            public Vector3 GetVector() => lastVector;
        }
        
        [SerializeField] private GizmoManager gizmo;
        [SerializeField] private Canvas canvas;

        [SerializeField] private TMP_Text nameText;

        [SerializeField] private InputVector pos, rot, scale;

        public void Init()
        {
            gizmo.OnSelectObject.AddListener(OnChangeSelection);
            
        }

        public void InputVectorReset(Transform objec)
        {
            pos.Init();
            rot.Init();
            scale.Init();
            
            pos.OnChange.AddListener(delegate(Vector3 arg0)
            {
                objec.transform.position = arg0;
            });
            rot.OnChange.AddListener(delegate(Vector3 arg0)
            {
                objec.transform.eulerAngles = arg0;
            });
            scale.OnChange.AddListener(delegate(Vector3 arg0)
            {
                objec.transform.localScale = arg0;
            });
        }
        public void OnChangeSelection(List<Transform> selection)
        {
            canvas.enabled = selection.Count == 1;

            if (canvas.enabled)
            {
                InputVectorReset(selection[0]);
                pos.Convert(selection[0].position);
                rot.Convert(selection[0].eulerAngles);
                scale.Convert(selection[0].localScale);
            }
        }


    }
}

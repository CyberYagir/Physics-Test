using System.Collections.Generic;
using Builder;
using UnityEngine;

public enum Keymap
{
    Tool_Move, Tool_Rotate, Tool_Scale, Tool_Space,
    
    Fly_Forward, Fly_Right, Fly_Up,
    
    Window_ItemsMananager,
}

public class KeyboardService : MonoBehaviour
{
    [System.Serializable]
    public class KeyAxis
    {
        [System.Serializable]
        public class Smoothstep
        {
            [SerializeField] private bool haveSmooth;
            [SerializeField] private float smoothSpeed = 2;

            public float SmoothSpeed => smoothSpeed;

            public bool HaveSmooth => haveSmooth;
        }

        [SerializeField] private Keymap key;
        [SerializeField] private KeyCode plus, minus;
        [SerializeField] private Smoothstep smoothstep;
        private float value;
        public Keymap Map => key;
        public float Value => value;

        public void Update()
        {
            int newValue = 0;
            if (Input.GetKey(plus))
            {
                newValue += 1;
            }
            if (Input.GetKey(minus))
            {
                newValue -= 1;
            }

            if (smoothstep.HaveSmooth)
            {
                value = Mathf.Lerp(value, newValue, smoothstep.SmoothSpeed * Time.deltaTime);
            }
            else
            {
                value = newValue;
            }
        }

        public bool Down()
        {
            return Input.GetKeyDown(plus) || Input.GetKeyDown(minus);
        }
    }


    [SerializeField] private List<KeyAxis> axies = new List<KeyAxis>();

    private Manager manager;
    private static KeyboardService Instance;

    public void Init(Manager manager)
    {
        this.manager = manager;

        Instance = this;
    }
    
    private void Update()
    {
        for (int i = 0; i < axies.Count; i++)
        {
            axies[i].Update();
        }
    }



    public static float GetAxis(Keymap map)
    {
        return Instance.axies.Find(x => x.Map == map).Value;
    }

    public static bool GetDown(Keymap map)
    {
        return Instance.axies.Find(x => x.Map == map).Down();
    }
}

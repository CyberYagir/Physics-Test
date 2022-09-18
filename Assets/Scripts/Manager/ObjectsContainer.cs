using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Additional;
using UnityEngine;

namespace Manager
{

    public class Manager : MonoBehaviour
    {
        public virtual void Init()
        {
            
        }


        public virtual Dictionary<string, Action> GetCommands()
        {
            return new Dictionary<string, Action>();
        }
    }
    public class ObjectsContainer : Manager
    {
        [SerializeField] private List<ObjectData> objects = new List<ObjectData>(100);
        public List<ObjectData> Objects => objects;

        public override void Init()
        {
            LoadObjects();        
        }


        public void LoadObjects()
        {
            objects.Clear();
            objects = Resources.LoadAll<ObjectData>("Objects").ToList();
        }
    }
}

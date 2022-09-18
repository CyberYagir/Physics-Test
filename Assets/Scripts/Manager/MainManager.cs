using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private List<Manager> managers;
        public List<Manager> Managers => managers;

        private void Awake()
        {
            foreach (var manager in managers)
            {
                manager.Init();
            }
        }
    }
}

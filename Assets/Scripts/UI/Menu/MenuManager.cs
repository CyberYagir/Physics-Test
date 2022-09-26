using System;
using UnityEngine;

namespace UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private FileSystem fileSystem;
        
        
        [SerializeField] private DesingnersDrawer desingnersDrawer;
        
        public FileSystem FileSystem => fileSystem;
        
        
        private void Awake()
        {
            fileSystem = new FileSystem();
            desingnersDrawer.Init(this);
        }
    }
}

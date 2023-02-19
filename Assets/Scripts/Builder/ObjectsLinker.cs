using System.Collections.Generic;
using UnityEngine;

namespace Builder
{
    public class ObjectsLinker : MonoBehaviour
    {
        [SerializeField] private List<Object> objects;

        public List<Object> Objects => objects;
    }
}

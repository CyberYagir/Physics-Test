using System;
using UnityEngine;

namespace UI
{
    public class CursorManager : MonoBehaviour
    {
        public enum CursorState
        {
            Pointer = 0, Hand = 1, Nope = 2
        }
        [SerializeField] private Texture2D[] cursors;

        private static CursorManager Instance;

        public void Awake()
        {
            Instance = this;
            SetCursor(CursorState.Pointer);
        }


        public static void SetCursor(CursorState state)
        {
            Cursor.SetCursor(Instance.cursors[(int) state], Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}

using System;
using UnityEngine;

namespace Playground
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Rigidbody platform;
        [SerializeField] private Transform p1, p2;
        [SerializeField] private int dir = -1;
        [SerializeField] private float speed;
        private void FixedUpdate()
        {

            if (dir == -1)
            {
                Move(p2.position);
            }
            else if (dir == 1)
            {
                Move(p1.position);
            }
            else
            {
                dir = 1;
            }
        }

        public void Move(Vector3 target)
        {
            platform.velocity = (target - platform.position).normalized *  speed;
            if (Vector3.Distance(target, platform.position) < 0.1f)
            {
                dir *= -1;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(p1.position, p2.position);
            
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(p1.position, platform.position);
            
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(p2.position, platform.position);
        }
    }
}

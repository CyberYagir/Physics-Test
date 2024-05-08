using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Playground
{
    public class MovingUnit : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string test = "YA EBLAN";
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private List<Transform> points;
        
        private int id;
        private Transform targetPoint;

        public NavMeshAgent Agent => agent;

        
        public List<Transform> GetPath() => points;

        public void SetPath(List<Transform> newPath) => points = newPath;
        
        private void Update()
        {
            if (targetPoint == null)
            {
                targetPoint = points[id];
                Agent.SetDestination(targetPoint.position);
                id++;
                if (id >= points.Count)
                {
                    id = 0;
                }
            }else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                targetPoint = null;
                return;
            }


            AnimationLogic();
            RotationLogic();
        }

        private void RotationLogic()
        {
            var dir = targetPoint.position - transform.position;
            dir.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Agent.angularSpeed * Time.deltaTime);
        }

        private void AnimationLogic()
        {
            var dir = transform.InverseTransformDirection(Agent.velocity);
            animator.SetFloat("Vertical", dir.z);
            animator.SetFloat("Horizontal", dir.x);
        }


        [SerializeField] private Mesh arrowMesh;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }

            if (points.Count > 2)
            {
                Gizmos.DrawLine(points[0].position, points.Last().position);
            }

            if (targetPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(targetPoint.position, 0.25f);
            }



            if (Agent.velocity.normalized != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.matrix = Matrix4x4.TRS(
                    transform.position + transform.forward/2f,
                    Quaternion.LookRotation(Agent.velocity.normalized) * Quaternion.Euler(Vector3.right * 90),
                    Vector3.one
                );

                Gizmos.DrawMesh(arrowMesh, 0, Vector3.zero, Quaternion.identity, Vector3.one/2f);
            }
        }
    }
}

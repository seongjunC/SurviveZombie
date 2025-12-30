using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Monster
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ZombieAttackController : MonoBehaviour
    {
        [Header("공격 범위")]
        [SerializeField] private float attackRadius = 1.5f;
        [SerializeField] private float attackAngle = 120f;
        [SerializeField] private LayerMask targetMask;
        
        [Header("데미지")]
        [SerializeField] private int damage = 20;

        [Header("시각 효과")]
        [SerializeField] private float indicatorDuration = 1f;
        [SerializeField] private Material indicatorMaterial;
        
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh viewMesh;
        private Coroutine drawCoroutine;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();


            viewMesh = new Mesh();
            viewMesh.name = "ViewMesh";
            meshFilter.mesh = viewMesh;
            
            if(indicatorMaterial is not null)
                meshRenderer.material = indicatorMaterial;
            
            meshRenderer.enabled = false;
        }

        public void ShowAttackIndicatior()
        {
            if (drawCoroutine is not null) return;
            drawCoroutine = StartCoroutine(DrawShape());
        }

        public void PerformAttack()
        {
            Debug.Log("Attack");
            Collider[] targets = Physics.OverlapSphere(transform.position, attackRadius, targetMask);

            HashSet<GameObject> hitTargets = new();
            
            foreach (var target in targets)
            {
                if (target.gameObject == gameObject) continue;
                
                if(hitTargets.Contains(target.gameObject)) return;

                if (!target.TryGetComponent<PlayerController>(out var player)) continue;
                
                var targetTransform = target.transform;
                var distance = Vector3.Distance(transform.position, targetTransform.position);
                if (distance > attackRadius) continue;
                
                var direction = (targetTransform.position - transform.position).normalized;
                
                if (!(Vector3.Angle(transform.forward, direction) < attackAngle / 2)) continue;
                    
                player.TakeDamage(damage);
                
                hitTargets.Add(target.gameObject);
                Debug.Log($"Damage : {damage}");
            }
        }
        
        private IEnumerator DrawShape()
        {
            meshRenderer.enabled = true;
            DrawFanShape();
            
            yield return new WaitForSeconds(indicatorDuration);
            
            meshRenderer.enabled = false;
            drawCoroutine = null;
        }

        private void DrawFanShape()
        {
            int segments = 50;
            int vertexCount = segments + 2;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[segments * 3];
            
            vertices[0] = Vector3.zero;
            
            float angle = attackAngle;
            float currentAngle = -angle / 2;
            float angleStep = angle / segments;
            
            float scaleCompensation = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
            if (scaleCompensation <= 0.01f) scaleCompensation = 1;
            
            float drawRadius = (attackRadius / scaleCompensation) * 0.9f;
            
            for (int i = 0; i <= segments; i++)
            {
                float rad = Mathf.Deg2Rad * currentAngle;
                Vector3 vertex = new Vector3(Mathf.Sin(rad) * drawRadius, 0.1f, Mathf.Cos(rad)* drawRadius);
                vertices[i + 1] = vertex;

                if (i < segments)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
                
                currentAngle += angleStep;
            }
            
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();

        }

        private void OnDrawGizmosSelected()
        {
            var leftDir = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward;
            var rightDir = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward;
            
            Gizmos.DrawRay(transform.position, leftDir * attackRadius);
            Gizmos.DrawRay(transform.position, rightDir * attackRadius);

            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        
        
    }
}
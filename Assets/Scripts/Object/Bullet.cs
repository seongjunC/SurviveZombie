using DefaultNamespace;
using Monster;
using UnityEngine;

namespace Object
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPooledObject
    {
        public float speed = 20f;
        public int damage = 10;
        private Rigidbody rb;
        
        public void OnObjectSpawn()
        {
            rb ??= GetComponent<Rigidbody>();
            
            rb.linearVelocity = transform.forward * speed;
            rb.angularVelocity = Vector3.zero;
            
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            
            CancelInvoke(nameof(Deactivate));
            Invoke(nameof(Deactivate), 3f);
        }

        public void Deactivate()
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            gameObject.SetActive(false);
        }

        public void DamageToEnemy(Collider other)
        {
            other.GetComponent<MonsterController>().TakeDamage(damage);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                DamageToEnemy(other);
                Deactivate();
            }
        }
    }
}
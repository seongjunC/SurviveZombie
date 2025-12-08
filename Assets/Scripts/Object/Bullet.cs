using DefaultNamespace;
using UnityEngine;

namespace Object
{
    public class Bullet : MonoBehaviour, IPooledObject
    {
        public float speed = 20f;
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
            gameObject.SetActive(false);
        }

        public void TakeDamage()
        {
            //TODO
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                TakeDamage();
                Deactivate();
            }
        }
    }
}
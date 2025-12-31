
using UnityEngine;

namespace pattern.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        [SerializeField] private bool _isDontDestroyOnLoad = true;
        
        
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindFirstObjectByType<T>();
                
                if (_instance != null) return _instance;
                
                GameObject singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();

                var singletonComponent = _instance as Singleton<T>;
                if (singletonComponent != null && singletonComponent._isDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(singletonObject);
                }
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance is not null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
            if (_isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        
    }
}
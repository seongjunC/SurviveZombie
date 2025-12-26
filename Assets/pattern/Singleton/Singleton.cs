
using UnityEngine;

namespace pattern.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _lock = new object();

        [SerializeField] private bool _isDontDestryOnLoad = true;
        
        
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = FindFirstObjectByType<T>();
                        if (_instance is null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            _instance = singletonObject.AddComponent<T>();

                            var singletonComponent = _instance as Singleton<T>;
                            if (singletonComponent != null && singletonComponent._isDontDestryOnLoad)
                            {
                                DontDestroyOnLoad(singletonObject);
                            }
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance is null)
            {
                _instance = this as T;
                if (_isDontDestryOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
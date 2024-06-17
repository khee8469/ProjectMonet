using UnityEngine;
using JJH;

namespace JJH
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        // 싱글톤을 awake 에서 초기화 시켜서 --> 다른 스크립트보다 manager 들이 start를 먼저 돌 수 있도록한다. 
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void CreateInstance()
        {
            T resource = Resources.Load<T>($"Manager/{typeof(T).Name}");
            instance = Instantiate(resource);
        }

        public static void ReleaseInstance()
        {
            if (instance == null)
                return;

            Destroy(instance.gameObject);
            instance = null;
        }
    }

}

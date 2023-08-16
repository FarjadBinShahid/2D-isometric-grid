
using UnityEngine;

namespace core.architecture
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields

        private static T instance;

        #endregion

        #region Properties

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        if(typeof(T).Name.Equals("DataManager"))
                        {
                            Debug.Log("Accessed DataManager while it is destroyed");
                        }

                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Methods

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

        protected virtual void Initialize()
        {

        }
        protected virtual void DeInitialize()
        {
            instance = null;
        }

        #endregion
    }
}
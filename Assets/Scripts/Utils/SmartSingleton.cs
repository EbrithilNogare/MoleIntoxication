using System.Collections;
using System.Collections.Generic;

using UnityEngine;

//taken from https://gamedev.stackexchange.com/a/151547


namespace UnityEngine
{
    /// <summary>
    /// Abstract class for creating Unity singletons easily.
    /// Usage: <see cref="T"/> : <see cref="SmartSingleton{T}"/>
    /// </summary>
    /// <typeparam name="T"><see cref="SmartSingleton{T}"/> script that should be instance</typeparam>
    public abstract class SmartSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        [SerializeField]
        private bool _persistent = false;

        public static bool Quitting { get; private set; }

        /// <summary>
        /// Singleton instance of <see cref="T"/>. If such instance doesn't exist it will create new one.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (Quitting)
                {
                    Debug.LogWarning($"[{nameof(SmartSingleton<T>)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return null;
                }
                lock (Lock)
                {
                    if (_instance != null)
                        return _instance;

                    return SetInstance();
                }
            }
        }

        private static T SetInstance()
        {
            var instances = FindObjectsOfType<T>();
            var count = instances.Length;
            if (count > 0)
            {
                if (count == 1)
                    return _instance = instances[0];
                Debug.LogWarning($"[{nameof(SmartSingleton<T>)}<{typeof(T)}>] There should never be more than one {nameof(SmartSingleton<T>)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);
                return _instance = instances[0];
            }

            Debug.Log($"[{nameof(SmartSingleton<T>)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
            return _instance = new GameObject($"({nameof(SmartSingleton<T>)}){typeof(T)}")
                       .AddComponent<T>();
        }

        protected virtual void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                SetInstance();
            }

            if (_persistent)
                DontDestroyOnLoad(gameObject);

        }

        private void OnApplicationQuit()
        {
            Quitting = true;
        }
    }
}


using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Basic Manager: Monobehaviour with a static instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AbstractManager<T> : MonoBehaviour where T : AbstractManager<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }
    }
}

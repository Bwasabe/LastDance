using UnityEngine;
using UnityEngine.SceneManagement;


public class MonoSingletonWithoutDontDestroy<T> : MonoBehaviour where T : MonoSingletonWithoutDontDestroy<T>
{
    private static bool _shuttingDown = false;
    private static object _locker = new object();
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning("[Instance] Instance" + typeof(T) + "is already destroyed. Returning null.");
                return null;
            }
            lock (_locker)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
 
    protected virtual void Start() {
        _shuttingDown = false;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _instance = FindObjectOfType<T>();
        
        if (_instance == null)
            _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
    }
    
    protected virtual void OnDestroy()
    {
        _shuttingDown = true;
    }
    protected virtual void OnApplicationQuit()
    {
        _shuttingDown = true;
    }
}

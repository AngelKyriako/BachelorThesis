using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class SingletonPhotonMono<T>: Photon.MonoBehaviour where T: Photon.MonoBehaviour {

    private const string containerName = "Managers";

    private static T _instance;

    private static object _lock = new object();

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }

            lock (_lock) {
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));
                    DontDestroyOnLoad(_instance.gameObject);
                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!");
                        return _instance;
                    }

                    if (_instance == null) {
                        GameObject singletonContainer;
                        if (!(singletonContainer = GameObject.Find(containerName))) {
                            singletonContainer = new GameObject(containerName);
                            DontDestroyOnLoad(singletonContainer);
                            singletonContainer.transform.position = Vector3.zero;
                            Debug.Log("[Singleton] GameObject " + singletonContainer.name + "was created as a container " +
                                      "of all Monobehavior singleton classes");
                        }
                        _instance = singletonContainer.AddComponent<T>();

                        Debug.Log("[Singleton] Added an instance of <" + typeof(T) +
                                  "> as a component of the '" + singletonContainer.name +
                                  "' object.");
                    }
                    else {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy() {
        applicationIsQuitting = true;
    }
}
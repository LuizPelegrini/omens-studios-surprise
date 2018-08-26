using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            //                if (_instance == null && FindObjectOfType<T>() == null)
            //                {
            //                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            //                }
            //                else
            //                if (FindObjectOfType<T>() != null)
            //                {
            //                    _instance = FindObjectOfType<T>();
            //                }

            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    /// <summary> Os Managers especificos de uma determinada cena devem setar essa variavel para FALSE. </summary>
    protected bool IsPersistentBetweenScenes = true;

    private bool alreadyUnsubscribed = false;

    protected virtual void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else if (IsPersistentBetweenScenes)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    protected virtual void OnEnable()
    {
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        if (!alreadyUnsubscribed)
        {
            UnsubscribeEvents();
        }
    }

    protected virtual void OnDestroy()
    {
        if (!alreadyUnsubscribed)
        {
            UnsubscribeEvents();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        if (!alreadyUnsubscribed)
        {
            UnsubscribeEvents();
        }
    }

    protected virtual void SubscribeEvents()
    {
        alreadyUnsubscribed = false;
    }

    protected virtual void UnsubscribeEvents()
    {
        alreadyUnsubscribed = true;
    }
}


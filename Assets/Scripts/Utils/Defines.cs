using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Health,
    Boost,
    Invincibillity
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject singletonObject = GameObject.Find(typeof(T).Name);

                if (null == singletonObject)
                {
                    singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
                else
                    instance = singletonObject.GetComponent<T>();
            }

            return instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }
}

public interface IInteractable
{
    public string GetInteractName();
    public string GetInteractPrompt();
    public void OnInteract();
}

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public interface IJumpable
{
    void Jumping(float jumpPower);
}
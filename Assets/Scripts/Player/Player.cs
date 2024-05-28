using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller { get; private set; }
    public PlayerCondition Condition { get; private set; }
    public MoveSO MoveData { get { return moveData; } }
    
    [SerializeField] private MoveSO moveData;

    private void Awake()
    {
        CharacterManager.Instance.MainPlayer = this;

        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
    }
}
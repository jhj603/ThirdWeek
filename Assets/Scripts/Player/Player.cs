using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller { get; private set; }
    public PlayerCondition Condition { get; private set; }
    public MoveSO MoveData { get { return moveData; } }
    public Transform DropPos { get { return dropPos; } }
    public ItemSO ItemData { get; set; }

    [SerializeField] private MoveSO moveData;
    [SerializeField] private Transform dropPos;

    public Action OnAddItemEvent;

    private void Awake()
    {
        CharacterManager.Instance.MainPlayer = this;

        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
    }
}
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    [SerializeField] private Condition health;
    [SerializeField] private Condition stemina;

    public Condition Health { get { return health; } }
    public Condition Stemina { get { return stemina; } }

    private void Start()
    {
        CharacterManager.Instance.MainPlayer.Condition.UICondition = this;
    }
}
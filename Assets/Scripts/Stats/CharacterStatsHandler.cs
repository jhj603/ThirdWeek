using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStat baseStat;

    public CharacterStat CurrentStat { get; private set; }

    private void Awake()
    {
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        MoveSO moveSO = null;

        if (null != baseStat.moveSO)
            moveSO = Instantiate(baseStat.moveSO);

        CurrentStat = new CharacterStat { moveSO = moveSO };

        CurrentStat.maxHealth = baseStat.maxHealth;
    }
}
using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    [Range(1, 100)] public int maxHealth;

    public MoveSO moveSO;    
}

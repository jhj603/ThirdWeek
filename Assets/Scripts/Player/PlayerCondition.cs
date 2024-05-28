using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions UICondition { get; set; }
    
    public Condition Health {  get { return UICondition.Health; } }
    public Condition Stemina { get { return UICondition.Stemina; } }

    public event Action OnTakeDamage;

    private void Update()
    {
        Stemina.Add(Stemina.PassiveValue * Time.deltaTime);

        if (0f == Health.CurValue)
            Die();
    }

    public void Heal(float amount)
    {
        Health.Add(amount);
    }

    public void EatStemina(float amount)
    {
        Stemina.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Á×À½");
    }

    public void TakePhysicalDamage(int damage)
    {
        Health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public bool UseStemina(float amount)
    {
        if (amount > Stemina.CurValue)
            return false;

        Stemina.Subtract(amount);

        return true;
    }
}
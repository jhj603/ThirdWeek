using System;
using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UIConditions UICondition { get; set; }
    
    public Condition Health { get { return UICondition.Health; } }
    public Condition Stemina { get { return UICondition.Stemina; } }

    public event Action OnTakeDamage;

    private Coroutine boostCor;
    private Coroutine InvincibillityCor;

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

    public void Boost(float amount)
    {
        if (null != boostCor)
            StopCoroutine(boostCor);

        boostCor = StartCoroutine(BoostPlayer(amount));
    }

    private IEnumerator BoostPlayer(float amount)
    {
        CharacterManager.Instance.MainPlayer.Controller.AddSpeed = 5f;

        yield return new WaitForSeconds(amount);

        CharacterManager.Instance.MainPlayer.Controller.AddSpeed = 0f;
    }

    public void Invincibillity(float amount)
    {
        if (null != InvincibillityCor)
            StopCoroutine(InvincibillityCor);

        InvincibillityCor = StartCoroutine(InvinciblePlayer(amount));
    }

    private IEnumerator InvinciblePlayer(float amount)
    {
        this.gameObject.layer = LayerMask.GetMask("Invincibillity");

        yield return new WaitForSeconds(amount);

        this.gameObject.layer = LayerMask.GetMask("Player");
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
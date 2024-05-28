using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float CurValue { get { return curValue; } }
    public float PassiveValue { get { return passiveValue; } }

    [SerializeField] private float passiveValue;
    [SerializeField] private float startValue;
    [SerializeField] private float maxValue;
    [SerializeField] private Image uiBar;

    private float curValue;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = (curValue / maxValue);
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0f);
    }
}
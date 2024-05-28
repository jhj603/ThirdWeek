using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.MainPlayer.Condition.OnTakeDamage += Flash;
    }

    private void Flash()
    {
        if (null != coroutine)
            StopCoroutine(coroutine);

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float curAlpha = startAlpha;

        while (0 < curAlpha)
        {
            curAlpha -= (startAlpha / flashSpeed) * Time.deltaTime;

            image.color = new Color(1f, 100f / 255f, 100f / 255f, curAlpha);

            yield return null;
        }

        image.enabled = false;
    }
}
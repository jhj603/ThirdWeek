using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    [SerializeField] private Image promptBackGround;
    [SerializeField] private TextMeshProUGUI promptNameText;
    [SerializeField] private TextMeshProUGUI promptDescText;

    public void OnInteract()
    {
        CharacterManager.Instance.MainPlayer.ItemData = itemData;
        CharacterManager.Instance.MainPlayer.OnAddItemEvent?.Invoke();

        Destroy(gameObject);
    }

    public void OnPrompt()
    {
        promptBackGround.gameObject.SetActive(true);

        promptNameText.text = itemData.displayName;
        promptDescText.text = itemData.description;
    }

    public void OffPrompt()
    {
        promptBackGround.gameObject.SetActive(false);
    }
}
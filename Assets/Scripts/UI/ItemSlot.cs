using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityTxt;

    private Outline outline;

    public ItemSO ItemData { get; set; }
    public UIInventory Inventory { get; set; }
    public int Index { get; set; }
    public bool Equipped { get; set; }
    public int Quantity { get; set; }

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = Equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = ItemData.icon;
        quantityTxt.text = Quantity > 1 ? Quantity.ToString() : string.Empty;

        if (null != outline)
            outline.enabled = Equipped;
    }

    public void Clear()
    {
        ItemData = null;
        icon.gameObject.SetActive(false);
        quantityTxt.text = string.Empty;
    }

    public void OnClickButton()
    {
        Inventory.SelectItem(Index);
    }
}
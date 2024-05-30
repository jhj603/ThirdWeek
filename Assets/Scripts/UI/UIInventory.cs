using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private Transform slotPanel;

    [Header("Select Item")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private TextMeshProUGUI selectedStatName;
    [SerializeField] private TextMeshProUGUI selectedStatValue;
    
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject equipBtn;
    [SerializeField] private GameObject unEquipBtn;
    [SerializeField] private GameObject dropBtn;

    private ItemSlot[] slots;

    private PlayerController controller;
    private PlayerCondition condition;
    private Transform dropPos;

    private ItemSO selectedItemData;
    private int selectedItemIndex = 0;

    private int curEquipIndex;

    private void Start()
    {
        Player mainPlayer = CharacterManager.Instance.MainPlayer;

        controller = mainPlayer.Controller;
        condition = mainPlayer.Condition;
        dropPos = mainPlayer.DropPos;

        controller.OnInventoryEvent += Toggle;

        CharacterManager.Instance.MainPlayer.OnAddItemEvent += AddItem;

        inventoryWindow.SetActive(false);

        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Index = i;
            slots[i].Inventory = this;
        }

        ClearSelectedItemWindow();
    }

    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
            inventoryWindow.SetActive(false);
        else
            inventoryWindow.SetActive(true);
    }

    private void AddItem()
    {
        ItemSO data = CharacterManager.Instance.MainPlayer.ItemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (null != slot)
            {
                ++slot.Quantity;
                UpdateUI();
                CharacterManager.Instance.MainPlayer.ItemData = null;

                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (null != emptySlot)
        {
            emptySlot.ItemData = data;
            emptySlot.Quantity = 1;
            
            UpdateUI();

            CharacterManager.Instance.MainPlayer.ItemData = null;

            return;
        }

        ThrowItem(data);

        CharacterManager.Instance.MainPlayer.ItemData = null;
    }

    private ItemSlot GetItemStack(ItemSO data)
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if ((data == slots[i].ItemData) && (data.maxStackAmount > slots[i].Quantity))
                return slots[i];
        }

        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if (null != slots[i].ItemData)
                slots[i].Set();
            else
                slots[i].Clear();
        }
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if (null == slots[i].ItemData)
                return slots[i];
        }

        return null;
    }

    private void ThrowItem(ItemSO data)
    {
        Instantiate(data.dropPrefab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    public void SelectItem(int index)
    {
        if (null == slots[index].ItemData)
            return;

        selectedItemData = slots[index].ItemData;
        selectedItemIndex = index;

        selectedItemName.text = selectedItemData.displayName;
        selectedItemDescription.text = selectedItemData.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItemData.consumables.Length; ++i)
        {
            selectedStatName.text += selectedItemData.consumables[i].type.ToString() + '\n';
            selectedStatValue.text += selectedItemData.consumables[i].value.ToString() + '\n';
        }

        useBtn.SetActive(selectedItemData.type == ItemType.Consumable);
        equipBtn.SetActive(selectedItemData.type == ItemType.Equipable && !slots[index].Equipped);
        unEquipBtn.SetActive(selectedItemData.type == ItemType.Equipable && slots[index].Equipped);

        dropBtn.SetActive(true);
    }

    public void OnUseButton()
    {
        if (ItemType.Consumable == selectedItemData.type)
        {
            for (int i = 0; i < selectedItemData.consumables.Length; ++i)
            {
                switch(selectedItemData.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItemData.consumables[i].value);
                        break;
                    case ConsumableType.Boost:
                        condition.Boost(selectedItemData.consumables[i].value);
                        break;
                    case ConsumableType.Invincibillity:
                        condition.Invincibillity(selectedItemData.consumables[i].value);
                        break;
                }
            }

            RemoveSelectedItem();
        }
    }

    private void RemoveSelectedItem()
    {
        --slots[selectedItemIndex].Quantity;

        if (0 >= slots[selectedItemIndex].Quantity)
        {
            selectedItemData = null;
            slots[selectedItemIndex].ItemData = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItemData);
        RemoveSelectedItem();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].Equipped)
            UnEquip(curEquipIndex);

        slots[selectedItemIndex].Equipped = true;
        curEquipIndex = selectedItemIndex;

        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        slots[index].Equipped = false;

        UpdateUI();

        if (selectedItemIndex == index)
            SelectItem(selectedItemIndex);
    }
}
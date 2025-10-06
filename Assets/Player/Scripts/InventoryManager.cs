using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform InventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    private Item nearbyItem;
    void Start()
    {
        for (int i = 0; i < InventoryPanel.childCount; i++)
        {
            slots.Add(InventoryPanel.GetChild(i).GetComponent<InventorySlot>());
        }
    }

    private int slotNum;
    void Update()
    {
        if (nearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(nearbyItem.item.Name + " added x" + nearbyItem.amount);
            AddItem(nearbyItem.item, nearbyItem.amount);
            Destroy(nearbyItem.gameObject);
            nearbyItem = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
            nearbyItem = item;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Item>() == nearbyItem)
            nearbyItem = null;
    }

    private void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item && slot.amount < _item.maxAmount)
            {
                slot.amount += _amount;
                slot.itemAmount.text = slot.amount.ToString();
                return;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.SetIcon(_item.Icon);
                slot.isEmpty = false;
                slot.itemAmount.text = _amount.ToString();
                slotNum = slots.IndexOf(slot);
                return;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class QuickSlotSync : MonoBehaviour
{
    public Transform EquipmentPanel;
    public Transform QuickSlotPanel;

    private List<InventorySlot> equipSlots = new List<InventorySlot>();
    private List<InventorySlot> quickSlots = new List<InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < EquipmentPanel.childCount; i++)
            equipSlots.Add(EquipmentPanel.GetChild(i).GetComponent<InventorySlot>());

        for (int i = 0; i < QuickSlotPanel.childCount; i++)
            quickSlots.Add(QuickSlotPanel.GetChild(i).GetComponent<InventorySlot>());
    }

    void Update()
    {
        int count = Mathf.Min(equipSlots.Count, quickSlots.Count);

        for (int i = 0; i < count; i++)
        {
            var equip = equipSlots[i];
            var quick = quickSlots[i];

            // обновляем только если что-то изменилось
            if (equip.item != quick.item ||
                equip.amount != quick.amount ||
                equip.isEmpty != quick.isEmpty)
            {
                SyncSlot(equip, quick);
            }
        }
    }

    void SyncSlot(InventorySlot source, InventorySlot target)
    {
        target.item = source.item;
        target.amount = source.amount;
        target.isEmpty = source.isEmpty;

        if (source.item != null)
        {
            target.SetIcon(source.item.Icon);
        }
        else
        {
            target.iconGO.GetComponent<Image>().sprite = null;
            target.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        if (target.itemAmount != null)
            target.itemAmount.text = target.amount > 1 ? target.amount.ToString() : "";
    }
}

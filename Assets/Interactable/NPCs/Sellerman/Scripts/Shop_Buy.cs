using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Buy : MonoBehaviour
{
    public PlayerParameters playerParam;
    public InventoryManager inventory;
    public ItemScriptableObject item;

    public int price;
    public Image image;
    public TextMeshProUGUI TMPtext;
    void Start()
    {
        image.sprite = item.Icon;
        TMPtext.text = price.ToString();
    }

    public void SellItemToPlayer()
    {
        if (playerParam._money >= price)
        {
            playerParam._money -= price;
            inventory.AddItem(item, 1);
            Debug.Log(item.name + " has been sold for " + price);
        }
        else
            return;
    }

    public void SellAmmo()
    {
        if (playerParam._money >= price)
        {
            playerParam._money -= price;
            playerParam._bullets++;
            Debug.Log(item.name + " has been sold for " + price);
        }
        else
            return;
    }
}

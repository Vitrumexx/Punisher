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
    private TMP_InputField amount;
    public int price;
    public Image image;
    public TextMeshProUGUI TMPtext;
    void Start()
    {
        amount = GetComponentInChildren<TMP_InputField>();
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
        else return;
    }

    public void SellAmmo()
    {
        if (amount == null)
        {
            Debug.LogWarning("InputField не найден!");
            return;
        }

        string textValue = amount.text.Trim();
        if (string.IsNullOrEmpty(textValue))
        {
            Debug.LogWarning("Поле ввода пустое!");
            return;
        }

        if (!int.TryParse(textValue, out int IntValue))
        {
            Debug.LogWarning("Некорректное значение, нужно ввести число!");
            return;
        }

        int totalPrice = price * IntValue;

        if (playerParam._money >= totalPrice)
        {
            playerParam._money -= totalPrice;
            playerParam._bullets += IntValue;
            Debug.Log($"{IntValue} {item.name} продано за {totalPrice}");
        }
        else Debug.Log("Недостаточно денег!");
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Sell : MonoBehaviour
{
    public PlayerParameters playerParam;
    public ItemScriptableObject item;

    public int price;
    public Image image;
    public TextMeshProUGUI TMPtext;
    void Start()
    {
        image.sprite = item.Icon;
        TMPtext.text = price.ToString();
    }

    public void BuyFromPlayer()
    {
        if (playerParam._supplies >= 1)
        {
            playerParam._money += playerParam._supplies;
            playerParam._supplies = 0;
            Debug.Log(item.name + " has been sold for " + price);
        }
        else
            return;
    }
}

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
    private TMP_InputField amount;
    void Start()
    {
        amount = GetComponentInChildren<TMP_InputField>();
        image.sprite = item.Icon;
        TMPtext.text = price.ToString();
    }

    public void BuyFromPlayer()
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

        if (playerParam._supplies >= IntValue)
        {
            playerParam._money += totalPrice;
            playerParam._supplies -= IntValue;
            Debug.Log(item.name + " has been sold for " + price);
        }
        else Debug.Log("Недостаточно материалов!");
    }
}

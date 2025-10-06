using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemAmount;
    public GameObject dragItem;
    void Start()
    {
        dragItem = transform.GetChild(0).gameObject;
        iconGO = dragItem.transform.GetChild(0).gameObject;
        itemAmount = dragItem.transform.GetChild (1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }
}

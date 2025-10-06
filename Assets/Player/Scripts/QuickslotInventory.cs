using UnityEngine;
using UnityEngine.UI;
public class QuickslotInventory : MonoBehaviour { // Объект у которого дети являются слотами
                                                 
    public Transform quickslotParent;
    public Transform rightHand; public Sprite selectedSprite;
    public Sprite notSelectedSprite; public GameObject obj;
    public InventoryManager inventoryManager;
    public MenuHandler menuHandler;
    public InventorySlot currentSlot;
    public ConsumeableScript consuemableScript;
    public int currentQuickslotID = 0;
    public int oldID; public bool isActive; 
    
    void Update()
    {
        QuickSlotSelect();
    }
    void QuickSlotSelect()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID >= quickslotParent.childCount - 1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                currentQuickslotID++;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            SlotToHand();
        }
        if (mw < -0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount - 1;
            }
            else
            {
                currentQuickslotID--;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            SlotToHand();
        } 
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (currentQuickslotID == i)
                {
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    }
                } 
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    currentQuickslotID = i;
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                }
                SlotToHand();
            }
        }
        if  (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item != null)
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>().item.isConsumeable && !menuHandler.MenuIsOpened && quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite)
            {
                if (Input.GetMouseButtonDown(0))
                    UseConsumeableItem();
            }
        }
    }
    void SlotToHand()
    {
        currentSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
        isActive = quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite;
        if (!isActive || currentSlot.item == null)
        {
            if (obj != null)
            {
                Destroy(obj);
                obj = null;
            }
            return;
        }
        if (isActive && currentSlot.item != null)
        {
            if (obj != null && oldID != currentQuickslotID)
            {
                Destroy(obj);
                obj = null;
            }
            if (obj == null)
            {
                CreateItem();
            }
        }
        oldID = currentQuickslotID;
    }
    void UseConsumeableItem()
    {
        consuemableScript = currentSlot.item.itemPrefab.GetComponent<ConsumeableScript>();
        consuemableScript.Modifiers();
        
        if (currentSlot.amount <= 1)
            quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        else
        {
            currentSlot.amount -= 1;
            currentSlot.itemAmount.text = currentSlot.amount.ToString();
        }
            
    }
    void CreateItem()
    {
        obj = Instantiate(currentSlot.item.itemPrefab);
        if (currentSlot.item is DistantWeapon weaponData)
        {
            var weaponLogic = obj.GetComponent<DistantWeaponLogic>();
            if (weaponLogic != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    weaponLogic.Init(player.transform.GetChild(0).GetComponent<Animator>(), weaponData);
                }
            }
        }
        Transform grip = obj.transform.Find("GripPoint");
        if (grip != null)
        {
            obj.transform.SetParent(rightHand, false);
            obj.transform.position = rightHand.position - (grip.position - obj.transform.position);
            obj.transform.rotation = rightHand.rotation * Quaternion.Inverse(grip.localRotation);
        }
        else
        {
            
            obj.transform.SetParent(rightHand, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
        if (obj.TryGetComponent<Collider>(out Collider col)) col.enabled = false;
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
    }
}
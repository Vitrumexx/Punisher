using UnityEditor;
using UnityEngine;

public enum ItemType {Trophy, Consumeable, Weapon, Instrument}
public class ItemScriptableObject : ScriptableObject
{

    public int maxAmount;
    public ItemType Type;
    public Sprite Icon;
    public GameObject itemPrefab;
    public string Name;
    public string Description;
    public bool isConsumeable;

    [Header("Consumeable Parameters")]
    public float RestoreHealth;
    public float FreezeStamina;
    public float IncreaseSpeed;
}

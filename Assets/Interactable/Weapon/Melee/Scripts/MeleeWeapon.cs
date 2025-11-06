    using UnityEngine;

public enum MeleeWeaponType { Sword, Axe, Hammer, Spear, Sythe }

[CreateAssetMenu(fileName = "Melee Weapon", menuName = "Items/New Melee Weapon")]
public class MeleeWeapon : ItemScriptableObject
{
    public MeleeWeaponType WeaponType;
    public float radius;
    public float damage;
    public float attackSpeed;
}

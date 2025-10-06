using UnityEngine;

public enum DistantWeaponType { Pistol, MachineGun, AssaultRifle, Rifle, RocketLauncher, Shotgun }
[CreateAssetMenu(fileName = "Distant Weapon", menuName = "Items/New Distant Weapon")]
public class DistantWeapon : ItemScriptableObject
{
    public DistantWeaponType WeaponType;
    public int clipAmmo;
    public int maxAmmo;
    public float damage;
    public float shootingSpeed;
    public Animator anim;
}

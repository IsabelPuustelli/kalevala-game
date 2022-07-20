using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    [Header("Weapon")]
    [Header("World model")]
    public GameObject gameObject;
    public WeaponType weaponType;
    public bool interaction;

    [Header("Weapon stats")]
    public int damage;
    public float staminaCost;
    public float criticalChance, criticalMultiplier;

    [Header("Animation settings")]
    public string idleAnimation;
    public string[] attackAnimations;
    public string[] comboAnimations;

    [Header("Audio")]
    public Sound[] attackSounds;
    public Sound[] swingSounds;

    public override void Use()
    {
        // Equip weapon
        // Unequip weapon
    }

    public enum WeaponType { Sword, Shield }
}
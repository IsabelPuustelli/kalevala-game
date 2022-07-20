using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    // Currently equipped weapons
    [SerializeField] Transform leftWeaponHolder, rightWeaponHolder;
    [SerializeField] WeaponObject leftWeapon, rightWeapon;

    GameObject _leftWeaponObject, _rightWeaponObject;

    AnimationHandler _anim;
    PlayerController _player;
    Hand _lastActiveHand;

    bool heldLeft, heldRight;
    int comboAttackIndex = 0;

    void Awake()
    {
        _anim = GetComponent<AnimationHandler>();
        _player = GetComponent<PlayerController>();
    }

    void Start()
    {
        // Equip weapons
        EquipWeapon(leftWeapon, Hand.Left);
        EquipWeapon(rightWeapon, Hand.Right);
    }

    void Update()
    {
        if (leftWeapon.weaponType == WeaponObject.WeaponType.Shield && !_anim.IsInteracting)
            _anim.SetBool("isBlocking", heldLeft);
        else
            _anim.SetBool("isBlocking", false);
    }


    public void OnAttackLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_anim.IsInteracting)
        {
            Attack(Hand.Left);
            heldLeft = true;

        }
        if (ctx.canceled)
            heldLeft = false;

    }

    public void OnAttackRight(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_anim.IsInteracting)
        {
            Attack(Hand.Right);
            heldRight = true;
        }
        if (ctx.canceled)
            heldRight = false;
    }


    public void Attack(Hand hand)
    {
        var weapon = hand == Hand.Left ? leftWeapon : rightWeapon;
        if (!weapon) Debug.LogError("No weapon equipped");
        if (weapon.staminaCost > _player.GetStamina) return;

        comboAttackIndex++;

        if (comboAttackIndex % 3 == 0)
        {
            if (weapon.comboAnimations.Length > 0)
            {
                string animation = weapon.attackAnimations[Random.Range(0, weapon.attackAnimations.Length)];
                _anim.PlayAnimation(animation, weapon.interaction);
                _player.StaminaCost(weapon.staminaCost * 1.25f);
                return;
            }
        }

        // pick random attack animation
        if (weapon.attackAnimations.Length > 0)
        {
            string animation = weapon.attackAnimations[Random.Range(0, weapon.attackAnimations.Length)];
            _anim.PlayAnimation(animation, weapon.interaction);
            _player.StaminaCost(weapon.staminaCost);
        }
    }

    void Blocking()
    {
        if (_anim.IsInteracting)
            _anim.SetBool("isBlocking", false);
        else if (heldLeft)
            _anim.SetBool("isBlocking", true);
    }

    public void EquipWeapon(WeaponObject newWeapon, Hand hand)
    {
        (hand == Hand.Left ? ref leftWeapon : ref rightWeapon) = newWeapon;

        var obj = Instantiate(newWeapon.gameObject, hand == Hand.Left ? leftWeaponHolder : rightWeaponHolder) as GameObject;
        (hand == Hand.Left ? ref _leftWeaponObject : ref _rightWeaponObject) = obj;

        Debug.Log($"Equipped {newWeapon.name} to {hand} hand");
    }

    public void UnequipWeapon(Hand hand)
    {
        (hand == Hand.Left ? ref leftWeapon : ref rightWeapon) = null;
        // TODO: Destroy weapon object

    }

    public WeaponObject GetWeapon(Hand hand)
    {
        return (hand == Hand.Left ? leftWeapon : rightWeapon);
    }

    public Collider GetWeaponCollider()
    {
        return (_lastActiveHand == Hand.Left ? _leftWeaponObject.GetComponent<Collider>() : _rightWeaponObject.GetComponent<Collider>());
    }
}

public enum Hand { Left, Right }

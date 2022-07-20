using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Move, Look;
    [HideInInspector]
    public bool Roll, Jump, LockOn, UseItem, Interact;
    [HideInInspector]
    public bool AttackL, StrongAttackL, AttackR, StrongAttackR;


    public void OnMove(InputValue value) => Move = value.Get<Vector2>();
    public void OnLook(InputValue value) => Look = value.Get<Vector2>();
    public void OnRoll(InputValue value) => Roll = value.isPressed;
    public void OnJump(InputValue value) => Jump = value.isPressed;
    public void OnLockCamera(InputValue value) => LockOn = value.isPressed;
    public void OnUseItem(InputValue value) => UseItem = value.isPressed;
    public void OnInteract(InputValue value) => Interact = value.isPressed;
    public void OnAttackL(InputValue value) => AttackL = value.isPressed;
    public void OnStrongAttackL(InputValue value) => StrongAttackL = value.isPressed;
    public void OnAttackR(InputValue value) => AttackR = value.isPressed;
    public void OnStrongAttackR(InputValue value) => StrongAttackR = value.isPressed;


    // set mouse cursor
    public void SetCursorLock(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !value;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationHandler : MonoBehaviour
{
    Animator _animator;
    WeaponHandler _weaponHandler;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponHandler = GetComponent<WeaponHandler>();
    }

    public void PlayAnimation(string animation, bool isInteracting)
    {
        Debug.Log("Playing animation: " + animation);
        _animator.SetBool("isInteracting", isInteracting);
        _animator.CrossFade(animation, 0.2f);
    }


    public void OpenDamageCollider()
    {
        Debug.Log("Opening damage collider");
        var col = _weaponHandler.GetWeaponCollider();
        if (col) col.enabled = true;
    }

    public void CloseDamageCollider()
    {
        var col = _weaponHandler.GetWeaponCollider();
        if (col) col.enabled = false;
    }

    public void SetFloat(string name, float value, bool smooth) => _animator.SetFloat(name, value, smooth ? 0.1f : 0, Time.smoothDeltaTime);

    public float GetFloat(string name) => _animator.GetFloat(name);

    public void SetBool(string name, bool value) => _animator.SetBool(name, value);

    public void SetTrigger(string name) => _animator.SetTrigger(name);

    public bool IsInteracting { get => _animator.GetBool("isInteracting"); }
}

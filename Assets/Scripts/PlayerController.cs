using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour, ISaveable
{
    [Header("Basic")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRegen = 1f, staminaRegenDelay;
    [SerializeField] float staminaJumpCost = 25f, staminaRollCost = 15f;

    [Header("Locomotion")]
    [SerializeField] float movementSpeedMax;
    [SerializeField] float movementSpeedMin;
    [SerializeField] float jumpAmount = 3f;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;
    public GameObject _cameraTarget;
    [SerializeField] float minCameraLockDistance = 0.5f, maxCameraLockDistance = 20f;

    [SerializeField] float topClamp, bottomClamp;

    [Header("Audio")]
    [SerializeField] AudioClip[] footSteps;

    [Header("Misc")]
    [SerializeField] GameObject bloodSplatter;

    Vector2 _movement, _look;

    AnimationHandler _anim;
    Rigidbody _rb;
    WeaponHandler _weaponHandler;
    Transform _cam;
    AudioSource _audioSource;
    Vector3 _targetDir;

    [Header("Stats")]
    public float _health, _stamina, _staminaRegenTimer;
    float _cmTargetYaw, _cmTargetPitch;         // Cinemachine target angles
    bool _isTargetLocked;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<AnimationHandler>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _cam = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _health = maxHealth;
        _stamina = maxStamina;
        UserInterface.Instance.UpdateBar(Bar.Health, _health / maxHealth);
        UserInterface.Instance.UpdateBar(Bar.Stamina, _stamina / maxStamina);
    }

    void Update()
    {
        if (_health > 0)
        {
            if (!_isTargetLocked) Movement();
            else LockedMovement();
        }
        else
            Die();

        Stamina();
    }

    void LateUpdate() => CameraRotation();

    public void OnMove(InputAction.CallbackContext ctx) => _movement = ctx.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext ctx) => _look = ctx.ReadValue<Vector2>();

    void Movement()
    {
        _targetDir = (_cam.forward * _movement.y + _cam.right * _movement.x).normalized;
        _targetDir.y = 0;

        // rotate player
        if (_targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _movement.magnitude * 2f);
        }

        // apply movement based on rotation
        Vector3 movementDirection = transform.forward * _movement.magnitude;
        movementDirection *= Mathf.Lerp(movementSpeedMin, movementSpeedMax, _movement.magnitude);
        movementDirection.y = _rb.velocity.y;

        _rb.velocity = movementDirection;

        // set animation floats
        _anim.SetFloat("movement", _movement.magnitude, (_anim.GetFloat("movement") > 0.01f) ? true : false);
        _anim.SetFloat("horizontal", _movement.x, true);
        _anim.SetFloat("vertical", _movement.y, true);
    }

    // Locked locomotion when target is locked
    void LockedMovement()
    {
        if (_anim.IsInteracting) return;

        _targetDir = (virtualCamera.LookAt.transform.position - transform.position).normalized;
        _targetDir.y = 0;

        if (_targetDir == Vector3.zero)
            _targetDir = transform.forward;
        else if (Vector3.Distance(virtualCamera.LookAt.transform.position, transform.position) < minCameraLockDistance)
            _targetDir = _targetDir * -1;

        transform.rotation = Quaternion.LookRotation(_targetDir);

        // move forward based on input
        Vector3 movementDirection = new Vector3(_movement.x, 0, _movement.y);
        movementDirection *= Mathf.Lerp(movementSpeedMin, movementSpeedMax * 0.7f, _movement.magnitude);
        movementDirection = transform.TransformDirection(movementDirection);
        movementDirection.y = _rb.velocity.y;
        _rb.velocity = movementDirection;

        // set animation floats
        _anim.SetFloat("horizontal", _movement.x, true);
        _anim.SetFloat("vertical", _movement.y, true);
    }

    void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_look.sqrMagnitude >= .01f)
        {
            _cmTargetYaw += _look.x * Time.deltaTime;
            _cmTargetPitch += _look.y * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cmTargetYaw = ClampAngle(_cmTargetYaw, float.MinValue, float.MaxValue);
        _cmTargetPitch = ClampAngle(_cmTargetPitch, bottomClamp, topClamp);

        // apply the rotation to the transform of the camera
        _cameraTarget.transform.rotation = Quaternion.Euler(_cmTargetPitch, _cmTargetYaw, 0.0f);
    }

    public void OnRoll(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_anim.IsInteracting && _stamina > staminaRollCost)
        {
            // if we are locked to the target, use 8-directional roll else roll forward
            if (_movement.magnitude > 0.2f)
                _anim.PlayAnimation(_isTargetLocked ? "DirectionalRoll" : "Roll", true);

            // if we are not moving, we can only backstep
            else _anim.PlayAnimation("Backstep", true);
            StaminaCost(staminaRollCost);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !_anim.IsInteracting && _rb.velocity.y < .1f && _stamina > staminaJumpCost)
        {
            if (_movement.magnitude > 0f)
            {
                _anim.PlayAnimation("MovingJump", true);
                _rb.AddForce((transform.up + transform.forward) * jumpAmount / 2, ForceMode.Impulse);
            }
            else
            {
                _anim.PlayAnimation("Jump", true);
                _rb.AddForce(transform.up * jumpAmount, ForceMode.Impulse);
            }
            StaminaCost(staminaJumpCost);
        }
    }

    public void OnLockTarget(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_isTargetLocked)
            {
                _isTargetLocked = false;
                virtualCamera.LookAt = null;
            }
            else
            {
                var target = GetClosestEnemy(maxCameraLockDistance);
                if (target)
                {
                    virtualCamera.LookAt = target.transform;
                    _isTargetLocked = true;
                }
            }
            _anim.SetBool("isLockedOn", _isTargetLocked);
        }
    }

    void Stamina()
    {
        if (_anim.IsInteracting) return;

        // Stamina regeneration
        if (_staminaRegenTimer > 0f)
        {
            _staminaRegenTimer -= Time.deltaTime;
            return;
        }
        else if (_stamina < maxStamina)
        {
            _stamina += staminaRegen * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0f, maxStamina);
            UserInterface.Instance.UpdateBar(Bar.Stamina, _stamina / maxStamina);
        }

    }

    GameObject GetClosestEnemy(float maxDistance)
    {
        GameObject closestEnemy = null;
        float closestDistance = maxDistance;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    //public void Footstep() => _audioSource.PlayOneShot(footSteps[UnityEngine.Random.Range(0, footSteps.Length)], 0.05f);

    public void TakeDamage(float a)
    {
        Debug.Log("Player took " + a + " damage");
        _health -= a;
        UserInterface.Instance.UpdateBar(Bar.Health, _health / maxHealth);
        bloodSplatter.SetActive(true);
    }

    public void StaminaCost(float a)
    {
        _stamina -= a;
        _staminaRegenTimer = staminaRegenDelay;
        UserInterface.Instance.UpdateBar(Bar.Stamina, _stamina / maxStamina);
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public float GetStamina { get => _stamina; }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        this.transform.rotation = data.playerRotation;

    }
    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
        data.playerRotation = this.transform.rotation;
    }
    void Die()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MENU");
    }
}
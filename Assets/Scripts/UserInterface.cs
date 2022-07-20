using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;

    [SerializeField] TMP_Text _interactionText, _largePromptText, _smallPromptText;
    [SerializeField] Slider _healthSlider, _staminaSlider;

    bool _interact = false;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        _interactionText.text = "";
        _largePromptText.text = "";
        _smallPromptText.text = "";
        _interactionText.gameObject.SetActive(false);
    }


    public void UpdateBar(Bar bar, float value)
    {
        switch (bar)
        {
            case Bar.Health:
                Debug.Log("Updating health bar" + value);
                LeanTween.value(_healthSlider.gameObject, _healthSlider.value, value, .5f).setOnUpdate((float val) => _healthSlider.value = val);
                break;
            case Bar.Stamina:
                LeanTween.value(_staminaSlider.gameObject, _staminaSlider.value, value, .5f).setOnUpdate((float val) => _staminaSlider.value = val);
                break;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx) => _interact = ctx.performed;

    public bool InteractionPrompt(string s)
    {
        _interactionText.gameObject.SetActive(!s.Equals(""));
        _interactionText.text = s;

        if (_interact)
        {
            _interact = false;
            return true;
        }

        return false;
    }

    public void LargePrompt(string s, float t)
    {
        _largePromptText.gameObject.SetActive(true);
        _largePromptText.text = s;

        LeanTween.value(_largePromptText.gameObject, a => _largePromptText.alpha = a, 1f, 0f, t);
        LeanTween.scale(_largePromptText.gameObject, Vector3.one * 1.5f, t).setEaseInOutQuad().setOnComplete(() =>
        {
            _largePromptText.text = "";
            _largePromptText.gameObject.SetActive(false);
        });
    }

    public void SmallPrompt(string s, float t)
    {
        _smallPromptText.gameObject.SetActive(true);
        _smallPromptText.text = s;

        LeanTween.value(_smallPromptText.gameObject, a => _smallPromptText.alpha = a, 1f, 0f, t);
        LeanTween.scale(_largePromptText.gameObject, Vector3.one * 1.5f, t).setEaseInOutQuad().setOnComplete(() =>
        {
            _smallPromptText.text = "";
            _smallPromptText.gameObject.SetActive(false);
        });
    }
}

public enum Bar { Health, Stamina };

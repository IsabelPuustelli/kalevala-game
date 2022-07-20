using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Range(0.1f, 5f)]
    [SerializeField] float interactionRange = 2f;
    [SerializeField] Vector3 offset = new Vector3(0f, 1f, 0f);
    [SerializeField] string interactionText = "";
    [SerializeField] bool triggered = false, oneshot = false;

    [Header("Event")]
    [SerializeField] UnityEvent onInteract;

    SphereCollider _collider;

    void Awake()
    {
        // create new collider if none exists
        _collider = gameObject.AddComponent<SphereCollider>();

        // set collider properties
        _collider.isTrigger = true;
        _collider.radius = interactionRange;

    }

    void OnTriggerStay(Collider other)
    {
        if (triggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            if (UserInterface.Instance.InteractionPrompt(interactionText))
            {
                onInteract.Invoke();
                if (oneshot) triggered = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            UserInterface.Instance.InteractionPrompt("");
    }

}

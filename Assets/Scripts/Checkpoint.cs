using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] bool _discovered = false;

    Animator _animator;

    void Awake() => _animator = GetComponent<Animator>();

    void Discover()
    {
        _discovered = true;
        _animator.SetBool("lit", _discovered);
        UserInterface.Instance.LargePrompt("Bonfire lit", 1.5f);
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        string text = (_discovered) ? "Rest" : "Lit a fire";
        if (UserInterface.Instance.InteractionPrompt(text))
        {
            if (!_discovered)
                Discover();
            else
                GameManager.Instance.RestAtCheckpoint();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            UserInterface.Instance.InteractionPrompt("");
    }
}

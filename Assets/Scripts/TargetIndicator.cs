using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    Transform _lookAt;
    Camera _cam;
    PlayerController _player;
    Image _image;

    void Awake()
    {
        _cam = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _image = GetComponentInChildren<Image>();
    }

    void LateUpdate()
    {
        _lookAt = _player.virtualCamera.LookAt;

        if (_lookAt)
        {
            transform.position = _lookAt.position + _lookAt.gameObject.GetComponent<Collider>().bounds.extents.y * Vector3.up;
            transform.rotation = Quaternion.LookRotation(_player.transform.position - transform.position);
            _image.enabled = true;
        }
        else _image.enabled = false;
    }

}

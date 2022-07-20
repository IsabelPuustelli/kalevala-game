using UnityEngine;
using Cinemachine;

public
class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera _cam;
    Rigidbody _rb;


    void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _rb = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();
    }
    void LateUpdate()
    {
        var noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (_rb.velocity.magnitude > 0.1f)
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0.15f, Time.deltaTime * 5f);
        else
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0f, Time.deltaTime * 5f);

    }
}

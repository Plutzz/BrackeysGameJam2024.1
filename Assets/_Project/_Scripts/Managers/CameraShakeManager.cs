using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    [SerializeField] private float globalShakeForce;
    private CinemachineImpulseSource defaultImpulseSource;

    private void Start()
    {
        defaultImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void CameraShake() {
        CameraShake(defaultImpulseSource);
    }

    public void CameraShake(CinemachineImpulseSource impulse) {
        impulse.GenerateImpulseWithForce(globalShakeForce);
    }
}

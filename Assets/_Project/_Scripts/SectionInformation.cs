using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SectionInformation : MonoBehaviour
{
    public Vector3 playerSpawnPosition;
    public Vector3 doorSpawnPosition;
    [HideInInspector] public CinemachineVirtualCamera usedCamera;
    public bool ignoreSectionInformation;

    private void Start()
    {
        usedCamera = GetComponent<CinemachineVirtualCamera>();
    }
}

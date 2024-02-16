using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class SectionConnector : MonoBehaviour
{
    [SerializeField] private Transform sideOne;
    [SerializeField] private Transform sideTwo;

    [SerializeField] private CinemachineVirtualCamera cameraOne;
    [SerializeField] private CinemachineVirtualCamera cameraTwo;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") {
            return;
        }

        Vector3 playerPosition = collision.transform.position;

        if (Vector3.Distance(playerPosition, sideOne.position) < Vector3.Distance(playerPosition, sideTwo.position))
        {
            LevelManager.Instance.SetRespawnLocation(sideOne.position);
            cameraOne.enabled = true;
            cameraTwo.enabled = false;
        }
        else {
            LevelManager.Instance.SetRespawnLocation(sideTwo.position);
            cameraOne.enabled = false;
            cameraTwo.enabled = true;
        }
    }
}

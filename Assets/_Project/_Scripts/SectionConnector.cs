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
        bool respawnWithDoor = PlayerController.Instance.hasDoor;

        if (Vector3.Distance(playerPosition, sideOne.position) < Vector3.Distance(playerPosition, sideTwo.position))
        {
            LevelManager.Instance.SetRespawnLocation(sideOne.position, Vector3.zero, cameraOne);
            cameraOne.enabled = true;
            cameraTwo.enabled = false;
        }
        else {
            LevelManager.Instance.SetRespawnLocation(sideTwo.position, Vector3.zero, cameraTwo);
            cameraOne.enabled = false;
            cameraTwo.enabled = true;
        }
    }
}

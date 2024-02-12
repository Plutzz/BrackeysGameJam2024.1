using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipTime;

    private Coroutine turnCoroutine;
    
    private PlayerController playerController;

    //private bool facingRight;

    private void Awake()
    {
        playerController = playerTransform.gameObject.GetComponent<PlayerController>();
        transform.SetParent(null);
    }

    private void Update()
    {
        transform.position = playerTransform.position;  
    }

    public void CallFlip() {
        turnCoroutine = StartCoroutine(Flip());
    }

    private IEnumerator Flip() { 
        float startRoation = transform.localEulerAngles.y;
        float endRotation = playerController.isFacingRight ? 0 : 180;
        float yRotation = 0;

        float elapsedTime = 0;
        while (elapsedTime < flipTime) { 
            
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRoation, endRotation, (elapsedTime / flipTime));
            transform.rotation = Quaternion.Euler(0, yRotation, 0);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, endRotation, 0);
    }

}

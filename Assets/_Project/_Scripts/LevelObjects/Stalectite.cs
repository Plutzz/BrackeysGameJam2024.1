using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalectite : MonoBehaviour
{
    [SerializeField] private float rechargeTime;
    [SerializeField] private GameObject fallingStalectite;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LayerMask dropTriggers;

    private Animator animator;

    private bool stalectiteReady;

    private void Start()
    {
        animator = GetComponent<Animator>();
        stalectiteReady = true;
    }

    private void Update()
    {
        if (!stalectiteReady) { return; }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100, dropTriggers);
        if (hit.collider != null)
        {
            DropStalectite();
        }
    }

    public void DropStalectite() {
        Instantiate(fallingStalectite, spawnPoint.position, Quaternion.identity);
        stalectiteReady = false;
        animator.SetTrigger("drop");
        AudioManager.Instance?.PlaySound(AudioManager.Sounds.Stalactite);
    }

    public void finishedRecharge() { 
        stalectiteReady = true;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 100);
    }
}

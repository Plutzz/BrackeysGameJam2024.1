using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "GoatSpecial", menuName = "Specials/GoatSpecial")]
public class GoatSpecial : SpecialMoves
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireSpeed;

    //Could create a class attached to special game objects to store all of this stuff
    //[SerializeField] private CinemachineImpulseSource impulse;
    
    public override void UseSpecial(GameObject user)
    {
        Debug.Log("Used Goat Special");

        GameObject goat = Instantiate(projectile, user.transform.position + user.transform.right * 2f, Quaternion.identity);
        Rigidbody2D rb = goat.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = user.transform.right * fireSpeed;
        }

        CameraShakeManager.Instance.CameraShake();
    }

    
}

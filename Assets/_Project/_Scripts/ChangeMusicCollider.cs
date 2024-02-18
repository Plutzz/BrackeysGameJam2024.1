using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySong(AudioManager.Songs.SeaOfCircuts);
        }
    }
}

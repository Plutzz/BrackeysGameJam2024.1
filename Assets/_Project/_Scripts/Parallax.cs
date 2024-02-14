using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private float startPosX, startPosY;
    [SerializeField] GameObject Cam;
    [SerializeField] private float parallaxEffectX = 0;
    [SerializeField] private float parallaxEffectY = 0;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    private void FixedUpdate()
    {
        float _distX = Cam.transform.position.x * parallaxEffectX / 100;
        float _distY = Cam.transform.position.y * parallaxEffectY / 100;
        transform.position = new Vector3(startPosX + _distX, startPosY + _distY, transform.position.z);

    }
}

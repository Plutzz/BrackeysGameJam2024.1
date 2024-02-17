using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float Strength;
    [SerializeField] private float Scale;

    void Update()
    {
        mat.SetFloat("_Strength", Strength);
        mat.SetFloat("_Scale", Strength);
    }
}

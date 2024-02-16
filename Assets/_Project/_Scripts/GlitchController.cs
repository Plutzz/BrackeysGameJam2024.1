using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public Material mat;
    public float Strength;
    public float Scale;

    void Update()
    {
        mat.SetFloat("_Strength", Strength);
        mat.SetFloat("_Scale", Strength);
    }
}

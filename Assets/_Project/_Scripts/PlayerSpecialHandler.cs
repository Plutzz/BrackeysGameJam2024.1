using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialHandler : MonoBehaviour
{
    [SerializeField] private SpecialMoves forwardSpecial;

    public void UseSpecial() {
        forwardSpecial.UseSpecial(gameObject);
    }
}

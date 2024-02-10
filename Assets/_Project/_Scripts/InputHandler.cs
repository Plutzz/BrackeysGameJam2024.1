using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// Maybe make this a persistent class so you don't have to keep setting it up in every scene?
public class InputHandler : Singleton<InputHandler>
{
    [Header("Action Maps")]
    [SerializeField] private bool playerMap;
    [SerializeField] private bool universalMap;
    [SerializeField] private bool UIMap;
    [SerializeField] private bool dialogueMap;

    public PlayerInputActions playerInputActions { get; private set; }

    [Header("Player Input")]
    public Vector2 inputVector;     //Player's input vector for movement
    public bool isInteracting;      //is true if player is pressing interact button
    protected override void Awake()
    {
        base.Awake();
        playerInputActions = new PlayerInputActions();

        // Enables player controller inputs
        // These inputs will be relinqushed when the player has dialog or other UI open
        playerInputActions.Player.Enable();

        // Enables Universal inputs
        // These inputs are never meant to be relinquished
        playerInputActions.Universal.Enable();
    }

    private void Update()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        isInteracting = playerInputActions.Player.Interact.ReadValue<float>() == 1;

        // Gets the state of every input map (so we can see what is active easier through the inspector)
        playerMap = playerInputActions.Player.enabled;
        universalMap = playerInputActions.Universal.enabled;
        UIMap = playerInputActions.UI.enabled;
        dialogueMap = playerInputActions.Dialogue.enabled;
    }
}

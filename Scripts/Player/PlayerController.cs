using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs PlayerInputs { get; private set; }
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }
    public PlayerInputs.UIActions UIActions { get; private set; }

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
        PlayerActions = PlayerInputs.Player;
        UIActions = PlayerInputs.UI;
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
    }
}

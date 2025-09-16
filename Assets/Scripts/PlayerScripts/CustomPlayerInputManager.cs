using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CustomPlayerInputManager : PlayerInputManager
{
    public List<PlayerData> data = new List<PlayerData> ();
    public Camera backupCamera;

    private int activePlayers;

    private void Start()
    {
        if(backupCamera == null)
        {
            backupCamera = Camera.main;
        }
    }

    public void PlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
        Debug.Log($"Player joined: {playerInput.playerIndex} with device: {playerInput.devices[0].name}");

        playerInput.GetComponent<PlayerController>().Setup(data[playerInput.playerIndex]);

        activePlayers++;

        HandleBackupCamera();
    }

    public void PlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player left: {playerInput.playerIndex} with device: {playerInput.devices[0].name}");

        playerInput.GetComponent <PlayerController>().Packout(data[playerInput.playerIndex]);

        activePlayers--;

        HandleBackupCamera() ;
    }

    private void HandleBackupCamera()
    {
        if(backupCamera == null)
        {
            return; //to avoid an annoying error when exiting play mode
        }

        if(activePlayers == 0)
        {
            backupCamera.gameObject.SetActive(true);
        }
        else
        {
            backupCamera.gameObject.SetActive(false);
        }
    }
    
}

[System.Serializable]
public class PlayerData
{
    public GameObject playerObject;
    public PawnController ghost;
}

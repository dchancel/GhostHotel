using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public SO_PlayerData[] playerInfo = new SO_PlayerData[2];

    public PlayerData[] playerData = new PlayerData[2];

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

            for (int i = 0; i < playerData.Length; i++)
            {
                ApplyPlayerData(playerData[i], playerInfo[i]);
                playerData[i].playerObject.SetActive(false);
            }

        if (CustomPlayerInputManager.instance != null)
        {
            for(int i = 0; i < CustomPlayerInputManager.instance.players.Count; i++)
            {
                AttachPlayer(CustomPlayerInputManager.instance.players[i]);
            }
        }
    }

    private void ApplyPlayerData(PlayerData realPlayer, SO_PlayerData playerInfo)
    {
        //set the player to wear the proper hat and have the proper coloration
    }

    public void AttachPlayer(PlayerInput pi)
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].associatedPlayer == null)
            {
                playerData[i].associatedPlayer = pi;
                pi.GetComponent<PlayerController>().Setup(playerData[i]);
                return;
            }
        }
    }

    public void DetachPlayer(PlayerInput pi)
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if(playerData[i].associatedPlayer == pi)
            {
                playerData[i].associatedPlayer = null;
                playerData[i].playerObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public GameObject playerObject;
    public PawnController ghost;
    public PlayerInput associatedPlayer;
}

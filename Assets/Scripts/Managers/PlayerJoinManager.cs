using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class ChangePlayerReadyStateEvent : UnityEvent<int, bool>
{

}
[System.Serializable]
public class LeavePlayerEvent : UnityEvent<PlayerInput>
{

}
[System.Serializable]
public class StartGameEvent : UnityEvent
{

}

public class PlayerJoinManager : MonoBehaviour
{
    public static ChangePlayerReadyStateEvent changePlayerReadyStatus;
    public static LeavePlayerEvent leavePlayerEvent;
    public static StartGameEvent startGameEvent;
    public static bool allPlayersReady { get; set; }

    [SerializeField]
    private MainMenuUIManager mainMenuReference;
    [SerializeField]
    private List<PlayerInfo> playersJoined;
    private int totalJoinedPlayers;    

    private void OnEnable()
    {
        for (int i = 0; i < playersJoined.Count; i++)
        {
            playersJoined[i].PlayerID = 0;
            playersJoined[i].isReady = false;
            playersJoined[i].skinColor = null;
        }
        allPlayersReady = false;
        if(changePlayerReadyStatus == null)
        {
            changePlayerReadyStatus = new ChangePlayerReadyStateEvent();
            changePlayerReadyStatus.AddListener(CheckPlayerReadyState);
        }
        if(leavePlayerEvent == null)
        {
            leavePlayerEvent = new LeavePlayerEvent();
            leavePlayerEvent.AddListener(LeavePlayerEvent);
        }
        if(startGameEvent == null)
        {
            startGameEvent = new StartGameEvent();
            startGameEvent.AddListener(StartGame);
        }
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        changePlayerReadyStatus.RemoveAllListeners();
        leavePlayerEvent.RemoveAllListeners();
        startGameEvent.RemoveAllListeners();
        if (!allPlayersReady)
        {
            for (int i = 0; i < playersJoined.Count; i++)
            {
                playersJoined[i].PlayerID = 0;
                playersJoined[i].isReady = false;
                playersJoined[i].skinColor = null;
            }
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        AudioManager.instance.PlayJoinEvent();
        mainMenuReference.UpdateUIDisplay(playerInput, true);
        totalJoinedPlayers++;
        CheckStartGame();
    }

    private void LeavePlayerEvent(PlayerInput playerInput)
    {
        mainMenuReference.UpdateUIDisplay(playerInput, false);
        totalJoinedPlayers--;
        CheckStartGame();
    }

    private void CheckPlayerReadyState(int playerIndex, bool isReady)
    {
        if (isReady)
        {
            playersJoined[playerIndex - 1].PlayerID = playerIndex;
            playersJoined[playerIndex - 1].isReady = isReady;
        }
        else
        {
            playersJoined[playerIndex - 1].PlayerID = 0;
            playersJoined[playerIndex - 1].isReady = isReady;
        }
        AudioManager.instance.PlayReadyEvent();
        mainMenuReference.UpdateReadyDisplay(playerIndex, isReady);
        CheckStartGame();
    }

    private void CheckStartGame()
    {
        int amountofReadyPlayers = 0;
        for(int i = 0; i < playersJoined.Count; i++)
        {
            if (playersJoined[i].isReady)
            {
                amountofReadyPlayers++;   
            }
        }
        if (amountofReadyPlayers >= 2 && amountofReadyPlayers == totalJoinedPlayers)
        {
            allPlayersReady = true;
        }
        else
        {
            allPlayersReady = false;
        }
        mainMenuReference.CheckStartDisplay(allPlayersReady);
    }

    private void StartGame()
    {
        //loop over spawn point list to get players
        //Switch default map from menu to game
        //Disable player menu navigator
        //Enable player control
        //Enable rigidbody gravity
        SceneManager.LoadScene("Game");
    }
}

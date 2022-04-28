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
    private List<PlayerInfo> playersJoinedSO;
    [SerializeField][HideInInspector]
    private List<PlayerMenuNavigator> playersJoinedPrefabs;
    private int totalJoinedPlayers;    

    private void OnEnable()
    {
        for (int i = 0; i < playersJoinedSO.Count; i++)
        {
            playersJoinedSO[i].PlayerID = 0;
            playersJoinedSO[i].isReady = false;
            playersJoinedSO[i].skinColor = null;
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
            for (int i = 0; i < playersJoinedSO.Count; i++)
            {
                playersJoinedSO[i].PlayerID = 0;
                playersJoinedSO[i].isReady = false;
                playersJoinedSO[i].skinColor = null;
            }
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        AudioManager.instance.PlayJoinEvent();
        mainMenuReference.UpdateUIDisplay(playerInput, true);
        totalJoinedPlayers++;
        playersJoinedPrefabs.Add(playerInput.GetComponent<PlayerMenuNavigator>());
        CheckStartGame();
    }

    private void LeavePlayerEvent(PlayerInput playerInput)
    {
        playersJoinedPrefabs.Remove(playerInput.GetComponent<PlayerMenuNavigator>());
        AudioManager.instance.PlayLeaveEvent();
        mainMenuReference.UpdateUIDisplay(playerInput, false);
        totalJoinedPlayers--;
        CheckStartGame();
    }

    private void CheckPlayerReadyState(int playerIndex, bool isReady)
    {
        if (isReady)
        {
            playersJoinedSO[playerIndex - 1].PlayerID = playerIndex;
            playersJoinedSO[playerIndex - 1].isReady = isReady;
        }
        else
        {
            playersJoinedSO[playerIndex - 1].PlayerID = 0;
            playersJoinedSO[playerIndex - 1].isReady = isReady;
        }
        AudioManager.instance.PlayReadyEvent();
        mainMenuReference.UpdateReadyDisplay(playerIndex, isReady);
        CheckStartGame();
    }

    private void CheckStartGame()
    {
        int amountofReadyPlayers = 0;
        for(int i = 0; i < playersJoinedSO.Count; i++)
        {
            if (playersJoinedSO[i].isReady)
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
        foreach(PlayerMenuNavigator player in playersJoinedPrefabs)
        {
            player.SwitchControls();
        }
        SceneManager.LoadScene("Game");
    }
}

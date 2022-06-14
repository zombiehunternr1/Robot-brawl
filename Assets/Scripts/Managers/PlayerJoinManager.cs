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
public class LoadMinigameEvent : UnityEvent
{

}
[System.Serializable]
public class SwitchControlsEvent : UnityEvent
{

}
[System.Serializable]
public class PositionPlayersEvent : UnityEvent
{

}

public class PlayerJoinManager : MonoBehaviour
{
    public static ChangePlayerReadyStateEvent changePlayerReadyStatus;
    public static LeavePlayerEvent leavePlayerEvent;
    public static LoadMinigameEvent loadMinigameEvent;
    public static SwitchControlsEvent switchControlsEvent;
    public static PositionPlayersEvent positionPlayersEvent;
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
            playersJoinedSO[i].spawnPosition = Vector3.zero;
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
        if(loadMinigameEvent == null)
        {
            loadMinigameEvent = new LoadMinigameEvent();
            loadMinigameEvent.AddListener(LoadMiniGameScene);
        }
        if(switchControlsEvent == null)
        {
            switchControlsEvent = new SwitchControlsEvent();
            switchControlsEvent.AddListener(SwitchControls);
        }
        if(positionPlayersEvent == null)
        {
            positionPlayersEvent = new PositionPlayersEvent();
            positionPlayersEvent.AddListener(PositionPlayers);
        }
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        changePlayerReadyStatus.RemoveAllListeners();
        leavePlayerEvent.RemoveAllListeners();
        loadMinigameEvent.RemoveAllListeners();
        switchControlsEvent.RemoveAllListeners();
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

    private void LoadMiniGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    private void PositionPlayers()
    {
        for (int i = 0; i < playersJoinedPrefabs.Count; i++)
        {
            for (int j = 0; j < playersJoinedSO.Count; j++)
            {
                if (playersJoinedSO[j].PlayerID != 0)
                {
                    playersJoinedPrefabs[i].transform.position = playersJoinedSO[j].spawnPosition;
                }
            }
        }
    }

    private void SwitchControls()
    {
        foreach (PlayerMenuNavigator player in playersJoinedPrefabs)
        {
            player.SwitchToMinigameScript();
        }
    }
}

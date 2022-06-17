using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerJoinManager : MonoBehaviour
{
    public static bool allPlayersReady { get; set; }

    [SerializeField]
    private MainMenuUIManager mainMenuReference;
    [SerializeField]
    private List<PlayerInfo> playersJoinedSO;
    [SerializeField]
    private PlayerRanking playerRankSO;
    [SerializeField][HideInInspector]
    private List<PlayerMenuNavigator> playersJoinedPrefabs;
    private int totalJoinedPlayers;
    private int prefabIndex = 0;

    private void OnEnable()
    {
        playerRankSO.playerRanking.Clear();
        playerRankSO.currentPlayerIDs.Clear();
        for (int i = 0; i < playersJoinedSO.Count; i++)
        {
            playersJoinedSO[i].PlayerID = 0;
            playersJoinedSO[i].isReady = false;
            playersJoinedSO[i].skinColor = null;
            playersJoinedSO[i].spawnPosition = Vector3.zero;
        }
        allPlayersReady = false;
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
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

    public void LeavePlayerEvent(PlayerInput playerInput)
    {
        playersJoinedPrefabs.Remove(playerInput.GetComponent<PlayerMenuNavigator>());
        AudioManager.instance.PlayLeaveEvent();
        mainMenuReference.UpdateUIDisplay(playerInput, false);
        totalJoinedPlayers--;
        CheckStartGame();
    }

    public void CheckPlayerReadyState(int playerIndex, bool isReady)
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

    public void LoadMiniGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void PositionPlayers()
    {
        for(int i = 0; i < playersJoinedSO.Count; i++)
        {
            if(playersJoinedSO[i].PlayerID != 0)
            {
                playersJoinedPrefabs[prefabIndex].transform.position = playersJoinedSO[i].spawnPosition;
                playerRankSO.currentPlayerIDs.Add(playersJoinedSO[i].PlayerID);
                prefabIndex++;
            }
        }
    }

    public void SwitchControls()
    {
        foreach (PlayerMenuNavigator player in playersJoinedPrefabs)
        {
            player.SwitchToMinigameScript();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class PlayerJoinManager : MonoBehaviour
{
    public static bool allPlayersReady { get; set; }

    [SerializeField]
    private MainMenuUIManager mainMenuReference;
    [SerializeField]
    private List<PlayerInfo> playersJoinedSO;

    [SerializeField][HideInInspector]
    private List<PlayerMenuNavigator> playersJoinedPrefabs;
    private int totalJoinedPlayers;
    private int prefabIndex = 0;

    private void OnEnable()
    {
        for (int i = 0; i < playersJoinedSO.Count; i++)
        {
            playersJoinedSO[i].playerID = 0;
            playersJoinedSO[i].rankPosition = 0;
            playersJoinedSO[i].isReady = false;
            playersJoinedSO[i].skinColor = null;
            playersJoinedSO[i].currentPosition = Vector3.zero;
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
                playersJoinedSO[i].playerID = 0;
                playersJoinedSO[i].rankPosition = 0;
                playersJoinedSO[i].isReady = false;
                playersJoinedSO[i].skinColor = null;
                playersJoinedSO[i].currentPosition = Vector3.zero;
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
            playersJoinedSO[playerIndex - 1].playerID = playerIndex;
            playersJoinedSO[playerIndex - 1].isReady = isReady;
        }
        else
        {
            playersJoinedSO[playerIndex - 1].playerID = 0;
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

    public void PositionPlayers()
    {
        prefabIndex = 0;
        for(int i = 0; i < playersJoinedSO.Count; i++)
        {
            if(playersJoinedSO[i].playerID != 0)
            {
                playersJoinedPrefabs[prefabIndex].transform.position = playersJoinedSO[i].currentPosition;
                prefabIndex++;
            }
        }
    }

    public void DisplayRankOrder()
    {
        prefabIndex = 0;
        for (int i = 0; i < playersJoinedSO.Count; i++)
        {
            if(playersJoinedSO[i].playerID != 0)
            {
                playersJoinedPrefabs[prefabIndex].transform.rotation = new Quaternion(0, 180, 0, 0);
                if (playersJoinedSO[i].rankPosition == 0)
                {
                    playersJoinedPrefabs[prefabIndex].GetComponent<Animator>().Play("Victory");
                    playersJoinedPrefabs[prefabIndex].GetComponent<CharacterSkinController>().VictoryExpression();
                }
                else
                {
                    playersJoinedPrefabs[prefabIndex].GetComponent<Animator>().Play("Lost");
                    playersJoinedPrefabs[prefabIndex].GetComponent<CharacterSkinController>().DefeatExpression();
                }
                prefabIndex++;
            }
        }
    }

    public void AllowPlayerInput()
    {
        foreach (PlayerMenuNavigator player in playersJoinedPrefabs)
        {
            player.allowInput = true;
        }
    }

    public void DisAllowInput()
    {
        foreach(PlayerMenuNavigator player in playersJoinedPrefabs)
        {
            player.canInteract = false;
            player.allowInput = false;
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

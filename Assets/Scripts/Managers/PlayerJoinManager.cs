using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class ChangeColorDisplayEvent : UnityEvent<int, int, Material>
{

}
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
    public static ChangeColorDisplayEvent changeColorDisplay;
    public static ChangePlayerReadyStateEvent changePlayerReadyStatus;
    public static LeavePlayerEvent leavePlayerEvent;
    public static StartGameEvent startGameEvent;
    public static bool allPlayersReady { get; set; }

    [SerializeField]
    private List<PlayerInfo> playersJoined;
    private int totalJoinedPlayers;
    [Header("UI Elements")]
    [SerializeField]
    private RectTransform startGameDisplay;
    [SerializeField]
    private List<Transform> spawnPos;
    [SerializeField]
    private List<RectTransform> joiningDisplay;
    [SerializeField]
    private List<RectTransform> navigationDisplay;
    [SerializeField]
    private List<Image> skinColorDisplay;
    [SerializeField]
    private List<RectTransform> readyDisplay;
    [SerializeField]
    private List<Color32> skinColorOptions;

    private void OnEnable()
    {
        for (int i = 0; i < playersJoined.Count; i++)
        {
            playersJoined[i].PlayerID = 0;
            playersJoined[i].isReady = false;
            playersJoined[i].skinColor = null;
        }
        allPlayersReady = false;
        if(changeColorDisplay == null)
        {
            changeColorDisplay = new ChangeColorDisplayEvent();
            changeColorDisplay.AddListener(ChangeSkinColorDisplay);
        }
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
        changeColorDisplay.RemoveAllListeners();
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
        joiningDisplay[playerInput.playerIndex].gameObject.SetActive(false);
        navigationDisplay[playerInput.playerIndex].gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<PlayerMenuNavigator>().playerID = playerInput.playerIndex + 1;
        playerInput.gameObject.GetComponent<PlayerMenuNavigator>().GetPlayerInput(playerInput);
        playerInput.transform.position = spawnPos[playerInput.playerIndex].position + new Vector3(0, -3.5f, 0);
        playerInput.transform.SetParent(spawnPos[playerInput.playerIndex]);
        playerInput.transform.rotation = new Quaternion(0, 180, 0, 0);
        totalJoinedPlayers++;
        CheckStartGameDisplay();
    }

    private void LeavePlayerEvent(PlayerInput playerInput)
    {
        navigationDisplay[playerInput.playerIndex].gameObject.SetActive(false);
        joiningDisplay[playerInput.playerIndex].gameObject.SetActive(true);
        totalJoinedPlayers--;
        CheckStartGameDisplay();
    }

    public void ChangeSkinColorDisplay(int playerIndex, int skinIndex, Material skinColor)
    {
        AudioManager.instance.PlaySwitchColorEvent();
        skinColorDisplay[playerIndex - 1].color = skinColorOptions[skinIndex];
        playersJoined[playerIndex - 1].skinColor = skinColor;
    }

    private void CheckPlayerReadyState(int playerIndex, bool isReady)
    {
        if (isReady)
        {
            AudioManager.instance.PlayReadyEvent();
            navigationDisplay[playerIndex - 1].gameObject.SetActive(false);
            readyDisplay[playerIndex - 1].gameObject.SetActive(true);
            playersJoined[playerIndex - 1].PlayerID = playerIndex;
            playersJoined[playerIndex - 1].isReady = isReady;
            CheckStartGameDisplay();
        }
        else
        {
            AudioManager.instance.PlayerUnreadyEvent();
            readyDisplay[playerIndex - 1].gameObject.SetActive(false);
            navigationDisplay[playerIndex - 1].gameObject.SetActive(true);
            playersJoined[playerIndex - 1].PlayerID = 0;
            playersJoined[playerIndex - 1].isReady = isReady;
            CheckStartGameDisplay();
        }
    }

    private void CheckStartGameDisplay()
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
            startGameDisplay.gameObject.SetActive(true);
        }
        else
        {
            allPlayersReady = false;
            startGameDisplay.gameObject.SetActive(false);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform startGameDisplay;
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
    [SerializeField]
    private List<Transform> spawnPos;
    [SerializeField]
    private List<PlayerInfo> playersJoined;
    public void UpdateUIDisplay(PlayerInput playerInput, bool isJoining)
    {
        if (isJoining)
        {
            joiningDisplay[playerInput.playerIndex].gameObject.SetActive(false);
            navigationDisplay[playerInput.playerIndex].gameObject.SetActive(true);
            PlayerMenuNavigator player = playerInput.gameObject.GetComponent<PlayerMenuNavigator>();
            player.playerID = playerInput.playerIndex + 1;
            player.GetPlayerInput(playerInput);
            player.GetSpawnPos(spawnPos[playerInput.playerIndex].position);
            player.transform.position = spawnPos[playerInput.playerIndex].position + new Vector3(0, -3.5f, 0);
            player.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            navigationDisplay[playerInput.playerIndex].gameObject.SetActive(false);
            joiningDisplay[playerInput.playerIndex].gameObject.SetActive(true);
        }
    }

    public void UpdateReadyDisplay(int playerIndex, bool isReady)
    {
        if (isReady)
        {
            navigationDisplay[playerIndex - 1].gameObject.SetActive(false);
            readyDisplay[playerIndex - 1].gameObject.SetActive(true);
        }
        else
        {
            readyDisplay[playerIndex - 1].gameObject.SetActive(false);
            navigationDisplay[playerIndex - 1].gameObject.SetActive(true);
        }
    }

    public void ChangeSkinColorDisplay(int playerIndex, int skinIndex, Material skinColor)
    {
        skinColorDisplay[playerIndex - 1].color = skinColorOptions[skinIndex];
        playersJoined[playerIndex - 1].skinColor = skinColor;
    }

    public void CheckStartDisplay(bool allPlayersReady)
    {
        if (allPlayersReady)
        {
            startGameDisplay.gameObject.SetActive(true);
        }
        else
        {
            startGameDisplay.gameObject.SetActive(false);
        }
    }
}

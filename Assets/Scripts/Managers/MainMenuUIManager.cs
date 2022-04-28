using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class ChangeColorDisplayEvent : UnityEvent<int, int, Material>
{

}

public class MainMenuUIManager : MonoBehaviour
{
    public static ChangeColorDisplayEvent changeColorDisplay;

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

    private void OnEnable()
    {
        if (changeColorDisplay == null)
        {
            changeColorDisplay = new ChangeColorDisplayEvent();
            changeColorDisplay.AddListener(ChangeSkinColorDisplay);
        }
    }

    private void OnDisable()
    {
        changeColorDisplay.RemoveAllListeners();
    }

    public void UpdateUIDisplay(PlayerInput playerInput, bool isJoining)
    {
        if (isJoining)
        {
            joiningDisplay[playerInput.playerIndex].gameObject.SetActive(false);
            navigationDisplay[playerInput.playerIndex].gameObject.SetActive(true);
            playerInput.transform.position = spawnPos[playerInput.playerIndex].position + new Vector3(0, -3.5f, 0);
            playerInput.transform.SetParent(spawnPos[playerInput.playerIndex]);
            playerInput.transform.rotation = new Quaternion(0, 180, 0, 0);
            playerInput.gameObject.GetComponent<PlayerMenuNavigator>().playerID = playerInput.playerIndex + 1;
            playerInput.gameObject.GetComponent<PlayerMenuNavigator>().GetPlayerInput(playerInput);
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
        AudioManager.instance.PlaySwitchColorEvent();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class StartMinigameEvent : UnityEvent
{

}

public class MiniGameManager : MonoBehaviour
{
    public static StartMinigameEvent startMinigameCountdownEvent;

    [SerializeField]
    private RectTransform MinigameRulesPanel;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private Transform tilesReference;

    [SerializeField]
    private float minInterval;
    [SerializeField]
    private float maxInterval;

    private bool gameFinished { get; set; }
    private float timeBeforeCollapsing;
    private int selectedTile;
    [SerializeField]
    private List<Tile> tilesList;

    private void OnEnable()
    {
        gameFinished = false;
        if(startMinigameCountdownEvent == null)
        {
            startMinigameCountdownEvent = new StartMinigameEvent();
            startMinigameCountdownEvent.AddListener(StartCountdown);
        }
        GetTiles();
        //PlayerJoinManager.positionPlayersEvent.Invoke();
    }
    public void StartCountdown()
    {
        MinigameRulesPanel.gameObject.SetActive(false);
        StartCoroutine(Countdown());
    }

    private void OnDisable()
    {
        startMinigameCountdownEvent.RemoveAllListeners();
    }

    private void GetTiles()
    {
        tilesList = new List<Tile>();
        foreach(Tile tile in tilesReference.GetComponentsInChildren<Tile>())
        {
            tilesList.Add(tile);
        }
        StartCoroutine(TileSystem());
    }

    IEnumerator TileSystem()
    {
        while (!gameFinished)
        {
            timeBeforeCollapsing = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(timeBeforeCollapsing);
            selectedTile = Random.Range(0, tilesList.Count - 1);
            if (tilesList[selectedTile].isTargetable)
            {
                tilesList[selectedTile].isTargetable = false;
                tilesList[selectedTile].StartShaking();
            }
        }
    }

    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Go";
        PlayerJoinManager.switchControlsEvent.Invoke();
        StartCoroutine(TileSystem());
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        StopCoroutine(Countdown());
    }
}

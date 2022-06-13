using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class hidePanelEvent : UnityEvent
{

}

public class MiniGameManager : MonoBehaviour
{
    public static hidePanelEvent hidePanelEvent;

    [SerializeField]
    private RectTransform MinigameRulesPanel;
    [SerializeField]
    private TextMeshProUGUI countdownText;

    private void OnEnable()
    {
        if(hidePanelEvent == null)
        {
            hidePanelEvent = new hidePanelEvent();
            hidePanelEvent.AddListener(StartCountdown);
        }
    }

    private void OnDisable()
    {
        hidePanelEvent.RemoveAllListeners();
    }

    public void StartCountdown()
    {
        MinigameRulesPanel.gameObject.SetActive(false);
        StartCoroutine(Countdown());
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
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        StopAllCoroutines();
    }
}

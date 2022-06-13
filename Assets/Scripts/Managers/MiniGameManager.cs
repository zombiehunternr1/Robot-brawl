using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class hidePanelEvent : UnityEvent
{

}

public class MiniGameManager : MonoBehaviour
{
    public static hidePanelEvent hidePanelEvent;

    private void OnEnable()
    {
        if(hidePanelEvent == null)
        {
            hidePanelEvent = new hidePanelEvent();
            hidePanelEvent.AddListener(HidePanel);
        }
    }

    private void OnDisable()
    {
        hidePanelEvent.RemoveAllListeners();
    }

    [SerializeField]
    private RectTransform MinigameRulesPanel;

    public void HidePanel()
    {
        MinigameRulesPanel.gameObject.SetActive(false);
    }
}

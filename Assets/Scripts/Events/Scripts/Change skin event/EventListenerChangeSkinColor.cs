using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerChangeSkinColor : MonoBehaviour
{
    [SerializeField]
    private GameEventChangeSkinColor _gameEventChangeSkinColor;
    [SerializeField]
    private UnityEvent<int, int, Material> _response;
    private void OnEnable()
    {
        _gameEventChangeSkinColor.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEventChangeSkinColor.UnregisterListener(this);
    }

    public void OnEventRaised(int playerIndex, int skinIndex, Material materialValue)
    {
        _response.Invoke(playerIndex, skinIndex, materialValue);
    }
}

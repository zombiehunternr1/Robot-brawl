using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerCheckReady : MonoBehaviour
{
    [SerializeField]
    private GameEventCheckReady _gameEventCheckReady;
    [SerializeField]
    private UnityEvent<int, bool> _response;

    private void OnEnable()
    {
        _gameEventCheckReady.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEventCheckReady.UnregisterListener(this);
    }

    public void OnEventRaised(int intValue, bool boolValue)
    {
        _response.Invoke(intValue, boolValue);
    }
}

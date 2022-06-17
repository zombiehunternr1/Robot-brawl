using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerInt : MonoBehaviour
{
    [SerializeField]
    private GameEventInt _gameEventInt;
    [SerializeField]
    private UnityEvent<int> _response;

    private void OnEnable()
    {
        _gameEventInt.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEventInt.UnregisterListener(this);
    }

    public void OnEventRaised(int intValue)
    {
        _response.Invoke(intValue);
    }
}

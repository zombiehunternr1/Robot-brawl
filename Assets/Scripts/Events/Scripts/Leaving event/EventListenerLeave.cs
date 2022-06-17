using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventListenerLeave : MonoBehaviour
{
    [SerializeField]
    private GameEventLeave _gameEventLeave;
    [SerializeField]
    private UnityEvent<PlayerInput> _response;

    private void OnEnable()
    {
        _gameEventLeave.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEventLeave.UnregisterListener(this);
    }

    public void OnEventRaised(PlayerInput playerInput)
    {
        _response.Invoke(playerInput);
    }
}

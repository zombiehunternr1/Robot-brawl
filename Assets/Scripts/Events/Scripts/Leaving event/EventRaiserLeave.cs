using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventRaiserLeave : MonoBehaviour
{
    [SerializeField]
    private GameEventLeave _gameEvent;

    public void RaiseLeaveEvent(PlayerInput playerInput)
    {
        _gameEvent.RaiseLeave(playerInput);
    }
}

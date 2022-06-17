using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventRaiserLeave : MonoBehaviour
{
    [SerializeField]
    private GameEventLeave _gameLeaveEvent;

    public void RaiseLeaveEvent(PlayerInput playerInput)
    {
        _gameLeaveEvent.RaiseLeave(playerInput);
    }
}

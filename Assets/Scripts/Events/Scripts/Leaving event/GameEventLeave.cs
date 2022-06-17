using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Game event leave player", menuName = "SO/Game Events/Game event leave player")]
public class GameEventLeave : ScriptableObject
{
    private List<EventListenerLeave> _listeners = new List<EventListenerLeave>();

    public void RaiseLeave(PlayerInput playerInput)
    {
        for(int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(playerInput);
        }
    }

    public void RegisterListener(EventListenerLeave newListener)
    {
        _listeners.Add(newListener);
    }

    public void UnregisterListener(EventListenerLeave listenerToRemove)
    {
        _listeners.Remove(listenerToRemove);
    }
}

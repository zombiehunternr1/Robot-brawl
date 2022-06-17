using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game event int", menuName = "SO/Game Events/Game event int")]
public class GameEventInt : ScriptableObject
{
    private List<EventListenerInt> _listeners = new List<EventListenerInt>();

    public void RaiseInt(int intValue)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(intValue);
        }
    }

    public void RegisterListener(EventListenerInt newListener)
    {
        _listeners.Add(newListener);
    }

    public void UnregisterListener(EventListenerInt listenerToRemove)
    {
        _listeners.Remove(listenerToRemove);
    }
}

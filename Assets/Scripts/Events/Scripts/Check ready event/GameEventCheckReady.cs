using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game event check ready status", menuName = "SO/Game Events/Game event check ready status")]
public class GameEventCheckReady : ScriptableObject
{
    private List<EventListenerCheckReady> _listeners = new List<EventListenerCheckReady>();

    public void RaiseCheckReady(int intValue, bool boolValue)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(intValue, boolValue);
        }
    }
    public void RegisterListener(EventListenerCheckReady newListener)
    {
        _listeners.Add(newListener);
    }

    public void UnregisterListener(EventListenerCheckReady listenerToRemove)
    {
        _listeners.Remove(listenerToRemove);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game event empty", menuName ="SO/Game Events/Game event empty")]
public class GameEventEmpty : ScriptableObject
{
    private List<EventListener> _listeners = new List<EventListener>();

    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(EventListener newListener)
    {
        _listeners.Add(newListener);
    }

    public void UnregisterListener(EventListener listenerToRemove)
    {
        _listeners.Remove(listenerToRemove);
    }
}

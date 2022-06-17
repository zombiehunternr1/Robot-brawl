using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game event change skin color", menuName = "SO/Game Events/Game event change skin color")]
public class GameEventChangeSkinColor : ScriptableObject
{
    private List<EventListenerChangeSkinColor> _listeners = new List<EventListenerChangeSkinColor>();

    public void RaiseChangeSkinColor(int playerIndex, int skinIndex, Material materialValue)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(playerIndex, skinIndex, materialValue);
        }
    }
    public void RegisterListener(EventListenerChangeSkinColor newListener)
    {
        _listeners.Add(newListener);
    }

    public void UnregisterListener(EventListenerChangeSkinColor listenerToRemove)
    {
        _listeners.Remove(listenerToRemove);
    }
}

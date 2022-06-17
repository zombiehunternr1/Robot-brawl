using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseIntEvent : MonoBehaviour
{
    [SerializeField]
    private GameEventInt _gameIntEvent;

    public void RaiseLeaveEvent(int intvalue)
    {
        _gameIntEvent.RaiseInt(intvalue);
    }
}

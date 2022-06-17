using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRaiserCheckReady : MonoBehaviour
{
    [SerializeField]
    private GameEventCheckReady _gameEventCheckReady;

    public void RaiseCheckReadyEvent(int intValue, bool boolValue)
    {
        _gameEventCheckReady.RaiseCheckReady(intValue, boolValue);
    }
}

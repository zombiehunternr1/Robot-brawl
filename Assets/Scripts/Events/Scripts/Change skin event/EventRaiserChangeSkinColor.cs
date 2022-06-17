using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRaiserChangeSkinColor : MonoBehaviour
{
    [SerializeField]
    private GameEventChangeSkinColor _gameEventChangeSkinColor;

    public void RaiseChangeSkinColor(int playerIndex, int skinIndex, Material materialValue)
    {
        _gameEventChangeSkinColor.RaiseChangeSkinColor(playerIndex, skinIndex, materialValue);
    }
}

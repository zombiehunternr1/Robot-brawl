using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerRank : MonoBehaviour
{
    [SerializeField]
    private GameEventEmpty positionRankPlayerEvent;
    [SerializeField]
    private GameEventEmpty displayRankPlayerEvent;
    [SerializeField]
    private List<Transform> rankPositions;
    [SerializeField]
    private List<PlayerInfo> playerInfo;
    private int playerRank;


    private void OnEnable()
    {
        PositionRankedPlayers();
    }

    private void PositionRankedPlayers()
    {
        for (int i = 0; i < playerInfo.Count; i++)
        {
            playerRank = playerInfo[i].rankPosition;
            playerInfo[i].currentPosition = rankPositions[playerRank].position;
        }
        positionRankPlayerEvent.Raise();
        displayRankPlayerEvent.Raise();
    }
}

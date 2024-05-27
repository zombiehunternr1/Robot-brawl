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
    [SerializeField]
    private float timeTillGameReset;
    private float currenttime;
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
        StartCoroutine(CooldownTillReset());
    }

    private IEnumerator CooldownTillReset()
    {
        while(currenttime <= timeTillGameReset)
        {
            currenttime += Time.deltaTime / timeTillGameReset;
            yield return null;
        }
    }
}

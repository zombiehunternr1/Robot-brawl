using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> SpawnPoints;
    [SerializeField]
    private List<PlayerInfo> playerInfo;

    private int SpawnPointCount;
    
    private void OnEnable()
    {
        SpawnPointCount = 0;
        PlayerInitialisation();
    }

    private void PlayerInitialisation()
    {        
        for(int i = 0; i < playerInfo.Count; i++)
        {
            if(playerInfo[i].PlayerID != 0)
            {
                playerInfo[i].spawnPosition = SpawnPoints[SpawnPointCount].position;
                SpawnPointCount++;
            }
        }
    }
}

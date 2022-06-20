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
    
    private void OnEnable()
    {
        SetPlayerSpawnPosition();
    }

    private void SetPlayerSpawnPosition()
    {        
        for(int i = 0; i < playerInfo.Count; i++)
        {
            playerInfo[i].currentPosition = SpawnPoints[i].position;
        }
    }
}

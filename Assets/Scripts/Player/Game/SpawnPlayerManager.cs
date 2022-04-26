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
    private void Start()
    {
        //PlayerInitialisation();
    }

    private void PlayerInitialisation()
    {
        for(int i = 0; i < playerInfo.Count; i++)
        {
            if(playerInfo[i].PlayerID != 0)
            {

            }
        }
    }
}

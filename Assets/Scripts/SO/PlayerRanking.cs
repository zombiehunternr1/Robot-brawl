using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player rank", menuName = "SO/Player rank")]
public class PlayerRanking : ScriptableObject
{
    public List<int> currentPlayerIDs;
    public List<int> playerRanking;
}

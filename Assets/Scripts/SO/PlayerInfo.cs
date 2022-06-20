using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Info", menuName ="SO/Player Info")]
public class PlayerInfo : ScriptableObject
{
    public int playerID;
    public int rankPosition;
    public bool isReady;
    public Material skinColor;
    public Vector3 currentPosition;
}

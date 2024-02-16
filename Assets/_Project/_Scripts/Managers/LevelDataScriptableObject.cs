using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-Data", menuName = "Levels/Level-Data")]
public class LevelDataScriptableObject : ScriptableObject
{
    public Vector3 respawnLocation;
}

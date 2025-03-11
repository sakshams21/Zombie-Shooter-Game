using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataList", menuName = "Scriptable Objects/LevelDataList")]
public class LevelDataList : ScriptableObject
{
    [SerializeField]
    public List<LevelData> Data;
}

[Serializable]
public class LevelData
{
    public int ObjectKill;
    public int NumberOfBigZombies;
    public int NumberOfSmallZombies;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeGeometryDifficulty : MonoBehaviour
{
    [Header("Data")]
    public List<dataDiffilcuty> _dataDifficulty;

    [Serializable]
    public struct dataDiffilcuty
    {
        [Tooltip("Min Stars to connect")]
        public int minStars;
        [Tooltip("Max Stars to connect")]
        public int maxStars;
        [Tooltip("Size variation")]
        [Range(1, 9)]
        public int maxSize;
    }

    /// <summary>
    /// Returns difficulty data to create the level
    /// </summary>
    /// <param name="levelDifficulty">Difficulty level</param>
    /// <returns>Returns the data in format <see cref="dataDiffilcuty"/> </returns>
    public dataDiffilcuty GenerateDataDifficulty(int levelDifficulty)
    {
        if (_dataDifficulty.Count < levelDifficulty)
        {
            return _dataDifficulty[0];
        }
        else
        {
            return _dataDifficulty[levelDifficulty - 1];
        }
    }
}

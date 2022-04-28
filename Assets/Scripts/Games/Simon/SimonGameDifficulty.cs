using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGameDifficulty : MonoBehaviour
{
    [Header("Data")]
    public List<dataDiffilcuty> _dataDifficulty;

    [Serializable]
    public struct dataDiffilcuty
    {
        [Range(3,6)]
        [Tooltip("Number of buttons")]
        public int numberButtons;
        [Tooltip("Increase round list")]
        public int roundList;
        [Tooltip("Speed")]
        public float simonVelocity;
       
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

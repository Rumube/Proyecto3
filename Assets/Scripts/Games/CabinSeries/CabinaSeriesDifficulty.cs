using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinaSeriesDifficulty : MonoBehaviour
{
    [Header("Data")]
    public List<dataDiffilcuty> _dataDifficulty;

    [Serializable]
    public struct dataDiffilcuty
    {
        [Header("Scene")]
        [Tooltip("Number of max asteroids in scene")]
        [Range(3, 12)]
        public int maxAteroidesinScene;
        [Tooltip("Number of min asteroids in scene, never less than maxAteroidesinScene value")]
        [Range(3, 12)]
        public int minAteroidesinScene;
        [Tooltip("Geometry used")]
        public List<Geometry.Geometry_Type> geometryUsed;
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

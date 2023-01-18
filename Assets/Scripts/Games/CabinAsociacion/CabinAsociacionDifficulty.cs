using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinAsociacionDifficulty : MonoBehaviour
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
        [Tooltip("The posible geometry in the scene")]
        public List<Geometry.Geometry_Type> _geometryType;

        [Header("Target")]
        [Tooltip("The posible geometry target")]
        public List<Geometry.Geometry_Type> _targetGeometryType;
        [Tooltip("Number of max target asteroids")]
        [Range(3, 4)]
        public int maxTargetAsteroids;
        [Tooltip("Number of min target asteroids, never less than maxTargetAsteroids value")]
        [Range(1, 2)]
        public int minTargetAsteroids;
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

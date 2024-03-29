using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBalsterDifficulty : MonoBehaviour
{
    [Header("Data")]
    public List<dataDiffilcuty> _dataDifficulty;

    [Serializable]
    public struct dataDiffilcuty
    {
        [Tooltip("Possible geometry that can be produced at this level")]
        public List<Geometry.Geometry_Type> possibleGeometry;
        [Tooltip("Possible geometry that can be target at this level")]
        public List<Geometry.Geometry_Type> targetsGeometry;
        [Tooltip("Total number of asteroids at this level")]
        public int numAsteroids;
        [Tooltip("Total number of targets at this level")]
        public int numTargets;
        [Tooltip("Total number of different geometry targets at this level")]
        public int numGeometryTargets;
        [Tooltip("Speed at which asteroids travel")]
        public float speedMovement;
        [Tooltip("Set asteroids rotation")]
        public bool asteroidsCanRotate;
        [Tooltip("Speed at which asteroids rotate")]
        public float speedRotation;
    }

    /// <summary>
    /// Returns difficulty data to create the level
    /// </summary>
    /// <param name="levelDifficulty">Difficulty level</param>
    /// <returns>Returns the data in format <see cref="dataDiffilcuty"/> </returns>
    public dataDiffilcuty GenerateDataDifficulty(int levelDifficulty)
    {
        if(_dataDifficulty.Count < levelDifficulty)
        {
            return _dataDifficulty[0];
        }
        else{
            return _dataDifficulty[levelDifficulty - 1];
        }
    }

}

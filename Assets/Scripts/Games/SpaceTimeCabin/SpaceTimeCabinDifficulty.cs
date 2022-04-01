using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTimeCabinDifficulty : MonoBehaviour
{
    [Header("Data")]
    public List<dataDiffilcuty> _dataDifficulty;

    [Serializable]
    public struct dataDiffilcuty
    {
        [Tooltip("Number of asteroid created")]
        public int numberAsteroid;
        [Tooltip("Time delay between asteroid starts")]
        public float delayAsteroidStart;
        [Tooltip("Distance between the center and the gun target")]
        public Vector2 shotOffset;
        [Tooltip("Velocity of the asteroids")]
        public float asteroidMovementVelocity;
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

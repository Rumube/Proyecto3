using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTimeCabin : MonoBehaviour
{
    [Header("Game Configuration")]
    public Vector2 _targetPoint;
    public int _level;
    SpaceTimeCabinDifficulty.dataDiffilcuty _dataDifficulty;

    [Header("Game Data")]
    List<GameObject> _asteroids = new List<GameObject>();

    [Header("References")]
    public GameObject _asteroidPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        _dataDifficulty = GetComponent<SpaceTimeCabinDifficulty>().GenerateDataDifficulty(_level);
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void RestartGame()
    {
        GenerateAsteroids();
    }

    /// <summary>
    /// Generate the asteroids
    /// </summary>
    void GenerateAsteroids()
    {
        for (int i = 0; i < _dataDifficulty.numberAsteroid; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().InitAsteroid(_dataDifficulty.asteroidMovementVelocity, Vector2.zero, gameObject);
        }
    }

    /// <summary>
    /// Check if the shot are correct
    /// </summary>
    /// <param name="asteroid">The asteroid shotting</param>
    public void CheckIfIsCorrect(GameObject asteroid)
    {

    }

}

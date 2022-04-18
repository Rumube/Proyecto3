using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceTimeCabin : MonoBehaviour
{
    [Header("Game Configuration")]
    public Vector2 _targetPoint;
    public int _level;
    SpaceTimeCabinDifficulty.dataDiffilcuty _dataDifficulty;
    [HideInInspector]
    public bool _gameFinished = false;

    [Header("Game Data")]
    List<GameObject> _asteroids = new List<GameObject>();

    [Header("References")]
    public GameObject _asteroidPrefab;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        _dataDifficulty = GetComponent<SpaceTimeCabinDifficulty>().GenerateDataDifficulty(_level);
        _target = GameObject.FindGameObjectWithTag("GunTarget");
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
        _targetPoint = GenerateTargetPoint();
        GenerateAsteroids();
    }

    /// <summary>
    /// Generate the Vector2 coordinate of the target and set the position to the <see cref="_target"/>
    /// </summary>
    /// <returns>Value of <see cref="_targetPoint"/></returns>
    private Vector2 GenerateTargetPoint()
    {
        float xOffset = _dataDifficulty.shotOffset.x;
        float yOffset = _dataDifficulty.shotOffset.y;
        Vector2 newTargetPoint = new Vector2(Random.Range(-xOffset, xOffset), Random.Range(-yOffset, yOffset)+1);
        _target.transform.position = newTargetPoint;
        gameObject.GetComponent<AsteroidBlasterInput>().MoveGun(newTargetPoint);
        return newTargetPoint;
    }

    /// <summary>
    /// Generate the asteroids
    /// </summary>
    void GenerateAsteroids()
    {
        for (int i = 0; i < _dataDifficulty.numberAsteroid; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().InitAsteroid(_dataDifficulty.asteroidMovementVelocity, _targetPoint, gameObject);
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

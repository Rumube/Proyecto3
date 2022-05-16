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
    public int _successes = 0;
    public int _errors = 0;
    private bool _isAllAsteroidGenerated;
    private bool _finishingGame = false;

    [Header("References")]
    public GameObject _asteroidPrefab;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        _dataDifficulty = GetComponent<SpaceTimeCabinDifficulty>().GenerateDataDifficulty(_level);
        _target = GameObject.FindGameObjectWithTag("GunTarget");
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("No podemos mover el cañón, dispara cuando los asteroides pasen por la mira", true);

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
        if(ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            _finishingGame = false;
            _targetPoint = GenerateTargetPoint();
            StartCoroutine(GenerateAsteroids());
        }
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
    IEnumerator GenerateAsteroids()
    {
        _isAllAsteroidGenerated = false;
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < _dataDifficulty.numberAsteroid; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().InitAsteroid(_dataDifficulty.asteroidMovementVelocity, _targetPoint, gameObject);
            yield return new WaitForSeconds(_dataDifficulty.delayAsteroidStart);
        }
        _isAllAsteroidGenerated = true;
    }

    /// <summary>
    /// Checks if is correct
    /// </summary>
    /// <param name="newCollider">Shoted collider</param>
    public void CheckIfIsCorrect(Collider2D newCollider)
    {
        if (newCollider.tag == "Asteroid")
        {
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(newCollider.transform.position);
            _successes++;
        }
        //else
        //{
        //    ServiceLocator.Instance.GetService<IError>().GenerateError();
        //    _errors++;
        //}
        CheckIfFinish();
    }

    /// <summary>
    /// Generate an error when asteroid desapear
    /// </summary>
    public void LoseAsteroid()
    {
        ServiceLocator.Instance.GetService<IError>().GenerateError();
        _errors++;
        CheckIfFinish();
    }

    private void CheckIfFinish()
    {
        GameObject[] asteroidsInGame = GameObject.FindGameObjectsWithTag("Asteroid");
        int noHitAsteroid = asteroidsInGame.Length;
        foreach (GameObject currentAsteroid in asteroidsInGame)
        {
            if (currentAsteroid.GetComponent<Asteroid>()._state == Asteroid.Asteroid_State.exploding)
                noHitAsteroid--;
        }
        if(_isAllAsteroidGenerated && noHitAsteroid == 0 && !_finishingGame)
        {
            _finishingGame = true;
            ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_successes, _errors);
            _successes = 0;
            _errors = 0;
            Invoke("RestartGame", 3f);
        }
    }
}

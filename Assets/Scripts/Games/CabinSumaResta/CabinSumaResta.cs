using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinSumaResta : MonoBehaviour
{
    [Header("References")]
    public GameObject _asteroid;
    public List<GameObject> _spawnsList = new List<GameObject>();
    public List<GameObject> _gunList = new List<GameObject>();
    private List<GameObject> _generatedAsteroids = new List<GameObject>();

    [Header("Game Values")]
    private int _asteroidsInScene = 0;
    private int _targetAsteroids = 0;
    private int _initialSelecteds = 0;

    private int _successes = 0;
    private int _errors = 0;

    [Header("Configuration")]
    private int _level;
    private CabinSumaRestaDifficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _currentDataDifficulty = GetComponent<CabinSumaRestaDifficulty>().GenerateDataDifficulty(_level);
        GetComponent<AsteroidBlasterInput>().SetGameMode(AsteroidBlasterInput.GAME_MODE.addSubtraction);
        RestartGame();

        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RestartGame()
    {
        DestroyAsteroids();
        GenerateTarget();
        GenerateAsteroids();
        GenerateOrder();
    }
    private void DestroyAsteroids()
    {
        List<GameObject> auxList = new List<GameObject>(_generatedAsteroids);
        for (int i = 0; i < _generatedAsteroids.Count; i++)
        {
            Destroy(auxList[i]);
        }
        _generatedAsteroids.Clear();
    }
    private void GenerateTarget()
    {
        _asteroidsInScene = Random.Range(_currentDataDifficulty.minAteroidesinScene, _currentDataDifficulty.maxAteroidesinScene - 1);
        _targetAsteroids = Random.Range(_currentDataDifficulty.minTargetAsteroids, _currentDataDifficulty.maxTargetAsteroids - 1);
        _initialSelecteds = Random.Range(_currentDataDifficulty.minInitialSelecteds, _currentDataDifficulty.maxInitialSelecteds - 1);

        if (_targetAsteroids > _asteroidsInScene)
        {
            _targetAsteroids = _asteroidsInScene;
        }
        if(_initialSelecteds > _asteroidsInScene)
        {
            _initialSelecteds = _asteroidsInScene;
        }
    }
    /// <summary>
    /// Starts the proccess to generate 
    /// the asteroids in game scene
    /// </summary>
    private void GenerateAsteroids()
    {
        SpawnAsteroids();
        SetSelectedAsteroids();
    }

    /// <summary>
    /// Spawns in the scene asteroids in random positions
    /// and save them in the <see cref="_generatedAsteroids"/> list.
    /// </summary>
    private void SpawnAsteroids()
    {
        List<GameObject> spawnAux = new List<GameObject>(_spawnsList);
        System.Random random = new System.Random();
        int n = spawnAux.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject value = spawnAux[k];
            spawnAux[k] = spawnAux[n];
            spawnAux[n] = value;
        }

        for (int i = 0; i < _asteroidsInScene; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroid, spawnAux[i].transform);
            newAsteroid.name = "Asteroid";
            _generatedAsteroids.Add(newAsteroid);
        }
    }
    /// <summary>
    /// Select a random asteroids using
    /// the <see cref="_initialSelecteds"/> value
    /// </summary>
    private void SetSelectedAsteroids()
    {
        List<GameObject> asteroidListAux = new List<GameObject>(_generatedAsteroids);
        System.Random random = new System.Random();
        int n = asteroidListAux.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject value = asteroidListAux[k];
            asteroidListAux[k] = asteroidListAux[n];
            asteroidListAux[n] = value;
        }

        for (int i = 0; i < _initialSelecteds; i++)
        {
            asteroidListAux[i].GetComponent<PickableAsteroid>().SelectAsteroid();
        }
    }
    /// <summary>
    /// Generates the order for the player
    /// </summary>
    private void GenerateOrder()
    {
        string order = "¡Destruye los asteroides para que solo queden " + _targetAsteroids + " !";
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(order, true);
    }
    /// <summary>
    /// Checks if the game is correct
    /// </summary>
    public void CheckIfIsCorrect()
    {
        int shootedAsteroids = 0;
        int selectedAsteroids = _generatedAsteroids.Count;

        foreach (GameObject currentAsteroid in _generatedAsteroids)
        {
            if (currentAsteroid.GetComponent<PickableAsteroid>().GetSelected())
            {
                shootedAsteroids++;
                selectedAsteroids--;
            }
        }

        if(shootedAsteroids != 0)
        {
            foreach (GameObject currentGun in _gunList)
            {
                currentGun.GetComponent<Animator>().SetTrigger("Shot");
            }
        }

        if(selectedAsteroids == _targetAsteroids)
        {
            _successes++;
            StartCoroutine(FinishAnimation());
        }
        else
        {
            _errors++; 
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
    }
    public IEnumerator FinishAnimation()
    {
        foreach (GameObject currentAteroid in _generatedAsteroids)
        {
            if (currentAteroid.GetComponent<PickableAsteroid>().GetSelected())
            {
                currentAteroid.GetComponent<SpriteRenderer>().enabled = false;
                currentAteroid.GetComponent<PickableAsteroid>().BrokenAsteroid();
            }
        }
        yield return new WaitForSeconds(1f);
        ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(transform.position);
        ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_successes, _errors);
        RestartGame();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CabinAsociacion : MonoBehaviour
{
    [Header("References")]
    public GameObject _asteroid;
    public List<GameObject> _spawnsList = new List<GameObject>();
    public List<GameObject> _gunList = new List<GameObject>();
    private List<GameObject> _generatedAsteroids = new List<GameObject>();

    [Header("Game Values")]
    private int _asteroidsInScene = 0;
    private int _numTargetAsteroids = 0;
    private int _initialSelecteds = 0;
    private Geometry.Geometry_Type _geometryTarget;

    private int _successes = 0;
    private int _errors = 0;

    [Header("Configuration")]
    private int _level;
    private CabinAsociacionDifficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _currentDataDifficulty = GetComponent<CabinAsociacionDifficulty>().GenerateDataDifficulty(_level);
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
        _numTargetAsteroids = Random.Range(_currentDataDifficulty.minTargetAsteroids, _currentDataDifficulty.maxTargetAsteroids);
        _geometryTarget = _currentDataDifficulty._targetGeometryType[Random.Range(0, _currentDataDifficulty._targetGeometryType.Count)];

        if (_numTargetAsteroids > _asteroidsInScene)
        {
            _numTargetAsteroids = _asteroidsInScene;
        }
    }
    /// <summary>
    /// Starts the proccess to generate 
    /// the asteroids in game scene
    /// </summary>
    private void GenerateAsteroids()
    {
        SpawnAsteroids();
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
            if(i <= _numTargetAsteroids)
            {
                newAsteroid.GetComponent<PickableAsteroid>().SetAsociacionValue(_geometryTarget);
            }
            else
            {
                Geometry.Geometry_Type newGeometry = _currentDataDifficulty._geometryType[Random.Range(0, _currentDataDifficulty._geometryType.Count)];
                newAsteroid.GetComponent<PickableAsteroid>().SetAsociacionValue(newGeometry);
            }
        }
    }



    /// <summary>
    /// Generates the order for the player
    /// </summary>
    private void GenerateOrder()
    {
        Geometry aux = new Geometry();
        string part1 = ServiceLocator.Instance.GetService<MultilanguajeReader>().GetText(14);
        string part2 = ServiceLocator.Instance.GetService<MultilanguajeReader>().GetText(_numTargetAsteroids);
        string part3 = ServiceLocator.Instance.GetService<MultilanguajeReader>().GetText(92);
        string part4 = ServiceLocator.Instance.GetService<MultilanguajeReader>().GetText(aux.getGeometryID(_geometryTarget));
        string part5 = ServiceLocator.Instance.GetService<MultilanguajeReader>().GetText(91);

        string order = part1 + part2 + part3 + part4 + part5;
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(order, true);
    }
    /// <summary>
    /// Checks if the game is correct
    /// </summary>
    public void CheckIfIsCorrect()
    {
        bool correct = true;
        int shootedAsteroids = 0;

        foreach (GameObject currentAsteroid in _generatedAsteroids)
        {
            PickableAsteroid asteroid = currentAsteroid.GetComponent<PickableAsteroid>();
            if (asteroid.GetSelected())
            {
                if(asteroid.GetGeometry() != _geometryTarget)
                {
                    correct = false;
                }
                shootedAsteroids++;
            }
        }

        if(shootedAsteroids != _numTargetAsteroids)
        {
            correct = false;
        }

        if(shootedAsteroids != 0)
        {
            foreach (GameObject currentGun in _gunList)
            {
                currentGun.GetComponent<Animator>().SetTrigger("Shot");
            }
        }

        if(correct)
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
                StartCoroutine(currentAteroid.GetComponent<PickableAsteroid>().BrokenAsteroid());
            }
        }
        yield return new WaitForSeconds(0.3f);
        ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(transform.position);
        ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_successes, _errors);
        RestartGame();
    }

}

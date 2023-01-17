using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinSeries : MonoBehaviour
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
    public enum SERIES_TYPE
    {
        highToLow = 0,
        lowToHigh = 1,
        moreSpikesToLess = 2,
        lessSpikesToMore = 3
    }
    public SERIES_TYPE _serieType;

    private int _successes = 0;
    private int _errors = 0;

    [Header("Configuration")]
    private int _level;
    private CabinaSeriesDifficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _currentDataDifficulty = GetComponent<CabinaSeriesDifficulty>().GenerateDataDifficulty(_level);
        GetComponent<AsteroidBlasterInput>().SetGameMode(AsteroidBlasterInput.GAME_MODE.series);
        RestartGame();

        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
    }

    private void RestartGame()
    {
        DestroyAsteroids();
        _serieType = SERIES_TYPE.lessSpikesToMore;
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
        _serieType = (SERIES_TYPE)(Random.Range(0, 4));
        _asteroidsInScene = Random.Range(_currentDataDifficulty.minAteroidesinScene, _currentDataDifficulty.maxAteroidesinScene - 1);

        if((_serieType == SERIES_TYPE.lessSpikesToMore || _serieType == SERIES_TYPE.moreSpikesToLess) && _asteroidsInScene >= 5)
        {
            _asteroidsInScene = 5;
        }
    }
    /// <summary>
    /// Starts the proccess to generate 
    /// the asteroids in game scene
    /// </summary>
    private void GenerateAsteroids()
    {
        //SpawnAteroidsHighToLow();
        //SpawnAteroidsLowToHigh();
        //SpawnAteroidsMoreToLess();
        SpawnAteroidsLessToMore();
        //switch (_serieType)
        //{
        //    case SERIES_TYPE.highToLow:
        //        break;
        //    case SERIES_TYPE.lowToHigh:
        //        break;
        //    case SERIES_TYPE.moreSpikesToLess:
        //        break;
        //    case SERIES_TYPE.lessSpikesToMore:
        //        break;
        //    default:
        //        break;
        //}
        //SpawnAsteroids();
        //SetSelectedAsteroids();
    }
    /// <summary>
    /// Generate asteroids and asing the value
    /// in the correct order. Low to high mode
    /// </summary>
    private void SpawnAteroidsLowToHigh()
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
        float scale = 0.8f;
        for (int i = 0; i < _asteroidsInScene; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroid, spawnAux[i].transform);
            newAsteroid.name = "Asteroid";
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(scale, 0, i, this);
            _generatedAsteroids.Add(newAsteroid);
            scale += 0.1f;
        }
    }

    private void SpawnAteroidsLessToMore()
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
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(1, i, i, this);
            _generatedAsteroids.Add(newAsteroid);
        }
    }

    /// <summary>
    /// Generate asteroids and asing the value
    /// in the correct order. High to low mode
    /// </summary>
    private void SpawnAteroidsHighToLow()
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
        float scale = 1.2f;
        for (int i = 0; i < _asteroidsInScene; i++)
        {
            GameObject newAsteroid = Instantiate(_asteroid, spawnAux[i].transform);
            newAsteroid.name = "Asteroid";
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(scale, 0, i,this);
            _generatedAsteroids.Add(newAsteroid);
            scale -= 0.1f;
        }
    }
    private void SpawnAteroidsMoreToLess()
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
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(1, i, _asteroidsInScene - i, this);
            _generatedAsteroids.Add(newAsteroid);
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

        foreach (GameObject currentGun in _gunList)
        {
            currentGun.GetComponent<Animator>().SetTrigger("Shot");
        }

        int selectedAsteroids = _generatedAsteroids.Count;
        foreach (GameObject currentAsteroid in _generatedAsteroids)
        {
            if (currentAsteroid.GetComponent<PickableAsteroid>().GetSelected())
            {
                selectedAsteroids--;
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
    public SERIES_TYPE GetSeriesType()
    {
        return _serieType;
    }
}

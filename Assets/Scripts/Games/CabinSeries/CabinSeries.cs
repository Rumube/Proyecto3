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
    private bool _firstGame = true;
    public List<GameObject> _playerOrder = new List<GameObject>();
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
        GenerateTarget();
        GenerateAsteroids();
        GenerateOrder();
    }
    /// <summary>
    /// Destroy the asteroids in the scene
    /// </summary>
    private void DestroyAsteroids()
    {
        List<GameObject> auxList = new List<GameObject>(_generatedAsteroids);
        for (int i = 0; i < _generatedAsteroids.Count; i++)
        {
            Destroy(auxList[i]);
        }
        _generatedAsteroids.Clear();
        _playerOrder.Clear();
    }
    /// <summary>
    /// Select from <see cref="SERIES_TYPE"/>
    /// </summary>
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
        switch (_serieType)
        {
            case SERIES_TYPE.highToLow:
                SpawnAteroidsHighToLow();
                break;
            case SERIES_TYPE.lowToHigh:
                SpawnAteroidsLowToHigh();
                break;
            case SERIES_TYPE.moreSpikesToLess:
                SpawnAteroidsMoreToLess();
                break;
            case SERIES_TYPE.lessSpikesToMore:
                SpawnAteroidsLessToMore();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Spawn the asteroids in order <see cref="SERIES_TYPE.lowToHigh"/>
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
    /// <summary>
    /// Spawn the asteroids in order <see cref="SERIES_TYPE.lessSpikesToMore"/>
    /// </summary>
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
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(0.8f, i, i, this);
            _generatedAsteroids.Add(newAsteroid);
        }
    }

    /// <summary>
    /// Spawn the asteroids in order <see cref="SERIES_TYPE.highToLow"/>
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
    /// <summary>
    /// Spawn the asteroids in order <see cref="SERIES_TYPE.moreSpikesToLess"/>
    /// </summary>
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
            newAsteroid.GetComponent<PickableAsteroid>().SetValues(0.8f, i, _asteroidsInScene - i - 1, this);
            _generatedAsteroids.Add(newAsteroid);
        }
    }
    /// <summary>
    /// Generates the order for the player
    /// </summary>
    private void GenerateOrder()
    {
        string order = "";
        if (_firstGame)
        {
            order = "¡Destruye los asteroides de ";
            _firstGame = false;
        }else
        {
            order = "¡Ahora de ";
        }
        switch (_serieType)
        {
            case SERIES_TYPE.highToLow:
                order += "mayor a menor tamaño!";
                break;
            case SERIES_TYPE.lowToHigh:
                order += "menor a mayor tamaño!";
                break;
            case SERIES_TYPE.moreSpikesToLess:
                order += "más a menos lados!";
                break;
            case SERIES_TYPE.lessSpikesToMore:
                order += "menos a más lados!";
                break;
            default:
                break;
        }
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(order, true);
    }
    /// <summary>
    /// Checks if the game is correct
    /// </summary>
    public void CheckIfIsCorrect()
    {
        bool correct = true;

        foreach (GameObject currentAsteroid in _generatedAsteroids)
        {
            if(currentAsteroid.GetComponent<PickableAsteroid>().GetPositionInOrder() != currentAsteroid.GetComponent<PickableAsteroid>().GetPlayerPositionOrder())
            {
                correct = false;
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
    /// <summary>
    /// Updates the positions of the asteroids selected by the user.
    /// </summary>
    private void UpdatePositions()
    {
        foreach (GameObject currentAsteroid in _playerOrder)
        {
            currentAsteroid.GetComponent<PickableAsteroid>().UpdateOrderText(_playerOrder.IndexOf(currentAsteroid));
        }
    }
    /// <summary>
    /// Shoot the asteroids in order leaving a waiting time between shots
    /// </summary>
    public IEnumerator FinishAnimation()
    {
        foreach (GameObject currentAteroid in _generatedAsteroids)
        {
            if (currentAteroid.GetComponent<PickableAsteroid>().GetSelected())
            {
                currentAteroid.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(currentAteroid.GetComponent<PickableAsteroid>().BrokenAsteroid());
            }
            foreach (GameObject currentGun in _gunList)
            {
                currentGun.GetComponent<Animator>().SetTrigger("Shot");
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.3f);
        ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(transform.position);
        ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(_successes, _errors);
        RestartGame();
    }
    /// <summary>
    /// Returns the value of <see cref="_serieType"/>
    /// </summary>
    /// <returns></returns>
    public SERIES_TYPE GetSeriesType()
    {
        return _serieType;
    }
    /// <summary>
    /// Adds the selected asteroid to the asteroid list <see cref="_playerOrder"/>
    /// </summary>
    /// <param name="newAteroid">Selected asteroid</param>
    public void AddAsteroidToPlayerOrder(GameObject newAteroid)
    {
        _playerOrder.Add(newAteroid);
        UpdatePositions();
    }
    /// <summary>
    /// Remove the selected asteroid to the asteroid list <see cref="_playerOrder"/>
    /// </summary>
    /// <param name="newAteroid">Selected asteroid</param>
    public void RemoveAsteroidToPlayerOrder(GameObject newAsteroid)
    {
        _playerOrder.Remove(newAsteroid);
        UpdatePositions();
    }
}

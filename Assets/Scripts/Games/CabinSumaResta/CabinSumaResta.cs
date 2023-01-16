using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinSumaResta : MonoBehaviour
{
    [Header("References")]
    public GameObject _asteroid;
    public List<GameObject> _spawnsList = new List<GameObject>();
    private List<GameObject> _generatedAsteroids = new List<GameObject>();

    [Header("Game Values")]
    private int _asteroidsInScene = 0;
    private int _targetAsteroids = 0;
    private int _initialSelecteds = 0;

    [Header("Configuration")]
    private int _level;
    private CabinSumaRestaDifficulty.dataDiffilcuty _currentDataDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _currentDataDifficulty = GetComponent<CabinSumaRestaDifficulty>().GenerateDataDifficulty(_level);

        RestartGame();

        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Prueba", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RestartGame()
    {
        //Borrar Asteroides
        DestroyAsteroids();
        //Generar nuevo target
        GenerateTarget();
        //Generar Ateroides
        GenerateAsteroids();
    }
    private void DestroyAsteroids()
    {
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
    private void CheckIfIsCorrect()
    {


    }

}

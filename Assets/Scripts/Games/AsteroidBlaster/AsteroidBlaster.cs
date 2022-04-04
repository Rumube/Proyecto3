using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using UnityEngine.UI;

public class AsteroidBlaster : MonoBehaviour
{
    //Geometry Forms
    [Header("Geometry forms")]
    public GameObject[] _geometryForms;

    //Game Configuration
    [SerializeField]
    private int _level;
    [SerializeField]
    private List<GameObject> _asteroids = new List<GameObject>();
    [SerializeField]
    Dictionary<Geometry.Geometry_Type, GameObject> _asteroidsDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();
    public bool _gameFinished = false;

    public bool _finishCreateAsteroids = false;
    public int _successes = 0;
    public int _mistakes = 0;

    bool _firstGame = true;
    AsteroidBalsterDifficulty.dataDiffilcuty _currentDataDifficulty;

    public AudioSource _audioSource;
    Text _textMessage;
    // Start is called before the first frame update
    void Start()
    {
        restartGame();
        //_audioSource = GetComponent<AudioSource>();
        _textMessage = GameObject.FindGameObjectWithTag("Order").GetComponent<Text>();
        //Speaker.Instance.VoiceForCulture("es-ES");
    }

    /// <summary>
    /// Obtains a dictionary with asteroids and their
    /// shapes and calls the function generateTargets
    /// </summary>
    void setTarget()
    {
        _asteroidsDic.Clear();
        _targetList.Clear();

        for (int i = 0; i < _currentDataDifficulty.numGeometryTargets; i++)
        {
            bool goodTarget = true;
            int index = 0;
            index = Random.Range(0, _currentDataDifficulty.targetsGeometry.Count);
            do
            {
                goodTarget = true;
                if (_targetList.Contains(_currentDataDifficulty.targetsGeometry[index]))
                {
                    goodTarget = false;
                    index++;
                    if (index >= _currentDataDifficulty.targetsGeometry.Count)
                        index = 0;
                }
            } while (!goodTarget);
            _targetList.Add(_currentDataDifficulty.targetsGeometry[index]);
        }
    }

    /// <summary>
    /// Generate the asteroids and
    /// start the value of the properties.
    /// </summary>
    void GenerateAsteroids()
    {
        DeleteAsteroids();
        #region Target Asteroids
        int index = 0;
        do
        {
            bool created = false;
            foreach(GameObject prefab in _geometryForms)
            {
                if(prefab.GetComponent<Geometry>()._geometryType == _targetList[index] && !created)
                {
                    created = true;
                    GameObject newAsteroid = Instantiate(prefab);
                    newAsteroid.SetActive(false);
                    newAsteroid.GetComponent<Asteroid>().InitAsteroid(_currentDataDifficulty.speedMovement, _currentDataDifficulty.speedRotation, gameObject);
                    _asteroids.Add(newAsteroid);
                    index++;
                    if (index >= _targetList.Count)
                        index = 0;
                }
            }
        } while (_asteroids.Count < _currentDataDifficulty.numTargets);
        #endregion

        #region Others Asteroids
        do
        {
            bool created = false;
            foreach (GameObject prefab in _geometryForms)
            {
                if (!_currentDataDifficulty.targetsGeometry.Contains(prefab.GetComponent<Geometry>()._geometryType) && !created)
                {
                    created = true;
                    GameObject newAsteroid = Instantiate(prefab);
                    newAsteroid.SetActive(false);
                    newAsteroid.GetComponent<Asteroid>().InitAsteroid(_currentDataDifficulty.speedMovement, _currentDataDifficulty.speedRotation, gameObject);
                    _asteroids.Add(newAsteroid);
                    index++;
                    if (index >= _currentDataDifficulty.possibleGeometry.Count)
                        index = 0;
                }
            }
        } while (_asteroids.Count < _currentDataDifficulty.numAsteroids);
        #endregion
        StartCoroutine(LaunchAsteroids());
    }

    /// <summary>
    /// Activates asteroids with a slight delay.
    /// </summary>
    IEnumerator LaunchAsteroids()
    {
        float time = 1.5f / _asteroids.Count;
        foreach(GameObject asteroid in _asteroids)
        {
            asteroid.SetActive(true);
            yield return new WaitForSeconds(time);
        }
        if (_firstGame)
        {
            ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
            _firstGame = false;
        }
        _finishCreateAsteroids = true;
        _gameFinished = false;
        foreach (GameObject asteroid in _asteroids)
        {
            asteroid.GetComponent<Asteroid>().GenerateNewTarget();
        }
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Destruye los asteroides con forma de c�rculo", 1.5f, _textMessage);
    }

    /// <summary>
    /// Start the procces to restart the game.
    /// </summary>
    void restartGame()
    {
        _currentDataDifficulty = GetComponent<AsteroidBalsterDifficulty>().GenerateDataDifficulty(_level);
        setTarget();
        GenerateAsteroids();
    }

    /// <summary>
    /// Destroy all the asteroids.
    /// </summary>
    void DeleteAsteroids()
    {
        StopCoroutine(LaunchAsteroids());
        foreach (GameObject asteroid in _asteroids)
            Destroy(asteroid);
        _asteroids.Clear();
    }

    /// <summary>
    /// Checks if the destroyed asteroid is correct.
    /// </summary>
    /// <param name="asteroid">Asteroid destroyed.</param>
    public void CheckIfIsCorrect(GameObject asteroid)
    {
        _asteroids.Remove(asteroid);
        if (_targetList.Contains(asteroid.GetComponent<Geometry>()._geometryType))
        {
            //"es-es-x-eea-local"
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("�Correcto!", 1.5f, _textMessage);
            _successes++;
        }
        else
        {
            _mistakes++;
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("�Te has equivocado!", 1.5f, _textMessage);
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
        if (CheckIfIsFinish())
        {
            ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.playing;
            _gameFinished = true;
            //TODO: Finish and generate score
            StopAllCoroutines();
            restartGame();
        }
    }

    /// <summary>
    /// Check if after this destroyed asteroid the game is over.
    /// </summary>
    /// <returns>True if is finished, false if is not.</returns>
    public bool CheckIfIsFinish()
    {
        bool finish = false;
        if(_asteroids.Count == 0)
            finish = true;
        else
        {
            bool existGeometry = false;
            foreach (GameObject currentAsteroid in _asteroids)
            {
                if (_targetList.Contains(currentAsteroid.GetComponent<Geometry>()._geometryType))
                {
                    existGeometry = true;
                }
            }
            if (!existGeometry)
                finish = true;
        }
        return finish;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateStarsTelescopeGeometry : MonoBehaviour
{
    [Header("Prefabs references")]
    public GameObject _star;
    public GameObject _starsParent;
    public List<Transform> _constelationPos = new List<Transform>();
    public GameObject _particles;
    public GameObject Square, Pentagon, Hexagon;

    [Header("Configuration")]
    private TelescopeGeometryDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    public bool _pressed = false;
    private bool _firstRound = true;
    int _randomNum;
    private GameObject _constelationGo;
    private List<GameObject> _gameStarList = new List<GameObject>();
    public GameObject _panelAppear;

    //public enum ConstelationType
    //{
    //    triángulo = 0,
    //    cuadrado = 1,
    //    pentágono = 2,
    //    hexágono = 3
    //}
    private Geometry.Geometry_Type _constelationType;



    // Start is called before the first frame update
    void Start()
    {
        _panelAppear.GetComponent<Animator>().Play("Static");
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _dataDifficulty = GetComponent<TelescopeGeometryDifficulty>().GenerateDataDifficulty(_level);
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        StartCoroutine(StartGenerate());
    }

    /// <summary>
    /// Starts the generation process
    /// </summary>
    public IEnumerator StartGenerate()
    {
        yield return new WaitForSeconds(0.3f);
        GenerateStars();
    }

    // Update is called once per frame
    void Update()
    {
        print("----------------"+_gameStarList.Count+ "----------------");
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            InputManager();
        }
    }

    /// <summary>
    /// Starts the generation process
    /// </summary>
    public IEnumerator GenerateNewOrde()
    {
        yield return new WaitForSeconds(1f);
        DestroyStars();
        GenerateStars();
        //_constelationType = (ConstelationType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ConstelationType)).Length);
    }
    /// <summary>
    /// Clear the lists of stars and destroy the stars in the scene
    /// </summary>
    private void DestroyStars()
    {
        GetComponent<TelescopeGeometryConstelationGenerator>().ClearConstelation();
        List<GameObject> auxList = new List<GameObject>(_starList);
        for (int i = 0; i < auxList.Count; i++)
        {
            Destroy(_starList[i]);
        }
        _starList.Clear();
        GameObject aux = _constelationGo;
        _constelationGo = null;
        Destroy(aux);
        _constelationGo = new GameObject();
    }

    /// <summary>
    /// Generate the number of stars in the constelation.
    /// </summary>

    private void NumberConstelationStars()
    {
        _gameStarList.Clear();
        _constelationType = _dataDifficulty.possibleGeometry[UnityEngine.Random.Range(0, _dataDifficulty.possibleGeometry.Count)];
        int randomPos = UnityEngine.Random.Range(0, _constelationPos.Count);
        int count = 1;
        switch (_constelationType)
        {
            case Geometry.Geometry_Type.triangle:
                _constelationGo = new GameObject();
                print("Dibuja una constelación con forma de triángulo");
                break;

            case Geometry.Geometry_Type.square:
                print("Dibuja una constelación con forma de cuadrado");
                _constelationGo = Instantiate(Square, _starsParent.transform);
                _constelationGo.transform.position = _constelationPos[randomPos].position;

                foreach (Transform child in _constelationGo.transform)
                {
                    child.GetComponent<TelescopeGeometryStars>().InitStart(gameObject, count);
                    count++;
                    _gameStarList.Add(child.gameObject);
                    
                }
                _gameStarList.Add(_gameStarList[0]);
                break;
                
            case Geometry.Geometry_Type.pentagon:
                print("Dibuja una constelación con forma de pentágono");
                _constelationGo = Instantiate(Pentagon, _starsParent.transform);
                _constelationGo.transform.position = _constelationPos[randomPos].position;
                foreach (Transform child in _constelationGo.transform)
                {
                    child.GetComponent<TelescopeGeometryStars>().InitStart(gameObject, count);
                    count++;
                    _gameStarList.Add(child.gameObject);
                }
                _gameStarList.Add(_gameStarList[0]);
                break;

            case Geometry.Geometry_Type.hexagon:
                print("Dibuja una constelación con forma de hexágono");
                 _constelationGo = Instantiate(Hexagon, _starsParent.transform);
                _constelationGo.transform.position = _constelationPos[randomPos].position;
                foreach (Transform child in _constelationGo.transform)
                {
                    child.GetComponent<TelescopeGeometryStars>().InitStart(gameObject, count);
                    count++;
                    _gameStarList.Add(child.gameObject);
                }
                _gameStarList.Add(_gameStarList[0]);
                break;

            default:
                print("Dibuja una constelación con forma de triángulo");

                break;
        }
    }

    /// <summary>
    /// Generate the stars in the scene.
    /// </summary>
    private void GenerateStars()
    {
        _randomNum = UnityEngine.Random.Range(_dataDifficulty.minStars, _dataDifficulty.maxStars);
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StarSpawn");
        List<GameObject> spawnsList = new List<GameObject>(spawns);
        List<int> posRepeated = new List<int>();

        do
        {
            int posRandom = UnityEngine.Random.Range(0, 14);

            if (!posRepeated.Contains(posRandom))
            {
                GameObject newStar = Instantiate(_star, _starsParent.transform);
                posRepeated.Add(posRandom);
                newStar.transform.position = spawnsList[posRandom].transform.position;
                _starList.Add(newStar);
                newStar.GetComponent<TelescopeGeometryStars>().InitStart(gameObject, 0);
            }

        } while (_starList.Count < _randomNum);

        NumberConstelationStars();
        string _textOrder = "";
        Geometry auxgeo = new Geometry();
        if (_firstRound)
        {
            _firstRound = false;
            _textOrder += "Forma una constelación con forma de " + (auxgeo.getGeometryString(_constelationType));
        }
        else
        {
            _textOrder += "Ahora con forma de " + (auxgeo.getGeometryString(_constelationType));
        }

        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_textOrder, true);

    }



    /// <summary>
    /// Controlls the input of the game
    /// </summary>
    private void InputManager()
    {
        AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
        if (newInput.result)
        {
            if (!_pressed)
            {
                _pressed = true;
                _particles.SetActive(true);

            }
            else
            {
                GetComponent<TelescopeGeometryConstelationGenerator>().UpdateLastPosition(newInput.pos);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newInput.pos, 1f);

            foreach (Collider2D currentCollider in colliders)
            {
                if (currentCollider.gameObject.tag == "Star" && !currentCollider.GetComponent<TelescopeGeometryStars>().GetIsConnected())
                {
                    currentCollider.gameObject.GetComponent<TelescopeGeometryStars>().CollisionDetected();
                }
            }
            if (_particles.activeSelf)
            {
                _particles.transform.position = Camera.main.ScreenToWorldPoint(newInput.pos);
            }
        }
        else
        {
            if (_pressed)
            {
                _pressed = false;
                _particles.SetActive(false);
                GetComponent<TelescopeGeometryConstelationGenerator>().CheckIfIsCorrect();

                foreach (GameObject star in _gameStarList)
                {
                    star.GetComponent<TelescopeGeometryStars>().SetCorrectConnection(false);
                }
            }
        }
    }
    public Geometry.Geometry_Type getConstelationType()
    {
        return _constelationType;
    }

public List<GameObject> GetGameStarsList()
{
    return _gameStarList;
}
}



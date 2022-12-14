using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateStarsTelescopeGeometry : MonoBehaviour
{
    [Header("Prefabs references")]
    public GameObject _star;
    public GameObject _starsParent;
    public GameObject _particles;
    public GameObject Square, Pentagon, Hexagon;

    [Header("Configuration")]
    private TelescopeGeometryDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    public bool _pressed = false;
    private bool _firstRound = true;
    int _randomNum;
    public List<GameObject> _starsConstelation = new List<GameObject>();
    private int _constelationStars;
    private int _Constelations = UnityEngine.Random.Range(1, 4);


    // Start is called before the first frame update
    void Start()
    {
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
        _starsConstelation.Clear();
    }

    /// <summary>
    /// Generate the number of stars in the constelation.
    /// </summary>

    private void NumberConstelationStars()
    {
        switch (_Constelations)
        {
            case 1:
                print("Dibuja una constelaci�n con forma de tri�ngulo");

                break;
            case 2:
                print("Dibuja una constelaci�n con forma de cuadrado");
                GameObject newSquare = Instantiate(Square, _starsParent.transform);    
                break;
            case 3:
                print("Dibuja una constelaci�n con forma de pent�gono");
                GameObject newPentagon = Instantiate(Pentagon, _starsParent.transform);
                break;
            case 4:
                print("Dibuja una constelaci�n con forma de hex�gono");
                GameObject newHexagon = Instantiate(Hexagon, _starsParent.transform);
                break;

            default:
                break;
        }

        _constelationStars = UnityEngine.Random.Range(_dataDifficulty.minStars, _randomNum);
        int starListPosition = 0;
        for (int i = 0; i < _constelationStars; i++)
        {
            _starsConstelation.Add(_starList[starListPosition]);
            starListPosition++;
        }
    }

    /// <summary>
    /// Generate the stars in the scene.
    /// </summary>
    private void GenerateStars()
    {
        _randomNum = UnityEngine.Random.Range(_dataDifficulty.minStars, _dataDifficulty.maxStars);
        Debug.Log("Estrellas random" + _randomNum);
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
                newStar.GetComponent<TelescopeGeometryStars>().InitStart(gameObject);
                //print("Position " + posRandom);
            }

        } while (_starList.Count < _randomNum);

        NumberConstelationStars();
        string _textOrder = "";

        if (_firstRound)
        {
            _firstRound = false;
            _textOrder += "Forma una constelaci�n con " + (_constelationStars) + " estrellas";
        }
        else
        {
            _textOrder += "Ahora con " + (_constelationStars) + " estrellas";
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
            }
        }
    }
}



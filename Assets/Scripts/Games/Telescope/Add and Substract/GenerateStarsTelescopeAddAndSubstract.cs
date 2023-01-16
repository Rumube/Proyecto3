using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateStarsTelescopeAddAndSubstract : MonoBehaviour
{
    [Header("Prefabs references")]
    public GameObject _star;
    public GameObject _starsParent;
    public List<Transform> _constelationPos = new List<Transform>();
    public GameObject _particles;
    //public GameObject Square, Pentagon, Hexagon;

    [Header("Configuration")]
    private TelescopeAddAndSubstractDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    private bool _firstRound = true;
    private int _randomNum;
    private bool starConnected = false;
    private int _randomNumStars;


    //public int _constelationPoints = UnityEngine.Random.Range(2, 15);



    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _dataDifficulty = GetComponent<TelescopeAddAndSubstractDifficulty>().GenerateDataDifficulty(_level);
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
        GenerateSelectedStars();
    }

    // Update is called once per frame
    void Update()
    {/*
        print("----------------" + _gameStarList.Count + "----------------");
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            InputManager();
        }*/
    }

    /// <summary>
    /// Starts the generation process
    /// </summary>
    public IEnumerator GenerateNewOrde()
    {
        yield return new WaitForSeconds(1f);
        DestroyStars();
        GenerateStars();
        GenerateSelectedStars();
        //_constelationType = (ConstelationType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ConstelationType)).Length);
    }
    /// <summary>
    /// Clear the lists of stars and destroy the stars in the scene
    /// </summary>
    private void DestroyStars()
    {
        GetComponent<TelescopeAddAndSubstractConstelationGenerator>().ClearConstelation();
        List<GameObject> auxList = new List<GameObject>(_starList);
        for (int i = 0; i < auxList.Count; i++)
        {
            Destroy(_starList[i]);
        }
        _starList.Clear();
        GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Clear();
    }

    /// <summary>
    /// Generate the number of stars in the constelation.
    /// </summary>

    private void NumberConstelationStars()
    {
        int randomPos = UnityEngine.Random.Range(0, _constelationPos.Count);
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
                starConnected = true;
                newStar.GetComponent<TelescopeAddAndSubstractStars>().InitStart(gameObject, 0);
            }

        } while (_starList.Count < _randomNum);

        NumberConstelationStars();
        string _textOrder = "";

        if (_firstRound)
        {
            _firstRound = false;
            starConnected = false;
            _textOrder += "Forma una constelación de " + (_randomNum) + " estrellas";
        }
        else
        {
            _textOrder += "Ahora con " + (_randomNum) + " estrellas";
        }

        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_textOrder, true);

    }
    private void GenerateSelectedStars()
    {
        _randomNumStars = UnityEngine.Random.Range(_dataDifficulty.minStars, _randomNum);
        List<int> _posRepeated = new List<int>();

        List<GameObject> auxList = new List<GameObject>(_starList);  
        System.Random random = new System.Random();
        int n = auxList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject value = auxList[k];
            auxList[k] = auxList[n];
            auxList[n] = value;
        }

        for (int i = 0; i < _randomNumStars; i++)
        {
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Add(auxList[i]);
        }

        //int _posRandom = UnityEngine.Random.Range(0, 14);

        //    if (!_posRepeated.Contains(_posRandom))
        //    {
        //        for (int i = 0; i < (GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Count < _starList.Count); i++)
        //        {
        //            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Add(_starList[_starList.Count - 1]);
        //        }
                
        //    }
  

    }
    public void AddStars(GameObject star)//no interactúan las estrellas al clickar (no entra aquí)
    {
        print("Pulsado");
        if (!starConnected)
        {
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Add(star);
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>().AddNewPosition(star.transform.position);
        }
        else
        {
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Remove(star);
        }

    }


    /*
    /// <summary>
    /// Controlls the input of the game
    /// </summary>
    private void InputManager()
    {
        AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
        if (newInput.result)
        {
            
            

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newInput.pos, 1f);

            foreach (Collider2D currentCollider in colliders)
            {
                if (currentCollider.gameObject.tag == "Star" && !currentCollider.GetComponent<TelescopeAddAndSubstractStars>().GetIsConnected())
                {
                    currentCollider.gameObject.GetComponent<TelescopeAddAndSubstractStars>().CollisionDetected();
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
                GetComponent<TelescopeAddAndSubstractConstelationGenerator>().CheckIfIsCorrect();
            }
        }
    }*/

    public int GetRandomNum()
    {
        return _randomNum;
    }
}



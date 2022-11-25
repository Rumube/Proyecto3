using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateStarsTelescopeAssociation : MonoBehaviour
{
    [Header("Prefabs references")]
    public GameObject _star;
    public GameObject _starsParent;
    public GameObject _particles;


    [Header("Configuration")]
    private TelescopeAssociationDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    public bool _pressed = false;
    private bool _firstRound = true;
    int randomNum;

    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _dataDifficulty = GetComponent<TelescopeAssociationDifficulty>().GenerateDataDifficulty(_level);
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
    }

    private void DestroyStars()
    {
        GetComponent<TelescopeAssociationConstelationGenerator>().ClearConstelation();
        List<GameObject> auxList = new List<GameObject>(_starList);
        for (int i = 0; i < auxList.Count; i++)
        {
            Destroy(_starList[i]);
        }
        _starList.Clear();
    }

    /// <summary>
    /// Generate the number of stars in the constelation.
    /// </summary>
    private void NumberConstelationStars()
    {
        int ConstelationStars = UnityEngine.Random.Range(_dataDifficulty.minStars, randomNum);
        print("Este " + ConstelationStars);
    }

    /// <summary>
    /// Generate the stars in the scene.
    /// </summary>
    private void GenerateStars()
    {
        randomNum = UnityEngine.Random.Range(_dataDifficulty.minStars, _dataDifficulty.maxStars);
        Debug.Log("Estrellas random" + randomNum);
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StarSpawn");
        List<GameObject> spawnsList = new List<GameObject>(spawns);
        List<int> posRepeated = new List<int>();

        for (int i = 0; i < randomNum; i++)
        {
            GameObject newStar = Instantiate(_star, _starsParent.transform);
            int posRandom = UnityEngine.Random.Range(0, 14);

            if (posRepeated.Contains(posRandom))
            {
              
            }
            //Generate star position
            else
            {
                posRepeated.Add(posRandom);
                newStar.transform.position = spawnsList[posRandom].transform.position;
                _starList.Add(newStar);
                newStar.GetComponent<TelescopeAssociationStars>().InitStart(gameObject);
                print("Position " + posRandom);
            }


        }

        string _textOrder = "";

        if (_firstRound)
        {
            _firstRound = false;
            _textOrder += "Forma una constelación con x estrellas";
        }
        else
        {
            _textOrder += "Ahora con ";
        }

        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_textOrder, true);
        // NumberConstelationStars();
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
                GetComponent<TelescopeAssociationConstelationGenerator>().UpdateLastPosition(newInput.pos);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newInput.pos, 1f);

            foreach (Collider2D currentCollider in colliders)
            {
                if (currentCollider.gameObject.tag == "Star" && !currentCollider.GetComponent<TelescopeAssociationStars>().GetIsConnected())
                {
                    currentCollider.gameObject.GetComponent<TelescopeAssociationStars>().CollisionDetected();
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
                GetComponent<TelescopeAssociationConstelationGenerator>().ClearConstelation();
            }
        }
    }
}



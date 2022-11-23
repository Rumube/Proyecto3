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
  

    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _dataDifficulty = GetComponent<TelescopeAssociationDifficulty>().GenerateDataDifficulty(_level);
        //StartCoroutine(GenerateNewOrde());
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        StartCoroutine(StartGenerate());
    }

    /// <summary>
    /// Starts the generation process
    /// </summary>
    public IEnumerator StartGenerate()
    {
        yield return new WaitForSeconds(0.3f);
        GenerateSize();
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
        GetComponent<ConstelationGenerator>().ClearConstelation();
        List<GameObject> auxList = new List<GameObject>(_starList);
        for (int i = 0; i < auxList.Count; i++)
        {
            Destroy(_starList[i]);
        }
        _starList.Clear();
    }

    /// <summary>
    /// Generate the stars in the scene.
    /// </summary>
    private void GenerateSize()
    {
        //int rnadomNum = Random.r
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StarSpawn");
        List<GameObject> spawnsList = new List<GameObject>(spawns);

        //for (int i = 0; i < maxSerie; i++)
        //{
        //    GameObject newStar = Instantiate(_star, _starsParent.transform);


        //    //Generate star position
        //    newStar.transform.position = spawnsList[i].transform.position;
        //    _starList.Add(newStar);
        //    if (_starList = )
        //    {
            
        //    }
        //    newStar.GetComponent<Star>().InitStart(gameObject);
        //}

        string _textOrder = "";

        if (_firstRound)
        {
            _firstRound = false;
            _textOrder += "Forma una constelación con 5 estrellas";
        }
        else
        {
            _textOrder += "Ahora con ";
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
                GetComponent<ConstelationGenerator>().UpdateLastPosition(newInput.pos);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newInput.pos, 1f);

            foreach (Collider2D currentCollider in colliders)
            {
                if (currentCollider.gameObject.tag == "Star" && !currentCollider.GetComponent<Star>().GetIsConnected())
                {
                    currentCollider.gameObject.GetComponent<Star>().CollisionDetected();
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
                GetComponent<ConstelationGenerator>().ClearConstelation();
            }
        }
    }
}



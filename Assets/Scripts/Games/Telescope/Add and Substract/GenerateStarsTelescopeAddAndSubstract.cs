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
    public GameObject _panelAppear;
    public GameObject _starAnim;

    [Header("Configuration")]
    private TelescopeAddAndSubstractDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    private bool _firstRound = true;
    private int _randomNum;
    private int _randomNumStars;
    private int _randomStars;

   

    // Start is called before the first frame update
    void Start()
    {
        _panelAppear.GetComponent<Animator>().Play("Static");
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
    {

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
                newStar.GetComponent<TelescopeAddAndSubstractStars>().SetStarConnected(true);
            
            }

        } while (_starList.Count < _randomNum);

        NumberConstelationStars();
        string _textOrder = "";

        _randomStars = UnityEngine.Random.Range(_dataDifficulty.minStars, _randomNum);

        if (_firstRound)
        {
            _firstRound = false;
            _textOrder += "Forma una constelación de " + (_randomStars) + " estrellas";
        }
        else
        {
            _textOrder += "Ahora con " + (_randomStars) + " estrellas";
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

    }
    public void AddStars(GameObject star)
    {
        if (!star.GetComponent<TelescopeAddAndSubstractStars>().GetStarConnected())
        {
            star.GetComponent<TelescopeAddAndSubstractStars>().SetStarConnected(true);
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Add(star);
            _starAnim.GetComponent<Animator>().Play("Star_Slected");

        }
        else
        {
            star.GetComponent<TelescopeAddAndSubstractStars>().SetStarConnected(false);
            GetComponent<TelescopeAddAndSubstractConstelationGenerator>()._playerStarList.Remove(star);
        }

    }

    public int GetRandomNum()
    {
        return _randomNum;
    }
    public int GetRandomStars()
    {
        return _randomStars;
    }
}



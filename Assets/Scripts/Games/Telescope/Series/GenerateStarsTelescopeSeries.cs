using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStarsTelescopeSeries : MonoBehaviour
{


    [Header("Prefabs references")]
    public List<GameObject> _spikesStars;
    public GameObject _star;
    public GameObject _starsParent;

    [Header("Configuration")]
    private TelescopeSeriesDifficulty.dataDiffilcuty _dataDifficulty;
    private int _level;
    public List<GameObject> _starList = new List<GameObject>();
    public bool _pressed = false;

    private enum SERIES_TYPE
    {
        highToLow = 0,
        lowToHigh = 1,
        moreSpikesToLess = 2,
        lessSpikesToMore = 4
    }
    private SERIES_TYPE _serieType;
    // Start is called before the first frame update
    void Start()
    {
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _dataDifficulty = GetComponent<TelescopeSeriesDifficulty>().GenerateDataDifficulty(_level);
        GenerateNewOrde();
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
    public void GenerateNewOrde()
    {
        
        //TODO: DESTROY STARS
        DestroyStars();

        _serieType = (SERIES_TYPE)Random.Range(0, System.Enum.GetValues(typeof(SERIES_TYPE)).Length);
        print("Tipo: " + _serieType);
        switch (_serieType)
        {
            case SERIES_TYPE.highToLow:
                GenerateSize(true);
                break;
            case SERIES_TYPE.lowToHigh:
                GenerateSize(false);
                break;
            case SERIES_TYPE.moreSpikesToLess:
                GenerateSpikes(true);
                break;
            case SERIES_TYPE.lessSpikesToMore:
                GenerateSpikes(false);
                break;
            default:
                break;
        }
    }

    private void DestroyStars()
    {
        List<GameObject> auxList = new List<GameObject>(_starList);
        for (int i = 0; i < auxList.Count; i++)
        {
            Destroy(_starList[i]);
        }
        _starList.Clear();
    }

    /// <summary>
    /// Generates the stars in the <see cref="SERIES_TYPE.highToLow"/> game.
    /// </summary>
    private void GenerateSize(bool isHighToLow)
    {
        int maxSerie = _dataDifficulty.maxSerie;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StarSpawn");
        List<GameObject> spawnsList = new List<GameObject>(spawns);

        for (int i = 0; i < maxSerie; i++)
        {
            GameObject newStar = Instantiate(_star, _starsParent.transform);
            //Generate star scale
            Vector2 newScale = new Vector2(transform.localScale.x + (i*0.3f), transform.localScale.x + (i * 0.3f));
            newStar.transform.localScale = newScale;

            //Generate star position
            int randomIndex = Random.Range(0, spawnsList.Count);
            newStar.transform.position = spawnsList[randomIndex].transform.position;
            spawnsList.RemoveAt(randomIndex);
            _starList.Add(newStar);

            newStar.GetComponent<Star>().InitStart(gameObject);
        }
        if (isHighToLow)
        {
            _starList.Reverse();
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Une las estrellas de mayor a menor tama�o", true);
        }
        else
        {
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Une las estrellas de menor a mayor tama�o", true);
        }
    }

    private void GenerateSpikes(bool isMoreToLees)
    {
        int maxSerie = _dataDifficulty.maxSerie;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StarSpawn");
        List<GameObject> spawnsList = new List<GameObject>(spawns);

        for (int i = 0; i < maxSerie; i++)
        {
            GameObject newStar = Instantiate(_spikesStars[i], _starsParent.transform);

            //Generate star position
            int randomIndex = Random.Range(0, spawnsList.Count);
            newStar.transform.position = spawnsList[randomIndex].transform.position;
            spawnsList.RemoveAt(randomIndex);
            _starList.Add(newStar);

            newStar.GetComponent<Star>().InitStart(gameObject);
        }
        if (isMoreToLees)
        {
            _starList.Reverse();
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Une las estrellas de mayor a menor puntas", true);
        }
        else
        {
            ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("Une las estrellas de menor a mayor puntas", true);
        }
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
                //GetComponent<ConstelationGenerator>().AddNewPosition(newInput.pos);
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
        }
        else
        {
            if (_pressed)
            {
                _pressed = false;
                GetComponent<ConstelationGenerator>().ClearConstelation();
            }
        }
    }
}

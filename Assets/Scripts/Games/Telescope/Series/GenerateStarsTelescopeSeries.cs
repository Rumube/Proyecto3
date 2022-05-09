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
        _serieType = (SERIES_TYPE)Random.Range(0, System.Enum.GetValues(typeof(SERIES_TYPE)).Length);
        switch (_serieType)
        {
            case SERIES_TYPE.highToLow:
                break;
            case SERIES_TYPE.lowToHigh:
                break;
            case SERIES_TYPE.moreSpikesToLess:
                break;
            case SERIES_TYPE.lessSpikesToMore:
                break;
            default:
                break;
        }
        GenerateHighToLow();
    }

    /// <summary>
    /// Generates the stars in the <see cref="SERIES_TYPE.highToLow"/> game.
    /// </summary>
    private void GenerateHighToLow()
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
        _starList.Reverse();
    }

    private void InputManager()
    {
        AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
        print(newInput);
        if (newInput.result)
        {            
            if (!_pressed)
            {
                _pressed = true;
                GetComponent<ConstelationGenerator>().AddNewPosition(newInput.pos);
            }
            else
            {
                GetComponent<ConstelationGenerator>().UpdateLastPosition(newInput.pos);
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newInput.pos, 1f);

            foreach (Collider2D currentCollider in colliders)
            {
                if (currentCollider.gameObject.name == "Star(Clone)" && !currentCollider.GetComponent<Star>().GetIsConnected())
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

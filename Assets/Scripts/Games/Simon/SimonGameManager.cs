using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGameManager : MonoBehaviour
{
    private List<Geometry.Geometry_Type> _playerTaskList = new List<Geometry.Geometry_Type>();
    private List<Geometry.Geometry_Type> _playerSequenceList = new List<Geometry.Geometry_Type>();

    public List<AudioClip> buttonSoundsList = new List<AudioClip>();

    public List<Button> clickableButtons;

    public AudioClip loseSound;

    public AudioSource audioSource;

    public CanvasGroup _buttons;

    public GameObject startButton;
    public GameObject _imgTapa;

    private SimonGameDifficulty.dataDiffilcuty _data;
    public int _level;
    private List<GameObject> _playableButtons = new List<GameObject>();
    public List<GameObject> _orderListButtons = new List<GameObject>();
    private int _rounds;
    private int _currentRounds = 0;
    private bool _canClick = false;

    private void Start()
    {
        _data = GetComponent<SimonGameDifficulty>().GenerateDataDifficulty(_level);
        _rounds = _data.roundList;
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        StartGame();
    }

    private void Update()
    {
        if (ServiceLocator.Instance.GetService<GMSinBucle>()._gameStateClient == GMSinBucle.GAME_STATE_CLIENT.playing && _canClick)
        {
            _buttons.interactable = true;
        }
        else
        {
            _buttons.interactable = false;
        }
    }

    #region Buttom
    /// <summary>
    /// Add to the PlayerSequence the button pressed
    /// </summary>
    /// <param name="button">Reference of the button</param>
    public void AddToPlayerSequenceList(Button button)
    {
        if (_playerSequenceList.Count < _playerTaskList.Count)
        {
            _playerSequenceList.Add(button.gameObject.GetComponent<Geometry>()._geometryType);
            if (_playerSequenceList.Count == _playerTaskList.Count)
            {
                _buttons.interactable = false;
                _canClick = false;
            }
            StartCoroutine(HighlightButton(button.gameObject));

            for (int i = 0; i < _playerSequenceList.Count; i++)
            {
                if (_playerTaskList[i] == _playerSequenceList[i])
                {
                    continue;
                }
                else
                {
                    ServiceLocator.Instance.GetService<IError>().GenerateError();
                    ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(0, 1);
                    Invoke("StartGame", _data.simonVelocity);
                    //StartCoroutine(PlayerLost());
                    return;
                }
            }
        }
        else
        {
            _buttons.interactable = false;
            _canClick = false;
        }
        if (_playerSequenceList.Count == _playerTaskList.Count && _currentRounds < _rounds)
        {
            _currentRounds++;
            print(_currentRounds);
            if (_currentRounds < _rounds)
            {
                StartCoroutine(StartNextRound());
            }
            else
            {
                ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
                ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(1, 0);
                Invoke("StartGame", _data.simonVelocity);
            }
        }

    }
    #endregion
    /// <summary>
    /// Starts the game parameters
    /// </summary>
    public void StartGame()
    {
        _imgTapa.SetActive(true);
        GameObject[] findButtons = GameObject.FindGameObjectsWithTag("GameButton");

        foreach (GameObject currentButton in findButtons)
        {
            if (currentButton.activeSelf)
                currentButton.SetActive(false);
        }
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("íCompleta la secuencia para aterrizar la nave!");
        _currentRounds = 0;
        _playerSequenceList.Clear();
        _playerTaskList.Clear();
        _orderListButtons.Clear();
        GeneratePanel();
        StartCoroutine(StartNextRound());
    }

    public IEnumerator HighlightButton(GameObject simonButton)
    {
        simonButton.GetComponent<GeometryButton>()._light.SetActive(true);//Highlighted Color
        yield return new WaitForSeconds(_data.simonVelocity);
        simonButton.GetComponent<GeometryButton>()._light.SetActive(false);
        yield return new WaitForSeconds(_data.simonVelocity);
    }

    /// <summary>
    /// Starts new round in the game
    /// </summary>
    public IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(_data.simonVelocity);
        _imgTapa.SetActive(false);
        _playerSequenceList.Clear();
        _canClick = false;
        yield return new WaitForSeconds(_data.simonVelocity);
        int indexButton = Random.Range(0, _data.numberButtons);
        _playerTaskList.Add(_playableButtons[indexButton].GetComponent<Geometry>()._geometryType);
        _orderListButtons.Add(_playableButtons[indexButton]);
        string pista = "";
        foreach (GameObject currentButton in _orderListButtons)
        {
            pista += " " + currentButton.GetComponent<GeometryButton>().getGeometryString();
            yield return StartCoroutine(HighlightButton(currentButton));
        }
        Debug.Log(pista);
        _canClick = true;
        yield return null;
    }
    private void GeneratePanel()
    {
        List<Button> auxList = new List<Button>(clickableButtons);
        _playableButtons.Clear();

        for (int i = 0; i < _data.numberButtons; i++)
        {
            int randIndex = Random.Range(0, auxList.Count);
            auxList[randIndex].gameObject.SetActive(true);

            auxList.RemoveAt(randIndex);
        }

        GameObject[] findButtons = GameObject.FindGameObjectsWithTag("GameButton");

        foreach (GameObject currentButton in findButtons)
        {
            if (currentButton.activeSelf)
            {
                _playableButtons.Add(currentButton);
            }
        }

    }
}



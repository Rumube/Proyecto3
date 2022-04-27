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

    public CanvasGroup buttons;

    public GameObject startButton;

    private SimonGameDifficulty.dataDiffilcuty _data;
    public int _level;
    private List<GameObject> _playableButtons = new List<GameObject>();
    private int _rounds;
    private int _currentRounds = 0;

    private void Start()
    {
        _data = GetComponent<SimonGameDifficulty>().GenerateDataDifficulty(_level);
        _rounds = _data.roundList;
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        StartGame();
    }

    private void Update()
    {
        if(GetComponent<GMSinBucle>()._gameStateClient == GMSinBucle.GAME_STATE_CLIENT.playing)
        {
            buttons.interactable = true;
        }
        else
        {
            buttons.interactable = false;
        }
    }

    #region Buttom
    /// <summary>
    /// Add to the PlayerSequence the button pressed
    /// </summary>
    /// <param name="button">Reference of the button</param>
    public void AddToPlayerSequenceList(Button button)
    {
        _playerSequenceList.Add(button.gameObject.GetComponent<Geometry>()._geometryType);

        //StartCoroutine(HighlightButton(buttonId));

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
                StartGame();
                //StartCoroutine(PlayerLost());
                return;
            }
        }
        if (_playerSequenceList.Count == _playerTaskList.Count && _currentRounds < _rounds)
        {
            _currentRounds++;
            print(_currentRounds);
            if(_currentRounds < _rounds)
            {
                StartCoroutine(StartNextRound());
            }
            else
            {
                ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
                ServiceLocator.Instance.GetService<ICalculatePoints>().Puntuation(1, 0);
                StartGame();
            }
        }
    }
    #endregion
    /// <summary>
    /// Starts the game parameters
    /// </summary>
    public void StartGame()
    {
        GameObject[] findButtons = GameObject.FindGameObjectsWithTag("GameButton");

        foreach (GameObject currentButton in findButtons)
        {
            if (currentButton.activeSelf)
                currentButton.SetActive(false);
        }
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage("¡Completa la secuencia para aterrizar la nave!");
        _currentRounds = 0;
        _playerSequenceList.Clear();
        _playerTaskList.Clear();
        GeneratePanel();
        StartCoroutine(StartNextRound());

    }

    public IEnumerator HighlightButton(int buttonId)
    {
        /* clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][1];//Highlighted Color
         audioSource.PlayOneShot(buttonSoundsList[buttonId]);*/
        yield return new WaitForSeconds(0.5f);
        // clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][0];//Regular Color
    }

    /// <summary>
    /// Starts new round in the game
    /// </summary>
    public IEnumerator StartNextRound()
    {
        _playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(_data.simonVelocity);
        int indexButton = Random.Range(0, _data.numberButtons);
        _playerTaskList.Add(_playableButtons[indexButton].GetComponent<Geometry>()._geometryType);
        string pista = "";
        foreach (Geometry.Geometry_Type geometry in _playerTaskList)
        {
            pista += " " + geometry.ToString();
            //  yield return StartCoroutine(HighlightButton(index));
        }
        Debug.Log(pista);
        buttons.interactable = true;
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



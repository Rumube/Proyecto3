using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGameManager : MonoBehaviour
{
    public List<Geometry.Geometry_Type> playerTaskList = new List<Geometry.Geometry_Type>();
    public List<Geometry.Geometry_Type> playerSequenceList = new List<Geometry.Geometry_Type>();

    public List<AudioClip> buttonSoundsList = new List<AudioClip>();

    public List<Button> clickableButtons;

    public AudioClip loseSound;

    public AudioSource audioSource;

    public CanvasGroup buttons;

    public GameObject startButton;

    private SimonGameDifficulty.dataDiffilcuty _data;
    public int _level;
    private List<GameObject> playableButtons = new List<GameObject>();
    

    private void Start()
    {
       _data = GetComponent<SimonGameDifficulty>().GenerateDataDifficulty(_level);
        StartGame();
    }
    public void AddToPlayerSequenceList(Button button)
    {
        playerSequenceList.Add(button.gameObject.GetComponent<Geometry>()._geometryType);

        //StartCoroutine(HighlightButton(buttonId));

        for (int i = 0; i < playerSequenceList.Count; i++)
        {
            if (playerTaskList[i]==playerSequenceList[i])
            {
                continue;
            }
            else
            {
                Debug.Log("You have lost");
                StartCoroutine(PlayerLost());
                return;
            }
        }

        if (playerSequenceList.Count == playerTaskList.Count)
        {
            Debug.Log("Start Next Round");
            StartCoroutine(StartNextRound());
        }
    }

    public void StartGame()
    {
        Debug.Log("StartNextRound");
        playerSequenceList.Clear();
        playerTaskList.Clear();
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

    public IEnumerator PlayerLost()
    {
        audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        yield return new WaitForSeconds(2f);
        startButton.SetActive(true);

    }

    public IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(_data.simonVelocity);
        int indexButton = Random.Range(0,_data.numberButtons);
        playerTaskList.Add(playableButtons[indexButton].GetComponent<Geometry>()._geometryType);
        string pista = "";
        foreach(int index in playerTaskList)
        {
            pista +=" "+playableButtons[index];
          //  yield return StartCoroutine(HighlightButton(index));
        }
        Debug.Log(pista);
        buttons.interactable = true;
        yield return null;
    }
    private void GeneratePanel()
    {
        List<Button> auxList = new List<Button>(clickableButtons);
        playableButtons.Clear();

        for (int i = 0; i < _data.numberButtons; i++)
        {
            int randIndex = Random.Range(0,auxList.Count);
            auxList[randIndex].gameObject.SetActive(true);
            
            auxList.RemoveAt(randIndex);            
        }

        GameObject[] findButtons = GameObject.FindGameObjectsWithTag("GameButton");

        foreach (GameObject currentButton in findButtons)
        {
            if (currentButton.activeSelf)
            {
                playableButtons.Add(currentButton);
            }
        }

    }
}



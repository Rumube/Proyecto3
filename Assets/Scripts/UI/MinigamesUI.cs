using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigamesUI : MonoBehaviour
{
    [Tooltip("Is this minigame needed a check?")]
    public bool _needsCheck;
    public Button _checkButton;
    public GameObject _rankingMinigame;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<GameManager>()._gameStateClient = GameManager.GAME_STATE_CLIENT.playing;
        _rankingMinigame.SetActive(false);
        if (_needsCheck)
        {
            _checkButton.gameObject.SetActive(true);
        }
        else
        {
            _checkButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.ranking && !_rankingMinigame.activeInHierarchy)
        {
            _rankingMinigame.SetActive(true);
        }
    }

    public void ContinueButtonMinigamesRanking()
    {
        ServiceLocator.Instance.GetService<GameManager>()._returnToCommonScene = true;
        SceneManager.LoadScene("CristinaTest");
    }
}

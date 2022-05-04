using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigamesUI : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Is this minigame needed a check?")]
    public bool _needsCheck;

    [Header("References")]
    public Button _checkButton;
    public GameObject _rankingMinigame;
    public Animator _panelScoreAnim;
    public Button _Min;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameManager>().SetClientState(IGameManager.GAME_STATE_CLIENT.playing);
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
        if(ServiceLocator.Instance.GetService<IGameManager>().GetClientState() != IGameManager.GAME_STATE_CLIENT.playing)
            _Min.interactable = false;
        else
            _Min.interactable = true;

        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.ranking && !_rankingMinigame.activeInHierarchy)
        {
            _rankingMinigame.SetActive(true);
            _panelScoreAnim.Play("ScorePanel");
        }
    }

    #region Buttoms
    /// <summary>
    /// Close the game scene and move to common scene.
    /// </summary>
    public void ContinueButtonMinigamesRanking()
    {
        ServiceLocator.Instance.GetService<IGameManager>().SetReturnToCommonScene(true);
        SceneManager.LoadScene("CristinaTest");
    }
    /// <summary>
    /// Repeat the last order gived by Min.
    /// </summary>
    public void RepeatLastOrder()
    {
        ServiceLocator.Instance.GetService<IFrogMessage>().RepeatLastOrder();
    }
    #endregion
}

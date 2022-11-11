using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class UI : MonoBehaviour,IUI
{
    protected int _uiIndex;
    protected List<GameObject> _windowsTree;
    public bool _continueNextScreen;
    public GameObject RawImage;
    private void Awake()
    {
        _windowsTree = new List<GameObject>();
        _uiIndex = 0;
    }

    /// <summary>Quit the game</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }

    /// <summary>Open/close the window credits</summary>
    public void CreditsMobile()
    {
        ServiceLocator.Instance.GetService<UIManager>()._credits.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._credits.activeSelf);
    }
    public void TutorialMobile()
    {
        ServiceLocator.Instance.GetService<UIManager>()._tutorial.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._tutorial.activeSelf);
    }
    public void CreditsTablet()
    {
        ServiceLocator.Instance.GetService<UIManager>()._creditsTablet.SetActive(!ServiceLocator.Instance.GetService<UIManager>()._creditsTablet.activeSelf);
    }

    /// <summary>Open the next window deppending on the position of the array and close the previous one</summary>
    public void OpenNextWindow()
    {
        if (_continueNextScreen)
        {
            //Add scene
            _uiIndex++;

            //if is not the final scene, load the next scene 
            if (_uiIndex < 9)
            {
               
                _windowsTree[_uiIndex].SetActive(true);
                _windowsTree[_uiIndex - 1].SetActive(false);
                
            }
               
           //if is the final scene, load the animation and the first scene
            if (_uiIndex == 9)
            {
                _windowsTree[0].SetActive(true);
                RawImage.SetActive(false);
                _windowsTree[_uiIndex - 1].SetActive(false);
                ServiceLocator.Instance.GetService<ServerUtility>().ResetConnections();
               _continueNextScreen = true;
                _uiIndex = 0;
            }
          
        }      
    }

    /// <summary>Open the previous window deppending on the position of the array and close the current one</summary>
    public void OpenPreviousWindow()
    {
        _uiIndex--;
        _windowsTree[_uiIndex].SetActive(true);
        _windowsTree[_uiIndex + 1].SetActive(false);
    }
   
}

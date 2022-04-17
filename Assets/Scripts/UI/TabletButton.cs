using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletButton : MonoBehaviour
{
    public TextMeshProUGUI _textButton;
    public Image _highlighted;

    /// <summary>Select the tablet with highlight and save the text number on UIManager</summary>
    public void SelectTabletAddingStudents()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightTablets();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet = int.Parse(_textButton.text);
        ServiceLocator.Instance.GetService<MobileUI>().UpdateNumberMininautas();
    }

    /// <summary>Select the tablet with highlight and open the game info panel on stadistics</summary>
    public void SelectTabletStadistics()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightTabletsStadistics();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<MobileUI>().OpenInfoTabletStudentGamePanel(int.Parse(_textButton.text));
    }

    /// <summary>Select the tablet with highlight and close the game info panel on stadistics</summary>
    public void SelectAllTabletsStadistics()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightTabletsStadistics();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<MobileUI>().CloseInfoTabletStudentGamePanel();
    }
}

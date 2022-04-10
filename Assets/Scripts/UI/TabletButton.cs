using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletButton : MonoBehaviour
{
    public Text _textButton;
    public Image _highlighted;

    /// <summary>Select the tablet with highlight and save the text number on UIManager</summary>
    public void SelectTabletAddingStudents()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightTablets();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<NetworkManager>()._selectedTablet = int.Parse(_textButton.text);
        ServiceLocator.Instance.GetService<MobileUI>().UpdateNumberMininautas();
    }

    public void SelectTabletStadistics()
    {
        ServiceLocator.Instance.GetService<MobileUI>().QuitHighlightTabletsStadistics();
        _highlighted.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<MobileUI>().OpenInfoTabletStudentGamePanel(int.Parse(_textButton.text));
    }

}

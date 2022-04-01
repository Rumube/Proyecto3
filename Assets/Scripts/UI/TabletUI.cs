using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
public class TabletUI : UI
{
    public InputField _ipServer;
    public InputField _portServer;

    [Header("Calling students")]
    public Text _studentsText;
    public Text _currentStudentName;
    public Button _continueCallingButton;
    private bool _callingStudents;
    private bool _continuecallingStudent;
    public Animator _astrounautAnimator;
    public Animator _rocketAnimator;
    public AudioSource _audioSource;

    [Header("Student selection")]
    public Text _studentName;
    public Text _gameName;
    public Text _gameInfo;
    // Start is called before the first frame update
    void Start()
    {
        //Get the last info saved
        if (PlayerPrefs.HasKey("IPServer") && PlayerPrefs.HasKey("PortServer"))
        {
            _ipServer.text = PlayerPrefs.GetString("IPServer");
            _portServer.text = PlayerPrefs.GetString("PortServer");
        }
        //Adding root windows in order of appearance
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._initialScreenTablet);
        _windowsTree.Add(ServiceLocator.Instance.GetService<UIManager>()._connection);

        //Desactive all windows
        for (int i = 0; i < _windowsTree.Count; ++i)
        {
            _windowsTree[i].SetActive(false);
        }
        //Active just the first one
        _windowsTree[_uiIndex].SetActive(true);
    }
    /// <summary>
    /// Save the values places in the input in order to have direct access to the connection if
    /// server port and ip doesn't change
    /// </summary>
    public void SaveIPPortInfo()
    {
        if(_ipServer.text != PlayerPrefs.GetString("IPServer"))
        {
            PlayerPrefs.SetString("IPServer", _ipServer.text);
        }
        if (_portServer.text != PlayerPrefs.GetString("PortServer"))
        {
            PlayerPrefs.SetString("PortServer", _portServer.text);
        }      
    }
    // Update is called once per frame
    void Update()
    {
        if (Client._tablet != null && Client._tablet._students != null && Client._tablet._students.Count > 0 && !_callingStudents)
        {
            _callingStudents = true;
            StartCoroutine(CallingStudents());
        }
    }
    public IEnumerator CallingStudents()
    {
        for (int i = 0; i < Client._tablet._students.Count; ++i)
        {
            _currentStudentName.text = Client._tablet._students[i]._name;
            Speaker.Instance.Speak(_currentStudentName.text + " a la nave", _audioSource);
            _continuecallingStudent = false;
            _continueCallingButton.gameObject.SetActive(true);
            yield return new WaitUntil(() => _continuecallingStudent == true);
            _continueCallingButton.gameObject.SetActive(false);
            yield return AstronautAnimation();
        }
        ServiceLocator.Instance.GetService<NetworkManager>().SendEndCalling();
        _rocketAnimator.Play("NaveDespegue");
    }
    IEnumerator AstronautAnimation()
    {
        _astrounautAnimator.Play("Astronaut");
        yield return new WaitForSeconds(8.0f);
        _studentsText.text += _currentStudentName.text + "\n";
    }
    public void ButtonContinueCallingStudent()
    {
        _continuecallingStudent = true;
    }

    public void NewStudentGame()
    {
        ShowStudentGame();
    }
    void ShowStudentGame()
    {
        ServiceLocator.Instance.GetService<GameManager>().SelectStudentAndGame();
        ServiceLocator.Instance.GetService<NetworkManager>().SendStudentGame();
        _studentName.text = ServiceLocator.Instance.GetService<GameManager>()._currentstudentName;
        _gameName.text = ServiceLocator.Instance.GetService<GameManager>()._currentgameName;
    }

}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanelAddSubs : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;
    public int _row;
    public int _gapX;
    public int _gapY;
    public int _offsetX;
    public int _offsetY;

    int _count;

    public GameObject _canvas;
    public GameObject _panelAppear;
    [Header("Geometry")]
    public GameObject button;
    public GameObject[] _geometryForms;
    public List<GameObject> _geometryList = new List<GameObject>();
    public List<GameObject> _targetList = new List<GameObject>();
    public List<GameObject> _allList = new List<GameObject>();
    private List<Geometry.Geometry_Type> _typeTargetGeometry = new List<Geometry.Geometry_Type>();

    //Game Configuration
    [SerializeField]
    private int _level;
    public int _pressedButtons;
    public int _orderButtons;
    [SerializeField]
    
    AddPanelDifficulty _completeThePanel;
    AddPanelDifficulty.dataDiffilcuty _currentDataDifficulty;

    public ButtonManager _buttonManager;
    void Start()
    {
        _panelAppear.GetComponent<Animator>().Play("Static");

        _completeThePanel = GetComponent<AddPanelDifficulty>();
        _currentDataDifficulty = _completeThePanel.GenerateDataDifficulty(_level);
       
        GeneratePanel();
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
    }

    /// <summary>
    /// Creates a panel with geometry.
    /// </summary>
    void GeneratePanel()
    {
        _pressedButtons = UnityEngine.Random.Range(0, _column*_row);
        
        if (_pressedButtons+_currentDataDifficulty.elementToAddSubs>_column*_row)
        {
            _orderButtons = _pressedButtons - _currentDataDifficulty.elementToAddSubs;
        }
        else
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random==0)
            {
                _pressedButtons = UnityEngine.Random.Range(_currentDataDifficulty.elementToAddSubs+1, _column * _row);
                //if (_pressedButtons == _currentDataDifficulty.elementToAddSubs)
                //{
                //    _pressedButtons = _column * _row - _pressedButtons;
                //}
                _orderButtons = _pressedButtons - _currentDataDifficulty.elementToAddSubs;
               
                EDebug.Log("resta");
            }
            else
            {
                _orderButtons = _pressedButtons + _currentDataDifficulty.elementToAddSubs;
                EDebug.Log("suma");

            }
        }
        _geometryList.Clear();
        _targetList.Clear();
        _allList.Clear();
        GenerateTargets();
        GenerateNoTargetGeometry();

        //COLOCAR BOTONES EN POSICIÓN
        randomize(_allList, _allList.Count);
        for (int x = 0; x < _column; x++)
        {
            for (int y = 0; y < _row; y++)
            {
                _allList[_count].GetComponent<Transform>().position = new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0);
                _allList[_count].transform.SetParent(_canvas.transform, false);
                _count++;
               
            }
        }
        Invoke("SendMessage", 1f);
    }
    static void randomize(List<GameObject> arr, int n)
    {
        // Creating a object
        // for Random class
        var rand = new System.Random(UnityEngine.Random.Range(0, n));

        // Start from the last element and
        // swap one by one. We don't need to
        // run for the first element
        // that's why i > 0
        for (int i = n - 1; i > 0; i--)
        {

            // Pick a random index
            // from 0 to i
            int j = rand.Next(0, i + 1);

            // Swap arr[i] with the
            // element at random index
            GameObject temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        // Prints the random array
        for (int i = 0; i < n; i++)
            Console.Write(arr[i] + " ");
    }

    private void SendMessage()
    {
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_buttonManager.GetTextGame(), true);
    }
    /// <summary>
    /// Generates the normal geometry.
    /// </summary>
    private void GenerateNoTargetGeometry()
    {
        for (int i = 0; i < (_row * _column) - _pressedButtons; i++)
        {
            GameObject newGeometry;
            newGeometry = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
            //newGeometry.transform.SetParent(_canvas.transform, false);
            newGeometry.GetComponent<GeometryButton>()._isPresed = true;
            newGeometry.GetComponent<GeometryButton>()._light.SetActive(false);
            
            _geometryList.Add(newGeometry);
            _allList.Add(newGeometry);
        }

    }
    /// <summary>
    /// Generates the target geometry.
    /// </summary>
    private void GenerateTargets()
    {
        //GenerateTypeTargetGeometry();
        //do
        //{
            for (int i = 0; i < _allList.Count; i++)
            {
                Destroy(_allList[i]);
            }
            _targetList.Clear();
            _allList.Clear();
            for (int i = 0; i < _pressedButtons; i++)
            {
                int idGeometry = UnityEngine.Random.Range(0, _currentDataDifficulty.targetGeometry.Count);
                GameObject newGeometry = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
                _buttonManager._buttonCounter = 1 + _buttonManager._buttonCounter;
                newGeometry.GetComponent<Image>().sprite = newGeometry.GetComponent<ObjectPanel>()._pressedSprite;
                newGeometry.GetComponent<ObjectPanel>()._placed = false;
                newGeometry.GetComponent<ObjectPanel>()._pressed = true;

            newGeometry.GetComponent<GeometryButton>()._isPresed = false;
            newGeometry.GetComponent<GeometryButton>()._light.SetActive(true);

            _targetList.Add(newGeometry);
                _allList.Add(newGeometry);
                //newGeometry.transform.SetParent(_canvas.transform, false);
            }
        //} while (!CheckTargets());
    }
   
    
    // Update is called once per frame
    void Update()
    {
        if (_allList.Count == _row * _column)
        {
            if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() != IGameManager.GAME_STATE_CLIENT.playing)
            {
                for (int i = 0; i < _allList.Count; i++)
                {
                    _allList[i].GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                for (int i = 0; i < _allList.Count; i++)
                {
                    _allList[i].GetComponent<Button>().interactable = true;
                }
            }
        }

    }

    /// <summary>
    /// Restarts the minigame.
    /// </summary>
    public void Restart()
    {
        foreach (GameObject geometry in _allList)
        {
            Destroy(geometry);
        }
        _count = 0;
        GeneratePanel();
    }
}




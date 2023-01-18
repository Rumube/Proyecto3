using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel_Prueba : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;
    public int _row;
    public int _gapX;
    public int _gapY;
    public float _offsetX;
    public float _offsetY;

    int _count;

    public GameObject _canvas;
    public GameObject _panelAppear;
    [Header("Geometry")]
    public GameObject[] _geometryForms;
    public List<GameObject> _geometryList = new List<GameObject>();
    public List<GameObject> _targetList = new List<GameObject>();
    public List<GameObject> _allList = new List<GameObject>();
    public List<Geometry.Geometry_Type> _typeTargetGeometry = new List<Geometry.Geometry_Type>();
    public ButtonCounter_Prueba _buttomCounter;
    //Game Configuration
    private int _level;

    private Button _checkButton;
    CompleteThePanelDifficulty.dataDiffilcuty _currentDataDifficulty;

    void Start()
    {
        _panelAppear.GetComponent<Animator>().Play("Static");
        _level = ServiceLocator.Instance.GetService<INetworkManager>().GetMinigameLevel();
        _currentDataDifficulty = GetComponent<CompleteThePanelDifficulty>().GenerateDataDifficulty(_level);
        GeneratePanel();
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        _checkButton = GameObject.FindGameObjectWithTag("CheckButton").GetComponent<Button>();
    }
    // Update is called once per frame
    void Update()
    {
        if(ServiceLocator.Instance.GetService<IGameManager>().GetClientState() != IGameManager.GAME_STATE_CLIENT.playing && _allList[0].GetComponent<Button>().interactable)
        {
            foreach (GameObject currentButton in _allList)
            {
                currentButton.GetComponent<Button>().interactable = false;
            }
            _checkButton.interactable = false;
        }
        else if(ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing && !_allList[0].GetComponent<Button>().interactable)
        {
            foreach (GameObject currentButton in _allList)
            {
                currentButton.GetComponent<Button>().interactable = true;
            }
            _checkButton.interactable = true;
        }


        //if (_allList.Count == _row * _column)
        //{
        //    if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient != GameManager.GAME_STATE_CLIENT.playing)
        //    {
        //        for (int i = 0; i < _allList.Count; i++)
        //        {
        //            _allList[i].GetComponent<Button>().interactable = false;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _allList.Count; i++)
        //        {
        //            _allList[i].GetComponent<Button>().interactable = true;
        //        }
        //    }
        //}
    }
    /// <summary>
    /// Creates a panel with geometry.
    /// </summary>
    void GeneratePanel()
    {
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
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_buttomCounter.GetTextGame(), true);
    }
    /// <summary>
    /// Generates the normal geometry.
    /// </summary>
    private void GenerateNoTargetGeometry()
    {
        for (int i = 0; i < (_row * _column) - _currentDataDifficulty.numTargets; i++)
        {
            int geometryID = UnityEngine.Random.Range(0, _currentDataDifficulty.possibleGeometry.Count);
            if (geometryID >= 7)
                geometryID = 6;

            GameObject newGeometry;
            newGeometry = Instantiate(_currentDataDifficulty.possibleGeometry[geometryID], new Vector3(0, 0, 0), Quaternion.identity);
            //newGeometry.transform.SetParent(_canvas.transform, false);
            _geometryList.Add(newGeometry);
            _allList.Add(newGeometry);
        }

    }
    /// <summary>
    /// Generates the target geometry.
    /// </summary>
    private void GenerateTargets()
    {
        GenerateTypeTargetGeometry();
        do
        {
            for (int i = 0; i < _allList.Count; i++)
            {
                Destroy(_allList[i]);
            }
            _targetList.Clear();
            _allList.Clear();
            for (int i = 0; i < _currentDataDifficulty.numTargets; i++)
            {
                int idGeometry = UnityEngine.Random.Range(0, _currentDataDifficulty.targetGeometry.Count);
                bool isCorrect = false;
                do
                {
                    isCorrect = false;

                    if (!_typeTargetGeometry.Contains(_currentDataDifficulty.targetGeometry[idGeometry].GetComponent<Geometry>()._geometryType))
                    {
                        EDebug.Log("discart");
                        idGeometry++;
                        if (idGeometry >= _currentDataDifficulty.targetGeometry.Count)
                        {
                            idGeometry = 0;
                        }
                    }
                    else
                    {
                        GameObject newGeometry = Instantiate(_currentDataDifficulty.targetGeometry[idGeometry], new Vector3(0, 0, 0), Quaternion.identity);
                        newGeometry.GetComponent<ObjectPanel_Prueba>()._placed = false;
                        _targetList.Add(newGeometry);
                        _allList.Add(newGeometry);
                        //newGeometry.transform.SetParent(_canvas.transform, false);
                        isCorrect = true;
                    }

                } while (!isCorrect);



            }
        } while (!CheckTargets());
    }
    /// <summary>
    /// Generates the geometry type of the target.
    /// </summary>
    private void GenerateTypeTargetGeometry()
    {
        _typeTargetGeometry.Clear();
        for (int i = 0; i < _currentDataDifficulty.numGeometryTargets; i++)
        {
            bool isCorrect = false;
            int idGeometry = UnityEngine.Random.Range(0, _currentDataDifficulty.targetGeometry.Count);
            do
            {
                isCorrect = false;
                Geometry.Geometry_Type newGeometry = _currentDataDifficulty.targetGeometry[idGeometry].GetComponent<Geometry>()._geometryType;
                if (!_typeTargetGeometry.Contains(newGeometry))
                {
                    _typeTargetGeometry.Add(newGeometry);
                    isCorrect = true;
                }
                else
                {
                    idGeometry++;
                    if (idGeometry >= _currentDataDifficulty.targetGeometry.Count)
                    {
                        idGeometry = 0;
                    }
                }
            } while (!isCorrect);
        }

    }
    /// <summary>
    /// Checks if the list of targets contains geometry repeated.
    /// </summary>
    private bool CheckTargets()
    {
        bool result = true;
        List<Geometry.Geometry_Type> geoTypeTarget = new List<Geometry.Geometry_Type>();
        foreach (GameObject currentTarget in _targetList)
        {
            geoTypeTarget.Add(currentTarget.GetComponent<Geometry>()._geometryType);
        }
        foreach (Geometry.Geometry_Type currentGeometryTpe in _typeTargetGeometry)
        {
            if (!geoTypeTarget.Contains(currentGeometryTpe))
            {
                result = false;
            }
        }
        return result;
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



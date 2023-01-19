using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel_1 : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;
    public int _row;
    public int _gapX;
    public int _gapY;
    public float _offsetX;
    public float _offsetY;

    int _count;

    public GameObject _geometrySpawn;
    [Header("Geometry")]
    public GameObject[] _geometryForms;
    public List<GameObject> _firstList = new List<GameObject>();
    public List<GameObject> _diferentTargetList = new List<GameObject>();
    public List<GameObject> _allList = new List<GameObject>();
    private ButtonCounter _buttomCounter;
    public GameObject _GeometryButtons;
    public List<GameObject> _modifiedList = new List<GameObject>();
    public List<GameObject> _geometryList = new List<GameObject>();
    //Game Configuration
    [SerializeField]
    private int _level;

    [SerializeField]
    CompleteThePanelDifficulty_1 _completeThePanel;
    CompleteThePanelDifficulty_1.dataDiffilcuty _currentDataDifficulty;


    void Start()
    {
        _completeThePanel = GetComponent<CompleteThePanelDifficulty_1>();

        _currentDataDifficulty = _completeThePanel.GenerateDataDifficulty(_level);
        _buttomCounter = GetComponent<ButtonCounter>();
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        GeneratePanel();
    }

    /// <summary>
    /// Creates a panel with geometry.
    /// </summary>
    void GeneratePanel()
    {
        _diferentTargetList.Clear();
        _allList.Clear();

        for (int y = 0; y < _column; y++)
        {
            for (int x = 0; x < _row; x++)
            {
                _count++;
                print("col: " + y + " / row: " + x);
                if (x % 2 == 0)
                {
                    GameObject newGeometry = Instantiate(_GeometryButtons, _geometrySpawn.transform);
                    newGeometry.GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.circle;
                    newGeometry.GetComponent<ButtonCounter_1>().SetTrueGeometry(newGeometry.GetComponent<Geometry>()._geometryType);
                    newGeometry.transform.localPosition = new Vector3(x, y, 0);
                    _geometryList.Add(newGeometry);
                }
                else
                {
                    GameObject otherGeometry = Instantiate(_GeometryButtons, _geometrySpawn.transform);
                    otherGeometry.GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.square;
                    otherGeometry.GetComponent<ButtonCounter_1>().SetTrueGeometry(otherGeometry.GetComponent<Geometry>()._geometryType);
                    otherGeometry.transform.localPosition = new Vector3(x, y, 0);
                    _geometryList.Add(otherGeometry);
                }

            }
        }
        ModifyGeomtry();
        Invoke("SendMessage", 1f);
    }
    public void ModifyGeomtry()
    {
        _geometryList[3].GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.triangle;
    }

    public void CheckGeometry() //Comprobar las listas
    {
        bool correct = true;
        for (int i = 0; i < _geometryList.Count; i++)
        {
            if (_geometryList[i].GetComponent<ButtonCounter_1>().GetTrueGeometry() != _geometryList[i].GetComponent<Geometry>()._geometryType)
            {
                correct = false;
            }
        }
        if (correct)
        {
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
        }
        else
        {
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
    }
    private void SendMessage()
    {
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_buttomCounter.GetTextGame(), true);
    }

    /// <summary>
    /// Generates the target geometry.
    /// </summary>
    private void GenerateTargets()
    {

    }
    /// <summary>
    /// Generates the geometry type of the target.
    /// </summary>
    private void GenerateTypeTargetGeometry()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //if (_allList.Count == _row * _column)
        //{
        //    if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() != IGameManager.GAME_STATE_CLIENT.playing)
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




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

    public GameObject _canvas;
    [Header("Geometry")]
    public GameObject[] _geometryForms;
    public List<GameObject> _firstList = new List<GameObject>();
    public List<GameObject> _diferentTargetList = new List<GameObject>();
    public List<GameObject> _allList = new List<GameObject>();
    private ButtonCounter _buttomCounter;
    public GameObject _GeometryButtons;
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

        for (int x = 0; x < _column; x++)
        {
            for (int y = 0; y < _row; y++)
            {
                _count++;

                if (y % 2 == 0)
                {
                    GameObject newGeometry = Instantiate(_GeometryButtons);
                    newGeometry.GetComponent<Geometry>()._geometryType = Geometry.Geometry_Type.circle;
                }
            }
        }

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
        Invoke("SendMessage", 1f); //cambiar mensaje por apuntadp en block de notas
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
    }

    private void SendMessage()
    {
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_buttomCounter.GetTextGame(), true);
    }
    /// <summary>
    /// Generates the normal geometry.
    /// </summary>
    private void GenerateNoTargetGeometry() //REVISAR
    {
        for (int i = 0; i < (_row * _column) - _currentDataDifficulty.numTargets; i++)
        {

        }
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




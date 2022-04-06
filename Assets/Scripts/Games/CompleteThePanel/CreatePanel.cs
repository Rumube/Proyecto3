using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;
    public int _row;
    public int _gapX;
    public int _gapY;
    public int _offsetX;
    public int _offsetY;

    public GameObject _canvas;
    [Header("Geometry")]
    public GameObject[] _geometryForms;
    public List<GameObject> _geometryList= new List<GameObject>();
    public List<GameObject> _targetList = new List<GameObject>();
    private List<Geometry.Geometry_Type> _typeTargetGeometry = new List<Geometry.Geometry_Type>();

    //Game Configuration
    [SerializeField]
    private int _numberEmpty;
    [SerializeField]
    private int _level;
    int targetPlaced;
    [SerializeField]
    Dictionary<Geometry.Geometry_Type, GameObject> _geometryDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    //public List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();
    CompleteThePanelDifficulty completeThePanel;
    CompleteThePanelDifficulty.dataDiffilcuty _currentDataDifficulty;

    GameObject[] buttons;
    void Start()
    {
        completeThePanel = GetComponent<CompleteThePanelDifficulty>();
        _currentDataDifficulty = completeThePanel.GenerateDataDifficulty(_level);
        //SetTarget();
        //SetNumberEmpty();
        GeneratePanel();
        buttons = GameObject.FindGameObjectsWithTag("GameButton");
    }
    //void SetTarget()
    //{
    //    _geometryDic.Clear();
    //    _targetList.Clear();

    //    //foreach (GameObject geometry in _geometryForms)
    //    //{
    //    //    _geometryDic.Add(geometry.GetComponent<Geometry>()._geometryType, geometry);
    //    //}
    //    foreach (GameObject geometry in _geometryForms)
    //    {
    //        _geometryDic.Add(geometry.GetComponent<Geometry>()._geometryType, geometry);
    //    }
    //    List<Geometry.Geometry_Type> keyList = new List<Geometry.Geometry_Type>(this._geometryDic.Keys);
    //    _targetList = GetComponent<TargetSelector>().generateTargets(keyList, _level);
    //}
    //void SetNumberEmpty()
    //{
    //    _numberEmpty = 3 + (_level / 2);
    //    if (_numberEmpty > 8)
    //        _numberEmpty = 8;
    //}


  

    /// <summary>
    /// Creates a panel with geometry.
    /// </summary>
    void GeneratePanel()
    {
        _geometryList.Clear();

        GenerateTargets(0, 0);
        GenerateNoTargetGeometry(0, 0);

        //COLOCAR BOTONES EN POSICIÓN

        //for (int x = 0; x < _column; x++)
        //{
        //    for (int y = 0; y < _row; y++)
        //    {
        //        if((_geometryList.Count + _targetList.Count) < (_column * _row))
        //        {
        //            if (_targetList.Count >= _currentDataDifficulty.numTargets)
        //            else
                        
        //        }

        //    }
        //}
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
    }


    private void GenerateNoTargetGeometry(int x, int y)
    {
        for (int i = 0; i < (_row*_column) - _currentDataDifficulty.numTargets; i++)
        {
            int geometryID = Random.Range(0, _currentDataDifficulty.possibleGeometry.Count);
            if (geometryID >= 7)
                geometryID = 6;

            GameObject newGeometry;
            print("Hola: " + x + " - " + y);
            newGeometry = Instantiate(_currentDataDifficulty.possibleGeometry[geometryID], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
            newGeometry.transform.SetParent(_canvas.transform, false);
            _geometryList.Add(newGeometry);
        }

    }

    private void GenerateTargets(int x, int y)
    {
        GenerateTypeTargetGeometry();
        do
        {
            _targetList.Clear();
            for (int i = 0; i < _currentDataDifficulty.numTargets; i++)
            {
                int idGeometry = Random.Range(0, _currentDataDifficulty.targetGeometry.Count);
                GameObject newGeometry = Instantiate(_currentDataDifficulty.targetGeometry[idGeometry], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
                newGeometry.GetComponent<ObjectPanel>()._placed = false;
                _targetList.Add(newGeometry);
                newGeometry.transform.SetParent(_canvas.transform, false);
            }
        } while (!CheckTargets());
    }

    private void GenerateTypeTargetGeometry()
    {
         _typeTargetGeometry.Clear();
         for (int i = 0; i < _currentDataDifficulty.numGeometryTargets; i++)
         {
            bool isCorrect = false;
            int idGeometry = Random.Range(0, _currentDataDifficulty.targetGeometry.Count);
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
                    if(idGeometry >= _currentDataDifficulty.targetGeometry.Count)
                    {
                        idGeometry = 0;
                    }
                }
            } while (!isCorrect);
         }

    }
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
    // Update is called once per frame
    void Update()
    {
        
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient != GameManager.GAME_STATE_CLIENT.playing)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Button>().interactable = false;
            } 
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }
    }

     public void Restart()
     {
        foreach (GameObject geometry in _geometryList)
        {
            Destroy(geometry);
            //_geometryList.Remove(geometry);
        }
        _geometryList.Clear();
        //for (int i = 0; i < _geometryList.Count; i++)
        //{
        //    Destroy(_geometryList[i]);
        //    //_geometryList.RemoveAt(i);
        //}
        targetPlaced = 0;
        _geometryList.Clear();
        _targetList.Clear();

        //SetTarget();
        //SetNumberEmpty();
        GeneratePanel();
     }
}




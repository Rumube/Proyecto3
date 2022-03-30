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
      int maxValue = _level + 1;
      if (_level > _geometryForms.Length)
                maxValue = _geometryForms.Length - 1;

        for (int x = 0; x < _column; x++)
        {
            for (int y = 0; y < _row; y++)
            {
                    int geometryID = Random.Range(0, _currentDataDifficulty.allGeometry.Count);
                if (geometryID >= 7)
                {
                        geometryID = 6;
                }


                GameObject newGeometry;
                //GameObject newGeometry = Instantiate(_geometryForms[geometryID], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
                if (_currentDataDifficulty.numTargets>targetPlaced)
                {
                    
                    if (_currentDataDifficulty.numTargets -targetPlaced==_currentDataDifficulty.numGeometryTargets- 1 )
                    {
                        for (int i = 0; i < _currentDataDifficulty.targetGeometry.Count; i++)
                        {
                            if (!_targetList.Contains(_currentDataDifficulty.targetGeometry[i]))
                            {
                                newGeometry = Instantiate(_currentDataDifficulty.targetGeometry[i], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
                                newGeometry.GetComponent<ObjectPanel>()._placed = false;
                                _targetList.Add(newGeometry);
                                
                                targetPlaced++;
                               // EDebug.Log("Targetnumber" + targetPlaced+"x"+ _currentDataDifficulty.numTargets - targetPlaced);
                                newGeometry.transform.SetParent(_canvas.transform, false);
                                //newGeometry.GetComponent<Geometry>()._geometryType = _currentDataDifficulty.possibleGeometry[Random.Range(0, _currentDataDifficulty.possibleGeometry.Count)];
                                _geometryList.Add(newGeometry);
                                break;
                            }
                        }
                    }
                    else
                    {
                        newGeometry = Instantiate(_currentDataDifficulty.targetGeometry[Random.Range(0, _currentDataDifficulty.targetGeometry.Count)], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
                        newGeometry.GetComponent<ObjectPanel>()._placed = false;
                        _targetList.Add(newGeometry);
                        targetPlaced++;
                        newGeometry.transform.SetParent(_canvas.transform, false);
                        //newGeometry.GetComponent<Geometry>()._geometryType = _currentDataDifficulty.possibleGeometry[Random.Range(0, _currentDataDifficulty.possibleGeometry.Count)];
                        _geometryList.Add(newGeometry);
                    }
                   
                  
                }
                else
                {
                     newGeometry = Instantiate(_currentDataDifficulty.allGeometry[geometryID], new Vector3((x + _offsetX) * _gapX, (y + _offsetY) * _gapY, 0), Quaternion.identity);
                    newGeometry.transform.SetParent(_canvas.transform, false);
                    //newGeometry.GetComponent<Geometry>()._geometryType = _currentDataDifficulty.possibleGeometry[Random.Range(0, _currentDataDifficulty.possibleGeometry.Count)];
                    _geometryList.Add(newGeometry);
                }
                
               
               
                 
                    //newGeometry.GetComponent<Geometry>();
                
            }
        }
        //for (int i = 0; i < _numberEmpty; i++)
        //{
        //    _geometryList[i].GetComponent<ObjectPanel>()._placed=false;
        //}
        //for (int i = 0; i < _currentDataDifficulty.numTargets; i++)
        //{
        //    _geometryList[i].GetComponent<ObjectPanel>()._placed = false;

        //    _geometryList[i].GetComponent<Geometry>()._geometryType = _currentDataDifficulty.targetsGeometry[Random.Range(0, _currentDataDifficulty.targetsGeometry.Count)];
        //}
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
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
        //for (int i = 0; i < _geometryList.Count; i++)
        //{
        //    Destroy(_geometryList[i]);
        //    //_geometryList.RemoveAt(i);
        //}
        _geometryList.Clear();

        //SetTarget();
        //SetNumberEmpty();
        GeneratePanel();
     }
}




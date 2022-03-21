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
    
    //Game Configuration
    [SerializeField]
    private int _numberEmpty;
    [SerializeField]
    private int _level;
    [SerializeField]
    Dictionary<Geometry.Geometry_Type, GameObject> _geometryDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    public List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();
    void Start()
    {
        SetTarget();
        SetNumberEmpty();
        GeneratePanel();
    }
    void SetTarget()
    {
        _geometryDic.Clear();
        _targetList.Clear();

        foreach (GameObject geometry in _geometryForms)
        {
            _geometryDic.Add(geometry.GetComponent<Geometry>()._geometryType, geometry);
        }
        List<Geometry.Geometry_Type> keyList = new List<Geometry.Geometry_Type>(this._geometryDic.Keys);
        _targetList = GetComponent<TargetSelector>().generateTargets(keyList, _level);
    }
    void SetNumberEmpty()
    {
        _numberEmpty = 3 + (_level / 2);
        if (_numberEmpty > 8)
            _numberEmpty = 8;
    }


  

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
                    int geometryID = Random.Range(0, maxValue);
                    if (geometryID >= 7)
                    {
                        geometryID = 6;
                    }
                    GameObject newGeometry = Instantiate(_geometryForms[geometryID], new Vector3((x+_offsetX)*_gapX, (y+_offsetY)*_gapY, 0) , Quaternion.identity);
                  newGeometry.transform.SetParent(_canvas.transform, false);
                _geometryList.Add(newGeometry);
                    //newGeometry.GetComponent<Geometry>();
                }
            }
        for (int i = 0; i < _numberEmpty; i++)
        {
            _geometryList[i].GetComponent<ObjectPanel>()._placed=false;
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

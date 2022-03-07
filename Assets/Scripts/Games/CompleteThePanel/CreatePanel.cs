using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePanel : MonoBehaviour
{
    public int _column;
    public int _row;
    public int _gap;
 
    public GameObject _geometry;
    public GameObject[] _geometryForms;
    public List<GameObject> _geometryList= new List<GameObject>();
    //Game Configuration
    [SerializeField]
    private int _numberAsteroids;
    //[SerializeField]
    public int _level;
    [SerializeField]
    Dictionary<Geometry.Geometry_Type, GameObject> _asteroidsDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    public List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();
    void Start()
    {
        setTarget();
        setNumberAsteroids();
        GenerateAsteroids();
    }
    void setTarget()
    {
        _asteroidsDic.Clear();
        _targetList.Clear();

        foreach (GameObject asteroid in _geometryForms)
        {
            _asteroidsDic.Add(asteroid.GetComponent<Geometry>()._geometryType, asteroid);
        }
        List<Geometry.Geometry_Type> keyList = new List<Geometry.Geometry_Type>(this._asteroidsDic.Keys);
        _targetList = GetComponent<TargetSelector>().generateTargets(keyList, _level);
    }
    void setNumberAsteroids()
    {
        _numberAsteroids = 3 + (_level / 2);
        if (_numberAsteroids > 8)
            _numberAsteroids = 8;
    }
    void GenerateAsteroids()
    {
        
            //deleteAsteroids();
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
                    GameObject newGeometry = Instantiate(_geometryForms[geometryID], new Vector3(x, y, 0) * _gap, Quaternion.identity);
                    _geometryList.Add(newGeometry);
                    //newGeometry.GetComponent<Geometry>();
                }
            }
        for (int i = 0; i < _numberAsteroids; i++)
        {
            _geometryList[i].GetComponent<ObjectPanel>()._placed=false;
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

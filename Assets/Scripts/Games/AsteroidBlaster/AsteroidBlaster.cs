using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBlaster : MonoBehaviour
{
    //Geometry Forms
    [Header("Geometry forms")]
    public GameObject[] _geometryForms;

    //Game Configuration
    [SerializeField]
    private int _level;
    [SerializeField]
    private float _movementVelocity;
    [SerializeField]
    private float _rotationVelocity;
    [SerializeField]
    private int _numberAsteroids;
    [SerializeField]
    private List<GameObject> _asteroids = new List<GameObject>();
    [SerializeField]
    Dictionary<Geometry.Geometry_Type, GameObject> _asteroidsDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            restartGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        setTarget();
        setNumberAsteroids();
        GenerateAsteroids();
    }

    /*
     * @desc Set the max number of asteroids
     * **/
    void setNumberAsteroids()
    {
        _numberAsteroids = 3 + (_level / 2);
        if (_numberAsteroids > 8)
            _numberAsteroids = 8;
    }

    /*
     * @desc Obtains a dictionary with asteroids and their shapes and
     * calls the function generateTargets
     * **/
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

    /*
     * @desc Generate the asteroids and start the value of the properties
     * **/
    void GenerateAsteroids()
    {
        do
        {
            deleteAsteroids();
            int maxValue = _level + 1;
            if (_level > _geometryForms.Length)
                maxValue = _geometryForms.Length - 1;
            for (int i = 0; i < _numberAsteroids; i++)
            {
                int geometryID = Random.Range(0, maxValue);
                GameObject newAsteroid = Instantiate(_geometryForms[geometryID]);
                //newAsteroid.SetActive(false);
                newAsteroid.GetComponent<Asteroid>().InitAsteroid(_movementVelocity, _rotationVelocity, _level);
                _asteroids.Add(newAsteroid);
            }
        } while (!checkGenerateAteroids());
    }

    /*
     * @desc Checks if the arteroids generated are correct
     * @return bool - true if is correct, false if not is correct
     * **/
    bool checkGenerateAteroids()
    {
        bool result = true;
        Dictionary<Geometry.Geometry_Type, bool> goodCreation = new Dictionary<Geometry.Geometry_Type, bool>();

        foreach(Geometry.Geometry_Type targetGeometry in _targetList)
        {
            goodCreation.Add(targetGeometry, false);
        }

        foreach(GameObject asteroid in _asteroids)
        {
            foreach(Geometry.Geometry_Type geometry in _targetList)
            {
                if (asteroid.GetComponent<Geometry>()._geometryType == geometry)
                    goodCreation[geometry] = true;
            }
        }
        foreach(KeyValuePair<Geometry.Geometry_Type, bool> element in goodCreation)
        {
            if (!element.Value)
                result = false;
        }
        return result;
    }

    /*
     * @desc Start the procces to restart the game
     * **/
    void restartGame()
    {
        setTarget();
        setNumberAsteroids();
        GenerateAsteroids();
    }

    /*
     *@desc Destroy all the asteroids 
     * **/
    void deleteAsteroids()
    {
        foreach (GameObject asteroid in _asteroids)
            Destroy(asteroid);
        _asteroids.Clear();
    }
}

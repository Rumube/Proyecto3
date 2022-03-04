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
    private List<GameObject> _asteroids = new List<GameObject>();
    Dictionary<Geometry.Geometry_Type, GameObject> _asteroidsDic = new Dictionary<Geometry.Geometry_Type, GameObject>();
    [SerializeField]
    List<Geometry.Geometry_Type> _targetList = new List<Geometry.Geometry_Type>();


    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject asteroid in _geometryForms)
        {
            _asteroidsDic.Add(asteroid.GetComponent<Geometry>()._geometryType, asteroid);
        }
        List<Geometry.Geometry_Type> keyList = new List<Geometry.Geometry_Type>(this._asteroidsDic.Keys);
        _targetList = GetComponent<TargetSelector>().generateTargets(keyList, _level);
        StartCoroutine(GenerateAsteroids());
    }

    /*
     * @desc Generate the asteroids and start the value of the properties
     * **/
    IEnumerator GenerateAsteroids()
    {
        for(int i = 0; i < _numberAsteroids; i++)
        {
            int geometryID = Random.Range(0, _geometryForms.Length);
            GameObject newAsteroid = Instantiate(_geometryForms[geometryID]);
            newAsteroid.GetComponent<Asteroid>().InitAsteroid(_movementVelocity, _rotationVelocity, _level);
            _asteroids.Add(newAsteroid);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

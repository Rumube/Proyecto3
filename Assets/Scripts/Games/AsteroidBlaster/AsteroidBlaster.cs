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
    // Start is called before the first frame update
    void Start()
    {
        GenerateAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * @desc Generate the asteroids and start the value of the properties
     * **/
    void GenerateAsteroids()
    {
        
        for(int i = 0; i < _numberAsteroids; i++)
        {
            int geometryID = Random.Range(0, _geometryForms.Length);
            GameObject newAsteroid = Instantiate(_geometryForms[geometryID]);
            newAsteroid.GetComponent<Asteroid>().InitAsteroid(geometryID, _movementVelocity, _rotationVelocity, _level);
            _asteroids.Add(newAsteroid);
        }
    }
}

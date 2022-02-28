using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBlaster : MonoBehaviour
{
    //Geometry Forms
    [Header("Geometry forms")]
    public GameObject[] _geometryForms;

    //Game Configuration
    private int _level;
    private float _movementVelocity;
    private float _rotationVelocity;
    private int _numberAsteroids;
    private GameObject[] _asteroids;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateAsteroids()
    {
        for(int i = 0; i >= _numberAsteroids; i++)
        {
            //GameObject newAsteroid = Instantiate()
        }
    }
}

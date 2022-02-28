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
    private int _levelValuesRange;
    // Start is called before the first frame update
    void Start()
    {
        
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
        int geometryID = Random.Range(0, _geometryForms.Length);
        for(int i = 0; i >= _numberAsteroids; i++)
        {
            GameObject newAsteroid = Instantiate(_geometryForms[geometryID]);
            Asteroid asteroidScript = newAsteroid.GetComponent<Asteroid>();
            asteroidScript._movementVelocity = SetMovementVelocity();
            asteroidScript._movementVelocity = SetRotationVelocity();
            asteroidScript._idGeometry = geometryID;

        }
    }

    /*
     * @desc Generates the speed of movement depending on the difficulty
     * @return float - The speed value
     * **/
    float SetMovementVelocity()
    {
        return Random.Range(_movementVelocity - _levelValuesRange,
            _movementVelocity + _levelValuesRange);
    } 
    
    /*
     * @desc Generates the speed of rotation depending on the difficulty
     * @return float - The speed value
     * **/
    float SetRotationVelocity()
    {
        return Random.Range(_rotationVelocity - _levelValuesRange,
            _rotationVelocity + _levelValuesRange);
    }
}

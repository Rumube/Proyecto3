using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[Header("Configuration")]
    //Configuration
    [HideInInspector]
    public float _movementVelocity {get; set; }
    [HideInInspector]
    public float _rotationVelocity { get; set; }
    [HideInInspector]
    public int _idGeometry { get; set; }
    [SerializeField]
    private bool _rotating = false;
    [SerializeField]
    private Vector2 _startPostion;
    [SerializeField]
    private Vector2 _targetPosition;

    // Update is called once per frame
    void Update()
    {
        if(ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing){
            MoveAsteroid();
            if(_rotating)
                RotationAsteroid();
        }
    }

    /*
     * @desc Starts execution of asteroid initiation methods
     * @param int geometryID - The id that assigns the geometry type
     * @param float movementVelocity - The speed value
     * @param float rotationVelocity - The speed value
     * @param float levelValuesRange - Randomization parameter
     * **/
    public void InitAsteroid(int geometryID ,float movementVelocity,float rotationVelocity, float levelValuesRange)
    {
        SetMovementVelocity(movementVelocity, levelValuesRange);
        SetRotationVelocity(rotationVelocity, levelValuesRange);
        SetInitialPosition();
        _idGeometry = geometryID;
    }

    /*
     * @desc Move the asteroid in a direction and velocity
     * **/
    void MoveAsteroid()
    {
        float velocity = _movementVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, velocity);
        if ((Vector2)transform.position == _targetPosition)
            RestartPosition();

    }

    /*
     * @desc Rotate the asteroid in Z axis
     * **/
    void RotationAsteroid()
    {
        float velocity = _rotationVelocity * Time.deltaTime;
        transform.Rotate(transform.rotation.x, transform.rotation.y, velocity);
    }

    /*
     * @desc Restart the position
     * **/
    void RestartPosition()
    {
        transform.position = _startPostion;
    }

    private void OnBecameInvisible()
    {
        RestartPosition();
    }

    /*
     * @desc Generates the speed of movement depending on the difficulty
     * @param float movementVelocity - The speed value
     * @param float levelValuesRange - Randomization parameter
     * **/
    public void SetMovementVelocity(float movementVelocity, float levelValuesRange)
    {
        _movementVelocity = Random.Range(movementVelocity,
            (1 + levelValuesRange));
    }

    /*
     * @desc Generates the speed of rotation depending on the difficulty
     * @param float rotationVelocity - The speed value
     * @param float levelValuesRange - Randomization parameter
     * **/
    public void SetRotationVelocity(float rotationVelocity, float levelValuesRange)
    {
        _rotationVelocity = Random.Range(rotationVelocity,
            rotationVelocity + (45 + levelValuesRange));
    }

    /*
     * @desc Generates the initial and the target position
     * **/
    public void SetInitialPosition()
    {
        int initialPosition = Random.Range(0, 4);
        switch (initialPosition)
        {
            case 0: //Top Y = 6
                SetTopPosition();
                break;
            case 1: //Bottom Y = -6
                SetBottomPosition();
                break;
            case 2: //Left X = -10
                SetLeftPosition();
                break;
            case 3: //Right X = 10
                SetRightPosition();
                break;
        }
        transform.position = _startPostion;
    }
    /*
     * @desc Generates the initial top and the target position
     * **/
    void SetTopPosition()
    {
        int xPos = Random.Range(-7, 8);
        int xPosTarget = Random.Range(-7, 8);
        _startPostion = new Vector2(xPos, 7);
        _targetPosition = new Vector2(xPosTarget, -7);
    }
    /*
     * @desc Generates the initial bottom and the target position
     * **/
    void SetBottomPosition()
    {
        int xPos = Random.Range(-7, 8);
        int xPosTarget = Random.Range(-7, 8);
        _startPostion = new Vector2(xPos, -7);
        _targetPosition = new Vector2(xPosTarget, 7);
    }
    /*
     * @desc Generates the initial left and the target position
     * **/
    void SetLeftPosition()
    {
        int yPos = Random.Range(-3, 4);
        int yPosTarget = Random.Range(-3, 4);
        _startPostion = new Vector2(-10, yPos);
        _targetPosition = new Vector2(10, yPosTarget);
    }
    /*
     * @desc Generates the initial right and the target position
     * **/
    void SetRightPosition()
    {
        int yPos = Random.Range(-3, 4);
        int yPosTarget = Random.Range(-3, 4);
        _startPostion = new Vector2(10, yPos);
        _targetPosition = new Vector2(-10, yPosTarget);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Configuration
    public float _movementVelocity {get; set; }
    public float _rotationVelocity { get; set; }
    [Header("Configuration")]
    [SerializeField]
    private bool _rotating = false;
    [SerializeField]
    private Vector2 _startPostion;
    [SerializeField]
    private Vector2 _targetPosition;
    public GameObject _destroyGO;
    public GameObject _GO;

    public enum Asteroid_State
    {
        movement = 0,
        exploding = 1,
        destroyed = 2
    }

    public Asteroid_State state = Asteroid_State.movement;
    // Update is called once per frame
    void Update()
    {
        if(ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing && state == Asteroid_State.movement){
            MoveAsteroid();
            if(_rotating)
                RotationAsteroid();
        }
    }

    /// <summary>
    /// Starts execution of asteroid initiation methods.
    /// </summary>
    /// <param name="movementVelocity">The id that assigns the geometry type</param>
    /// <param name="rotationVelocity">The speed value</param>
    /// <param name="levelValuesRange">Randomization parameter</param>
    public void InitAsteroid(float movementVelocity,float rotationVelocity, float levelValuesRange)
    {
        SetMovementVelocity(movementVelocity, levelValuesRange);
        SetRotationVelocity(rotationVelocity, levelValuesRange);
        SetInitialPosition();
    }

    /// <summary>
    /// Move the asteroid in a direction and velocity.
    /// </summary>
    void MoveAsteroid()
    {
        float velocity = _movementVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, velocity);
        if ((Vector2)transform.position == _targetPosition)
            RestartPosition();

    }

    /// <summary>
    /// Rotate the asteroid in Z axis.
    /// </summary>
    void RotationAsteroid()
    {
        float velocity = _rotationVelocity * Time.deltaTime;
        transform.Rotate(transform.rotation.x, transform.rotation.y, velocity);
    }

    /// <summary>
    /// Restart the position.
    /// </summary>
    void RestartPosition()
    {
        transform.position = _startPostion;
    }

    private void OnBecameInvisible()
    {
        RestartPosition();
    }


    /// <summary>
    /// Generates the speed of movement 
    /// depending on the difficulty.
    /// </summary>
    /// <param name="movementVelocity">The speed value.</param>
    /// <param name="levelValuesRange">Randomization parameter,</param>
    public void SetMovementVelocity(float movementVelocity, float levelValuesRange)
    {
        _movementVelocity = Random.Range(movementVelocity,
            (1 + levelValuesRange / 2));
    }

    /// <summary>
    /// Generates the speed of rotation
    /// depending on the difficulty.
    /// </summary>
    /// <param name="rotationVelocity">The speed value.</param>
    /// <param name="levelValuesRange">Randomization parameter.</param>
    public void SetRotationVelocity(float rotationVelocity, float levelValuesRange)
    {
        _rotationVelocity = Random.Range(rotationVelocity,
            rotationVelocity + (5 + levelValuesRange));
    }


    /// <summary>
    /// Generates the initial and the target position.
    /// </summary>
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

    /// <summary>
    /// Generates the initial top and the target position.
    /// </summary>
    void SetTopPosition()
    {
        int xPos = Random.Range(-7, 8);
        int xPosTarget = Random.Range(-7, 8);
        _startPostion = new Vector2(xPos, 7);
        _targetPosition = new Vector2(xPosTarget, -7);
    }

    /// <summary>
    /// Generates the initial bottom and the target position.
    /// </summary>
    void SetBottomPosition()
    {
        int xPos = Random.Range(-7, 8);
        int xPosTarget = Random.Range(-7, 8);
        _startPostion = new Vector2(xPos, -7);
        _targetPosition = new Vector2(xPosTarget, 7);
    }

    /// <summary>
    /// Generates the initial left and the target position.
    /// </summary>
    void SetLeftPosition()
    {
        int yPos = Random.Range(-3, 4);
        int yPosTarget = Random.Range(-3, 4);
        _startPostion = new Vector2(-10, yPos);
        _targetPosition = new Vector2(10, yPosTarget);
    }

    /// <summary>
    /// Generates the initial right and the target position.
    /// </summary>
    void SetRightPosition()
    {
        int yPos = Random.Range(-3, 4);
        int yPosTarget = Random.Range(-3, 4);
        _startPostion = new Vector2(10, yPos);
        _targetPosition = new Vector2(-10, yPosTarget);
    }

    /// <summary>
    /// Change asteroid status to Exploding.
    /// </summary>
    public void AsteroidShot()
    {
        state = Asteroid_State.exploding;
    }

}

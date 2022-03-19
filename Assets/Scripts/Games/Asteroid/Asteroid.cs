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
    private Vector2 _direction;
    public GameObject _destroyGO;
    public GameObject _asteroidGO;
    //References
    GameObject _gm;
    Rigidbody2D _rb;

    private int _initialPosValue;
    public enum Asteroid_State
    {
        movement = 0,
        exploding = 1,
        destroyed = 2
    }

    public Asteroid_State state = Asteroid_State.movement;

    private void FixedUpdate()
    {
        if (ServiceLocator.Instance.GetService<GameManager>()._gameStateClient == GameManager.GAME_STATE_CLIENT.playing && state == Asteroid_State.movement)
        {
            MoveAsteroid();
            if (_rotating)
                RotationAsteroid();
        }
    }

    /// <summary>
    /// Starts execution of asteroid initiation methods.
    /// </summary>
    /// <param name="movementVelocity">The id that assigns the geometry type</param>
    /// <param name="rotationVelocity">The speed value</param>
    /// <param name="levelValuesRange">Randomization parameter</param>
    /// <param name="gameManager">Reference to the GameManager</param>
    /// <example><code>InitAsteroid(newMovementVelocity, newRotationVelocity, newLevel)</code></example>
    public void InitAsteroid(float movementVelocity,float rotationVelocity, float levelValuesRange, GameObject gameManager)
    {
        _asteroidGO.SetActive(true);
        _destroyGO.SetActive(false);
        SetMovementVelocity(movementVelocity, levelValuesRange);
        SetRotationVelocity(rotationVelocity, levelValuesRange);
        SetInitialPosition();
        _gm = gameManager;
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Move the asteroid in a direction and velocity.
    /// </summary>
    void MoveAsteroid()
    {
        _rb.velocity = _direction;
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
        GenerateNewTarget(_initialPosValue);
    }

    /// <summary>
    /// Generates the speed of movement 
    /// depending on the difficulty.
    /// </summary>
    /// <param name="movementVelocity">The speed value.</param>
    /// <param name="levelValuesRange">Randomization parameter.</param>
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
        _initialPosValue = Random.Range(0, 4);
        switch (_initialPosValue)
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
        RestartPosition();
    }

    /// <summary>
    /// Generates the initial top position.
    /// </summary>
    void SetTopPosition()
    {
        int xPos = Random.Range(-6, 7);
        _startPostion = new Vector2(xPos, 7);
    }

    /// <summary>
    /// Generates the initial bottom position.
    /// </summary>
    void SetBottomPosition()
    {
        int xPos = Random.Range(-6, 7);
        _startPostion = new Vector2(xPos, -7);
    }

    /// <summary>
    /// Generates the initial left position.
    /// </summary>
    void SetLeftPosition()
    {
        int yPos = Random.Range(-2, 3);
        _startPostion = new Vector2(-10, yPos);
    }

    /// <summary>
    /// Generates the initial right and the target position.
    /// </summary>
    void SetRightPosition()
    {
        int yPos = Random.Range(-2, 3);
        _startPostion = new Vector2(10, yPos);
    }

    /// <summary>
    /// Change asteroid status to Exploding.
    /// </summary>
    public void AsteroidShot()
    {
        state = Asteroid_State.exploding;
        _destroyGO.SetActive(true);
        _asteroidGO.SetActive(false);
        _destroyGO.GetComponent<Animator>().SetTrigger("Broke");
        _gm.GetComponent<AsteroidBlaster>().CheckIfIsCorrect(gameObject);
        StartCoroutine(FinishExploding());
    }

    /// <summary>
    /// Waits until animations end and destroy the GameObject
    /// </summary>
    IEnumerator FinishExploding()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Set a new target
    /// </summary>
    /// <param name="initialPos">0: Top - 1: Bottom - 2: Left - 3: Right - 4:Bouncing</param>
    private void GenerateNewTarget(int initialPos)
    {
        switch (initialPos)
        {
            case 0:
                _direction = new Vector2(Random.Range(-0.5f,0.5f), -1);
                break;
            case 1:
                _direction = new Vector2(Random.Range(-0.5f, 0.5f), 1);
                break;
            case 2:
                _direction = new Vector2(1, Random.Range(-0.5f, 0.5f));
                break;
            case 3:
                _direction = new Vector2(-1, Random.Range(-0.5f, 0.5f));
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            _direction = Vector3.Normalize(Vector3.Reflect(collision.relativeVelocity * -1, collision.contacts[0].normal));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "SpaceGame")
            RestartPosition();
    }

}

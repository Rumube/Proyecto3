using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float _movementVelocity { get; set; }
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
    public GameObject _collisionAsteroid;
    private bool _canRespawn;
    [Header("References")]
    GameObject _gm;
    Rigidbody2D _rb;
    Collider2D _collider;

    private int _initialPosValue;
    public enum Asteroid_State
    {
        movement = 0,
        exploding = 1,
        destroyed = 2
    }

    public Asteroid_State _state = Asteroid_State.movement;

    private void FixedUpdate()
    {
        if (ServiceLocator.Instance.GetService<GMSinBucle>()._gameStateClient == GMSinBucle.GAME_STATE_CLIENT.playing)
        {
            _collider.enabled = true;
            MoveAsteroid();
            if (_rotating)
                RotationAsteroid();
        }
        else
        {
            _collider.enabled = false;
            _rb.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Starts execution of asteroid initiation methods.
    /// Game: Geometri Cabin
    /// </summary>
    /// <param name="movementVelocity">The id that assigns the geometry type</param>
    /// <param name="rotationVelocity">The speed value</param>
    /// <param name="gameManager">Reference to the GameManager</param>
    /// <example><code>InitAsteroid(newMovementVelocity, newRotationVelocity, newLevel)</code></example>
    public void InitAsteroid(float movementVelocity, float rotationVelocity, GameObject gameManager)
    {
        _asteroidGO.SetActive(true);
        _destroyGO.SetActive(false);
        _movementVelocity = movementVelocity;
        _rotationVelocity = rotationVelocity;
        SetInitialPosition();
        RestartPosition();
        _gm = gameManager;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _canRespawn = true;
    }
    /// <summary>
    /// Starts execution of asteroid initiation methods.
    /// Game: Space Time Cabin
    /// </summary>
    /// <param name="movementVelocity"></param>
    /// <param name="center"></param>
    /// <param name="gameManager"></param>
    public void InitAsteroid(float movementVelocity, Vector2 center, GameObject gameManager)
    {
        _asteroidGO.SetActive(true);
        _destroyGO.SetActive(false);
        _movementVelocity = movementVelocity;
        _gm = gameManager;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        SetInitialPosition();
        transform.position = _startPostion;
        SetCenterPoint(center);
        _canRespawn = false;
        _rotating = true;
        _rotationVelocity = 10f;
        int layerAsteroidNoCollision = LayerMask.NameToLayer("AsteroidsNoCollision");
        gameObject.layer = layerAsteroidNoCollision;
    }

    /// <summary>
    /// Move the asteroid in a direction and velocity.
    /// </summary>
    void MoveAsteroid()
    {
        _rb.velocity = _direction * Time.fixedDeltaTime * 50.0f;
    }

    /// <summary>
    /// Rotate the asteroid in Z axis.
    /// </summary>
    void RotationAsteroid()
    {
        float velocity = _rotationVelocity * Time.fixedDeltaTime;
        transform.Rotate(transform.rotation.x, transform.rotation.y, velocity);
    }

    /// <summary>
    /// Generate a normalized vector2 to <see cref="_direction"/>
    /// </summary>
    /// <param name="center">Point where the asteroid has to pass through </param>
    void SetCenterPoint(Vector2 center)
    {
        _direction = (center - _startPostion).normalized;
    }

    /// <summary>
    /// Restart the position.
    /// </summary>
    void RestartPosition()
    {
        transform.position = _startPostion;
        GenerateNewTarget();
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
            case 1: //Bottom Y = -5
                SetBottomPosition();
                break;
            case 2: //Left X = -7
                SetLeftPosition();
                break;
            case 3: //Right X = 7
                SetRightPosition();
                break;
        }
    }

    /// <summary>
    /// Generates the initial top position.
    /// </summary>
    void SetTopPosition()
    {
        int xPos = Random.Range(-4, 4);
        _startPostion = new Vector2(xPos, 6);
    }

    /// <summary>
    /// Generates the initial bottom position.
    /// </summary>
    void SetBottomPosition()
    {
        int xPos = Random.Range(-4, 4);
        _startPostion = new Vector2(xPos, -5);
    }

    /// <summary>
    /// Generates the initial left position.
    /// </summary>
    void SetLeftPosition()
    {
        int yPos = Random.Range(-2, 3);
        _startPostion = new Vector2(-7, yPos);
    }

    /// <summary>
    /// Generates the initial right and the target position.
    /// </summary>
    void SetRightPosition()
    {
        int yPos = Random.Range(-2, 3);
        _startPostion = new Vector2(7, yPos);
    }

    /// <summary>
    /// Change asteroid status to Exploding.
    /// </summary>
    public void AsteroidShot()
    {
        _state = Asteroid_State.exploding;
        _destroyGO.SetActive(true);
        _asteroidGO.SetActive(false);
        _destroyGO.GetComponent<Animator>().SetTrigger("Broke");
        if (_gm.GetComponent<AsteroidBlaster>())
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
    public void GenerateNewTarget()
    {
        int isNegative = Random.Range(0, 2);
        switch (_initialPosValue)
        {
            case 0:
                _direction = new Vector2(Random.Range(0.4f, 0.8f), -1);
                if (isNegative == 0)
                    _direction.x *= -1;
                break;
            case 1:
                _direction = new Vector2(Random.Range(0.4f, 0.8f), 1);
                if (isNegative == 0)
                    _direction.x *= -1;
                break;
            case 2:
                _direction = new Vector2(1, Random.Range(0.4f, 0.8f));
                if (isNegative == 0)
                    _direction.y *= -1;
                break;
            case 3:
                _direction = new Vector2(-1, Random.Range(0.4f, 0.8f));
                if (isNegative == 0)
                    _direction.y *= -1;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canRespawn && collision.name == "SpaceGame")
            RestartPosition();
        else if (!_canRespawn && collision.name == "SpaceGame" && _state == Asteroid_State.movement)
        {
            _gm.GetComponent<SpaceTimeCabin>().LoseAsteroid();
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Creates the animation to the collisions.
    /// </summary>
    /// <param name="collision">Collision data</param>
    void CollisionController(Collision2D collision)
    {
        GameObject newCollision = Instantiate(_collisionAsteroid);
        newCollision.transform.position = collision.contacts[0].point;
        newCollision.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        newCollision.GetComponent<Animator>().Play("BrokenAsteroids_Animation");
        Destroy(newCollision, 1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            _direction = Vector3.Normalize(Vector3.Reflect(collision.relativeVelocity * -1, collision.contacts[0].normal));
            if ((_direction.x < 0.1f && _direction.x > -0.1f) || (_direction.y < 0.1f && _direction.y > -0.1f))
                GenerateNewTarget();
            _rb.AddForce(_direction * 1.5f, ForceMode2D.Impulse);
            CollisionController(collision);
        }
    }
}

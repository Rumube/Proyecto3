using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFeedback : MonoBehaviour
{
    //Configuration
    [SerializeField]
    public enum ParticleState
    {
        floating,
        movement
    }
    public ParticleState _state;
    public Vector2 _target;
    private Vector2 _direction;
    private float _finishFloating;
    private float _floatingVelocity;
    private float _movementVelocity;
    private Positive _positiveScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup >= _finishFloating && _state == ParticleState.floating)
            _state = ParticleState.movement;
        switch (_state)
        {
            case ParticleState.floating:
                FloatingMovement();
                break;
            case ParticleState.movement:
                MoveToTarget();
                break;
            default:
                _state = ParticleState.movement;
                break;
        }
    }

    /// <summary>
    /// Defines the start velues of the particle.
    /// </summary>
    /// <param name="direction">Direction in which it is fired</param>
    /// <param name="target">Point to move</param>
    /// <param name="timeFloating">Time the particle is floating</param>
    /// <param name="velocityFloating">Velocity of the particle in floating state</param>
    /// <param name="movementFloating">Velocity of the particle in movement state</param>
    public void SetStartValues(Vector2 direction, Vector2 target, float timeFloating, float velocityFloating, float movementFloating, Positive feedback)
    {
        _direction = direction;
        _target = target;
        _finishFloating = Time.realtimeSinceStartup + timeFloating;
        _floatingVelocity = velocityFloating;
        _movementVelocity = movementFloating;
        _state = ParticleState.floating;
        _positiveScript = feedback;
    }

    /// <summary>
    /// Generate the movement in floating state.
    /// </summary>
    private void FloatingMovement()
    {
        transform.position += (Vector3)_direction * _floatingVelocity * Time.deltaTime;
    }

    /// <summary>
    /// Generate the movement in movement state.
    /// </summary>
    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, _movementVelocity * Time.deltaTime);
        if (Vector2.Distance(_target, transform.position) < 0.5f)
        {
            //TODO: SONIDO?
            _positiveScript.AddPoints();
            Destroy(gameObject);
        }
    }
}

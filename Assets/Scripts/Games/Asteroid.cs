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

    private Vector3 _targetStartPosition = new Vector3(0, 0, 0);
    [SerializeField]
    private bool _rotating = false;
    [SerializeField]
    private Vector2 _startPostion;
    [SerializeField]
    private Transform _target;


    private void Start()
    {
        _startPostion = transform.position;
        _targetStartPosition = _target.position;
    }

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
     * @desc Move the asteroid in a direction and velocity
     * **/
    void MoveAsteroid()
    {
        _target.position = _targetStartPosition;
        float velocity = _movementVelocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, velocity);
        if (transform.position == _target.position)
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
}

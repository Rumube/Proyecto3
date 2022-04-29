using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positive : MonoBehaviour, IPositive
{
    [Header("Configuration")]
    public int _numberOfParticles;
    public float _floatingTime;
    public float _floatingVelocity;
    public float _particleVelocity;
    public int _pointsPerParticle;
    private int _currentPoints;
    private int _totalPoints;
    [Header("References")]
    public GameObject _goodParticle;
    public Vector2 _targetFeedback = new Vector2(6, 4);
    public AudioClip _clip;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateFeedback(new Vector2(0,0));
        if (_currentPoints > _totalPoints)
            _currentPoints++;
    }

    /// <summary>
    /// Start the feedback actions
    /// </summary>
    /// <param name="initPosition">Position to create the feedback</param>
    public void GenerateFeedback(Vector2 initPosition)
    {
        GenerateVisualFeedback(initPosition);
        AudioManagement();
    }

    /// <summary>
    /// Set the audio clip and play the sound
    /// </summary>
    private void AudioManagement()
    {
        _audio.clip = _clip;
        _audio.Play();
    }

    /// <summary>
    /// Generate the visual effects of positive feedback
    /// <paramref name="initPosition">Position to create the feedback</paramref>
    /// </summary>
    private void GenerateVisualFeedback(Vector2 initPosition)
    {
        float radialPart = 360 / _numberOfParticles;
        float degrees = Random.Range(0, 360);
        for (int i = 0; i < _numberOfParticles; i++)
        {
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degrees) * Vector2.right);
            GameObject newParticle = Instantiate(_goodParticle);
            newParticle.transform.position = initPosition;
            newParticle.GetComponent<ParticleFeedback>().SetStartValues(dir, _targetFeedback, _floatingTime, _floatingVelocity, _particleVelocity, this);
            degrees += radialPart;
        }
    }

    public void AddPoints()
    {
        _totalPoints += _pointsPerParticle;
    }
}

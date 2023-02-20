using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positive : MonoBehaviour, IPositive
{
    [Header("Configuration")]
    public int _numberOfParticles;
    public float _floatingTime;
    public float _floatingVelocity;
    public float _particleVelocity;
    public int _pointsPerParticle;
    public int _currentPoints;
    public int _totalPoints;
    private float _pitchValue = 1;
    private float _finishCombo;
    public float _particleSize = 1;
    [Header("References")]
    public GameObject _goodParticle;
    public GameObject _scoreEffect;
    public Vector2 _targetFeedback = new Vector2(6, 4);
    public AudioClip _clip;
    private AudioSource _audio;
    private Text _scorePanelText;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _scorePanelText = GameObject.FindGameObjectWithTag("ScorePanel").GetComponent<Text>();
        _currentPoints = 0;
        _totalPoints = 0;
        _scorePanelText.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if(_scorePanelText == null)
        {
            _scorePanelText = GameObject.FindGameObjectWithTag("ScorePanel").GetComponent<Text>();
        }

        if(Time.realtimeSinceStartup >= _finishCombo)
        {
            _pitchValue = 1;
        }

        if (_currentPoints < _totalPoints)
        {
            _currentPoints++;
            _scorePanelText.text = _currentPoints.ToString();
        }
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
        _scorePanelText = GameObject.FindGameObjectWithTag("ScorePanel").GetComponent<Text>();
        float radialPart = 360 / _numberOfParticles;
        float degrees = Random.Range(0, 360);
        for (int i = 0; i < _numberOfParticles; i++)
        {
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degrees) * Vector2.right);
            GameObject newParticle = Instantiate(_goodParticle);
            newParticle.transform.position = initPosition;
            newParticle.GetComponent<ParticleFeedback>().SetStartValues(dir, _targetFeedback, _floatingTime, _floatingVelocity, _particleVelocity, _particleSize, this);
            degrees += radialPart;
        }
    }
    /// <summary>
    /// Add points and activate animations and sounds for the minigame
    /// </summary>
    public void AddPoints()
    {
        _pitchValue += 0.01f;
        _finishCombo = Time.realtimeSinceStartup + 1f;

        GameObject newEffect = Instantiate(_scoreEffect);
        newEffect.GetComponent<ScoreEffect>().InitValues(_pointsPerParticle, _pitchValue);

        GameObject[] scoreEffectSpawners = GameObject.FindGameObjectsWithTag("ScoreEffectSpawner");
        int randomPos = Random.Range(0, scoreEffectSpawners.Length);
        newEffect.transform.SetParent(scoreEffectSpawners[0].transform.parent);
        newEffect.transform.position = scoreEffectSpawners[randomPos].transform.position;
        _totalPoints += _pointsPerParticle;
    }
}

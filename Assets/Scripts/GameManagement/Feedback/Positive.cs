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
    [Header("References")]
    public GameObject _goodParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateFeedback(new Vector2(0,0));
    }

    /// <summary>
    /// Start the feedback actions
    /// </summary>
    /// <param name="initPosition">Position to create the feedback</param>
    public void GenerateFeedback(Vector2 initPosition)
    {
        float radialPart = 360 / _numberOfParticles;
        float degrees = Random.Range(0, 360);
        Vector3 frogPos = GameObject.FindGameObjectWithTag("CountDown").GetComponent<RectTransform>().position;
        frogPos = new Vector3(frogPos.x, frogPos.y, Camera.main.transform.position.z);
        Vector2 target = Camera.main.ScreenToWorldPoint(frogPos);
        target = new Vector2(-target.x, -2f * target.y);
        for (int i = 0; i < _numberOfParticles; i++)
        {
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degrees) * Vector2.right);
            GameObject newParticle = Instantiate(_goodParticle);
            newParticle.transform.position = initPosition;
            newParticle.GetComponent<ParticleFeedback>().SetStartValues(dir, target, _floatingTime, _floatingVelocity, _particleVelocity);
            degrees += radialPart;
        }
    }
}


using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	Transform _camTransform;

	// How long the object should shake for.
	private float _finishShake;

	private float _shakeAmount;

	// Amplitude of the shake. A larger value shakes the camera harder.
	Vector3 originalPos;

	void Awake()
	{
		_camTransform = Camera.main.transform;
	}

	void OnEnable()
	{
		originalPos = _camTransform.localPosition;
	}

	void Update()
	{
		if (Time.realtimeSinceStartup < _finishShake)
		{
			_camTransform.localPosition = originalPos + Random.insideUnitSphere * _shakeAmount;
		}
		else
		{
			_camTransform.localPosition = originalPos;
		}
	}

	public void StartShake(float shakeDuration, float shakeIntensity)
    {
		_finishShake = Time.realtimeSinceStartup + shakeDuration;
		_shakeAmount = shakeIntensity;

	}
}

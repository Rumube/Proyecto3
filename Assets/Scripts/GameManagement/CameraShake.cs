
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
		UpdateShakeController();
	}
	/// <summary>
	/// Checks if the shake time is finish.
	/// If is finished the shake stop
	/// </summary>
	private void UpdateShakeController()
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
	/// <summary>
	/// Change the camera's motion and duration settings and it starts shaking
	/// </summary>
	/// <param name="shakeDuration">The time </param>
	/// <param name="shakeIntensity">The intensity of the shake</param>
	public void StartShake(float shakeDuration, float shakeIntensity)
    {
		_finishShake = Time.realtimeSinceStartup + shakeDuration;
		_shakeAmount = shakeIntensity;

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float _followSpeed = 2f;
	public Transform _target;
	private Transform _camTransform;
	public float _shakeDuration = 0f;
	public float _shakeAmount = 0.1f;
	private Vector3 _originalPos;

	public void Init(Transform target )
	{
        _camTransform = GetComponent<Transform>();

        _target = target;
		_originalPos = _camTransform.localPosition;
	}

	private void FixedUpdate()
	{
		if (_target == null)
			return;

		Vector3 newPosition = _target.position + (Vector3.back * 10);
		transform.position = Vector3.Slerp(transform.position, newPosition, _followSpeed * Time.deltaTime);

		if (_shakeDuration > 0)
		{
			_camTransform.localPosition = _originalPos + (Random.insideUnitSphere * _shakeAmount);

			_shakeDuration -= Time.deltaTime;
		}
	}

	public void ShakeCamera()
	{
		_originalPos = _camTransform.localPosition;
		_shakeDuration = 0.2f;
	}
}

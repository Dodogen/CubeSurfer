using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
	[SerializeField] private Transform playerTransform;

	private float _playerStartY;

	private Vector3 newPosition;
	private Vector3 offset;

	[SerializeField] private float lerpValue;

	private void Start()
	{
		SetOffsetValue();

		_playerStartY = playerTransform.position.y;
	}

	private void LateUpdate()
	{
		if (playerTransform != null)
		{
			SetCameraSmoothFollow();
		}
	}

	private void SetOffsetValue()
	{
		offset = transform.position - playerTransform.position;

	}

	private void SetCameraSmoothFollow()
	{
		newPosition = Vector3.Lerp(transform.position,
			new Vector3(0f, _playerStartY, playerTransform.position.z) + offset, lerpValue * Time.deltaTime);

		transform.position = newPosition;
	}
}

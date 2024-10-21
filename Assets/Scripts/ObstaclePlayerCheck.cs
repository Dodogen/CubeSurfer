using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlayerCheck : MonoBehaviour
{
	[SerializeField] GameObject _player;
	[SerializeField] float _raycastDistance = 0.45f;

	private OnDestroyDispatcher _onDestroyDispatcher;

	private List<Vector3> _directions = new List<Vector3>()
	{
		Vector3.forward,
		//Vector3.left,
		//Vector3.right,
		Vector3.back,
	};

	private RaycastHit _hit;

	private void OnDrawGizmos()
	{
		foreach (var direction in _directions)
		{
			Gizmos.color = Color.red;
			Vector3 startPosition = transform.position;
			Vector3 endPosition = startPosition + direction * _raycastDistance;
			Gizmos.DrawLine(startPosition, endPosition);
			Gizmos.DrawCube(endPosition, new Vector3(0.1f, 0.1f, 0.1f));
		}
	}

	private void Start()
	{
		_onDestroyDispatcher = GetComponent<OnDestroyDispatcher>();
	}

	private void FixedUpdate()
	{
		foreach (var direction in _directions)
		{
			if (Physics.Raycast(transform.position, direction, out _hit, _raycastDistance))
			{
				if (_hit.collider.gameObject == _player)
				{
					//replace
					_onDestroyDispatcher.DestroyObject(_player);
					_onDestroyDispatcher.DestroyObject(gameObject);
				}
			}
		}
	}
}

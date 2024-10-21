using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FourSidesRaycastController : MonoBehaviour
{
	[SerializeField] float _raycastDistance = 0.5f;
	[SerializeField] private StackController _playerStack;

	private Rigidbody _rigidbody;
	private OnDestroyDispatcher _onDestroyDispatcher;

	public bool _isStacked = false; // replace with property
	private RaycastHit _hit;

    private List<Vector3> _directions = new List<Vector3>() {
		Vector3.forward,
		//Vector3.left,
		//Vector3.right,
		Vector3.back,
	};

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

	private void Awake()
	{
		_onDestroyDispatcher = GetComponent<OnDestroyDispatcher>();
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
	}

	void FixedUpdate()
    {
        foreach(var direction in _directions)
		{
			if(Physics.Raycast(transform.position,direction,out _hit, _raycastDistance))
			{
				if (!_isStacked)
				{
					_playerStack.AddCube(gameObject);
					_rigidbody.isKinematic = false;
					_isStacked = true;
				}

				else if (_hit.collider.gameObject.CompareTag("Obstacle"))
				{
					_playerStack.RemoveCube(gameObject);
					_onDestroyDispatcher.DestroyObject(gameObject);
					_onDestroyDispatcher.DestroyObject(_hit.transform.gameObject);
				}
			}
		}
    }
}

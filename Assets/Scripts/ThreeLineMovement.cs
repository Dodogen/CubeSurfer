using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ThreeLineMovement : MonoBehaviour
{
	private enum Lanes
	{
		Left = -1,
		Center = 0,
		Right = 1,
	}

    public float _forwardSpeed;
    public Vector3 _direction = Vector3.forward;

    private Rigidbody _rigidBody;


	private Vector3 startTouchPosition;
	private Vector3 endTouchPosition;

	private Lanes _lane = Lanes.Center;
	private float _laneDistance = 1f;

	private Vector3 _targetPosition;

	void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startTouchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			endTouchPosition = Input.mousePosition;
			HandleSwipe();
		}

		HandleClick();
	}

	private void HandleSwipe()
	{
		float swipeDistance = endTouchPosition.x - startTouchPosition.x;

		if (swipeDistance > 0 && _lane < Lanes.Right)
		{
			MoveToLane(_lane + 1);
		}
		else if (swipeDistance < 0 && _lane > Lanes.Left)
		{
			MoveToLane(_lane - 1);
		}
	}

	private void HandleClick()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow) && _lane < Lanes.Right)
		{
			MoveToLane(_lane + 1);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) && _lane > Lanes.Left)
		{
			MoveToLane(_lane - 1);
		}

	}

	private void MoveToLane(Lanes lane)
	{
		_lane = lane;
		_targetPosition = new Vector3((int)lane * _laneDistance, transform.position.y, transform.position.z);
		_rigidBody.MovePosition(_targetPosition);
	}

	void FixedUpdate()
    {
		_rigidBody.velocity = _direction * _forwardSpeed + new Vector3(0,_rigidBody.velocity.y, 0);
    }
}

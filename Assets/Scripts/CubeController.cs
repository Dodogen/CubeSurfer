using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField] private Color _color;
    private float _speed;
    private Vector3 _direction;
    private Rigidbody _rigidBody;
    private FourSidesRaycastController _controller;

    public Color Color
    {
        get => _color;
        private set => _color = value;
    }


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<FourSidesRaycastController>();

        var x = _player.GetComponent<ThreeLineMovement>();
        if(x != null)
        {
            _speed = x._forwardSpeed;
            _direction = x._direction;
        }
    }

	private void FixedUpdate()
    {
        if (_controller._isStacked)
        {
			Vector3 newPosition = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
			_rigidBody.MovePosition(newPosition);
        }
	}

	public void ApplyMaterial(Color color)
	{
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null)
		{
			this.Color = color;
			renderer.material.color = color;
		}
	}
}

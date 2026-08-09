using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 3.5f;
    public bool useSmoothMovement = true;
    public float smoothTime = 0.08f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _currentVelocity;
    private Vector2 _smoothVelocity;
    private GlobalInputManager _inputManager;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.freezeRotation = true;
        _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        _inputManager = GlobalInputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (_inputManager == null)
            return;

        Vector2 direction = _inputManager.MoveDirection;
        Vector2 targetVelocity = direction * moveSpeed;

        if (useSmoothMovement)
        {
            _currentVelocity = Vector2.SmoothDamp(_currentVelocity, targetVelocity, ref _smoothVelocity, smoothTime, moveSpeed * 5f, Time.fixedDeltaTime);
        }
        else
        {
            _currentVelocity = targetVelocity;
        }

        _rigidbody2D.MovePosition(_rigidbody2D.position + _currentVelocity * Time.fixedDeltaTime);
    }

    public void TeleportTo(Vector2 targetPosition)
    {
        _rigidbody2D.position = targetPosition;
        _rigidbody2D.velocity = Vector2.zero;
        _currentVelocity = Vector2.zero;
    }
}

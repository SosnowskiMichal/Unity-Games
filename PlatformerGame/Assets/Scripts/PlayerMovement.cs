using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float _maxMoveSpeed = 10f;
    [SerializeField] private float _moveForce = 200f;
    [SerializeField] private float _jumpForce = 10f;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private AudioSource _sfxSource;

    private bool _canJump = true;
    private bool _movingRight = true;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private float _horizontalInput;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _canJump = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);

        if (_horizontalInput > 0 && !_movingRight) Flip();
        else if (_horizontalInput < 0 && _movingRight) Flip();

        _animator.SetBool("isRunning", _horizontalInput != 0);
        _animator.SetBool("isJumping", _rigidbody.linearVelocityY > 0);
        _animator.SetBool("isFalling", _rigidbody.linearVelocityY < 0);
        _animator.SetBool("isIdle", _horizontalInput == 0);
    }

    private void FixedUpdate() {
        _rigidbody.AddForceX(_horizontalInput * _moveForce);

        if (Mathf.Abs(_rigidbody.linearVelocityX) > _maxMoveSpeed) {
            _rigidbody.linearVelocityX = Mathf.Clamp(_rigidbody.linearVelocityX, -_maxMoveSpeed, _maxMoveSpeed);
        }

        if (_horizontalInput * _rigidbody.linearVelocityX < 0f) {
            _rigidbody.linearVelocityX = _horizontalInput * _maxMoveSpeed;
        }
    }

    public void Move(InputAction.CallbackContext context) {
        _horizontalInput = context.ReadValue<Vector2>().x;
    }

    private void Flip() {
        _movingRight = !_movingRight;
        transform.Rotate(Vector3.up, 180f);
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && _canJump) {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void PlayStepSound() {
        _sfxSource.PlayOneShot(_sfxSource.clip);
    }

}

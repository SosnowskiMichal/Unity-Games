using UnityEngine;

public class Shuriken : MonoBehaviour {

    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _rotationSpeed = 180f;

    private Rigidbody2D _rigidbody;
    private LevelController _levelController;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction) {
        _rigidbody.linearVelocity = direction.normalized * _speed;
    }

    private void Update() {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    private void Start() {
        _levelController = FindFirstObjectByType<LevelController>();
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Balloon")) {
            collision.GetComponent<Balloon>().PopBalloon(); ;
            Destroy(gameObject);
            _levelController.BalloonPopped();
        }
    }

}
using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour {

    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _maxForce = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start() {
        _spriteRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Length)];
        ApplyInitialForce();
        StartCoroutine(BalloonMovement());
    }

    private void ApplyInitialForce() {
        float force = UnityEngine.Random.Range(0.5f * _maxForce, _maxForce);
        _rigidbody.AddForceX(-force, ForceMode2D.Impulse);
        _rigidbody.AddForceY(0.1f * force, ForceMode2D.Impulse);
    }

    private IEnumerator BalloonMovement() {
        while (true) {
            float randomTime = UnityEngine.Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomTime);

            float force = UnityEngine.Random.Range(-_maxForce, _maxForce);
            _rigidbody.AddForceX(force, ForceMode2D.Impulse);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("BalloonDeathZone")) {
            PopBalloon();
        }
        else if (collision.CompareTag("Wall")) {
            if (collision.transform.position.x > 0) {
                _rigidbody.AddForceX(-0.1f, ForceMode2D.Impulse);
            }
            else {
                _rigidbody.AddForceX(0.1f, ForceMode2D.Impulse);
            }
        }
    }

    public void PopBalloon() {
        _animator.SetTrigger("pop");
    }

    public void DestroyAfterPop() {
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    
    public int PlayerLives { get; private set; } = 3;

    [SerializeField] private LevelController _levelController;

    private Animator _animator;
    private bool _canBeHit = true;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            if (!_canBeHit) return;
            _canBeHit = false;

            if (_animator != null) {
                Debug.Log("Player hit");
                _animator.ResetTrigger("isHit");
                _animator.SetTrigger("isHit");
            }

            PlayerLives--;
            _levelController.LifeLost(PlayerLives);

            if (PlayerLives <= 0) {
                _levelController.AllLivesLost();
            }

            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown() {
        yield return new WaitForSeconds(1);
        _canBeHit = true;
    }

}

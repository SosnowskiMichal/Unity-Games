using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShurikenShooter : MonoBehaviour {

    [SerializeField] private GameObject _shurikenPrefab;
    [SerializeField] private float _shotCooldown = 0.5f;

    private bool _canShoot = true;

    public void Shoot(InputAction.CallbackContext context) {
        if (context.performed && _canShoot) {
            _canShoot = false;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePosition - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion arrowRotation = Quaternion.Euler(0, 0, angle);
            GameObject arrowObj = Instantiate(_shurikenPrefab, transform.position, arrowRotation);

            if (arrowObj.TryGetComponent<Shuriken>(out Shuriken arrow)) {
                arrow.SetDirection(direction);
            }
            StartCoroutine(ShotCooldown());
        }
    }
    
    private IEnumerator ShotCooldown() {
        yield return new WaitForSeconds(_shotCooldown);
        _canShoot = true;
    }

}

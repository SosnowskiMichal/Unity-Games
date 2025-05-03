using System.Collections;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour {

    [SerializeField] private Balloon balloonPrefab;

    private void Start() {
        StartCoroutine(SpawnBalloons());
    }

    private IEnumerator SpawnBalloons() {
        Transform t = transform;
        while (true) {
            Instantiate(balloonPrefab, t.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
    }

}
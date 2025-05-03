using System.Collections;
using UnityEngine;

public class FallingObjectsSpawner : MonoBehaviour {

    [SerializeField] private GameObject _fallingObjectPrefab;
    [SerializeField] private float _spawnOffsetX = 6f;

    private void Start() {
        StartCoroutine(SpawnFallingObjects());
    }

    private IEnumerator SpawnFallingObjects() {
        Transform t = transform;
        while (true) {
            float spawnOffset = Random.Range(-_spawnOffsetX, _spawnOffsetX);
            Vector3 spawnPosition = new Vector3(t.position.x + spawnOffset, t.position.y, t.position.z);

            GameObject spawnedObject = Instantiate(_fallingObjectPrefab, spawnPosition, Quaternion.identity);
            Destroy(spawnedObject, 3f);

            float randomTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(randomTime);
        }
    }

}

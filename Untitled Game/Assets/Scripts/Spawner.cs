using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject player;

    public float scalePercentageMin, scalePercentageMax;

    public GameObject enemy;

    public Vector3 min, max;

    public float spawnTime = 2.0f;

    private float timeSinceSpawn = 0.0f;

    private void Update() {
        while (this.timeSinceSpawn >= this.spawnTime) {
            this.timeSinceSpawn -= this.spawnTime;

            Vector3 position = new Vector3(
                    Random.Range(this.min.x, this.max.x),
                    Random.Range(this.min.y, this.max.y),
                    Random.Range(this.min.z, this.max.z)
                );


            GameObject enemyInstance = Instantiate(this.enemy, position, Quaternion.identity, this.transform);
            Vector3 playerScale = this.player.transform.localScale;
            Vector3 enemyScale = Random.Range(this.scalePercentageMin, this.scalePercentageMax) * playerScale;
            enemyInstance.transform.localScale = enemyScale;
        }

        this.timeSinceSpawn += Time.deltaTime;
    }
}

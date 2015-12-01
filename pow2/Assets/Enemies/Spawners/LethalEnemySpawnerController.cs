using UnityEngine;
using System.Collections;

public class LethalEnemySpawnerController : MonoBehaviour {

    public int totalEnemiesToSpawn;
    public float secondsUntilNextEnemy;
    public string enemyResource;
    private float lastSpawnTimeSeconds;

	// Use this for initialization
	void Start () {
        lastSpawnTimeSeconds  = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time - lastSpawnTimeSeconds >= secondsUntilNextEnemy && totalEnemiesToSpawn > 0) {
            Spawn();
            lastSpawnTimeSeconds = Time.time;
            totalEnemiesToSpawn--;
        }
	}

    void Spawn() {
        Instantiate(Resources.Load(enemyResource), transform.position, Quaternion.identity);
    }
}

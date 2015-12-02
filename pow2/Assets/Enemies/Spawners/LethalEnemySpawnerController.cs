using UnityEngine;
using System.Collections;

public class LethalEnemySpawnerController : MonoBehaviour {

    public TextMesh counterText;
    public int totalEnemiesToSpawn;
    public float secondsUntilNextEnemy;
    public string enemyResource;
    private float lastSpawnTimeSeconds;

	// Use this for initialization
	void Start () {
        lastSpawnTimeSeconds  = Time.time;
        counterText.text = totalEnemiesToSpawn + "";
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time - lastSpawnTimeSeconds >= secondsUntilNextEnemy && totalEnemiesToSpawn > 0) {
            Spawn();
            lastSpawnTimeSeconds = Time.time;
            totalEnemiesToSpawn--;
            counterText.text = totalEnemiesToSpawn + "";
        }
	}

    void Spawn() {
        Instantiate(Resources.Load(enemyResource), transform.position, Quaternion.identity);
    }
}

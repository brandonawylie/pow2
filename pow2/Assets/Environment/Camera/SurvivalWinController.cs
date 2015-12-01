using UnityEngine;
using System.Collections;

public class SurvivalWinController : MonoBehaviour {

    private GameObject[] spawners;
    private bool hasWon;

	// Use this for initialization
	void Start () {
	    spawners = GameObject.FindGameObjectsWithTag("Spawner");
        hasWon = false;
	}
	
	// Update is called once per frame
	void Update () {
        bool win = true;
	    foreach (GameObject go in spawners) {
            LethalEnemySpawnerController sc = go.GetComponent<LethalEnemySpawnerController>();
            if (sc.totalEnemiesToSpawn <= 0) {
                win = false;
            }
        }
        hasWon = win;

        if (hasWon){
            Win();
        }
    }

    public void Win() {

    }
}

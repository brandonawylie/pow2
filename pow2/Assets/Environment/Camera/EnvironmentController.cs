using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
    public bool dev;
    public bool isSinglePlayer;

    private int p1,p2,p3,p4;

	// Use this for initialization
	void Start () {
	    if (!dev) {
            p1 = 1;//PlayerPrefs.GetInt("p1");
            p2 = PlayerPrefs.GetInt("p2");
            p3 = PlayerPrefs.GetInt("p3");
            p4 = PlayerPrefs.GetInt("p4");

            isSinglePlayer = (p1 + p2 + p3 + p4) == 1;
            if (p1 == 1) {
                SpawnPlayer(1);
            }

            if (p2 == 1) {
                SpawnPlayer(2);
            }

            if (p3 == 1) {
                SpawnPlayer(3);
            }

            if (p4 ==1) {
                SpawnPlayer(4);
            }
        }
	}

    void SpawnPlayer(int playerNum) {
        int sum = p1 + p2 + p3 + p4;
        int totalWidth = sum * (50 + 10);
        float offset = totalWidth + (sum / 2 - (playerNum + 1)) *  (totalWidth / sum);
        GameObject p = (GameObject)GameObject.Instantiate(Resources.Load("Player"), Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2 + offset, Screen.height/2, Camera.main.nearClipPlane) ), Quaternion.identity);
        p.GetComponent<PlayerController>().playerNumber = playerNum;
        GameObject i = (GameObject)GameObject.Instantiate(Resources.Load("PlayerIndicator"));
        i.GetComponent<PlayerIndicatorController>().playerController = p.GetComponent<PlayerController>();
        p.GetComponent<PlayerController>().indicator = i;
    }

    public Vector2 GetSpawnPoint() {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

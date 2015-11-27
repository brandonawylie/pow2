using UnityEngine;
using System.Collections;

public class PlayerIndicatorController : MonoBehaviour {
    public TextMesh playerNumberIndicator, playerHealthIndicator;
    public SpriteRenderer playerIndicatorTriangle;
    public PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerNumberIndicator.text = "P" + playerController.playerNumber;
	    playerNumberIndicator.color = playerController.GetColor();
        playerHealthIndicator.color = playerController.GetColor();
        playerIndicatorTriangle.color = playerController.GetColor();

        if (Camera.main.GetComponent<EnvironmentController>().isSinglePlayer) {
            playerNumberIndicator.text = "";
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
        UpdatePlayerHealthIndicator();



        transform.position = playerController.transform.position;
	}

    void UpdatePlayerHealthIndicator() {
        if (playerHealthIndicator.text.Length == playerController.hp) {
            return;    
        }
        string res = "";
        for (int i = 0; i < playerController.hp; i++) {
            res += "*";
        }
        playerHealthIndicator.text = res;
    }
}

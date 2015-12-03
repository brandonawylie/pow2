using UnityEngine;
using System.Collections;

public class MultiplayerWinController : MonoBehaviour {

    public TextMesh playerWinText;
    public TextMesh goBackToMainMenuText;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1) {
            playerWinText.text = "Player " + players[0].GetComponent<PlayerController>().playerNumber + " Wins!";
            goBackToMainMenuText.text = "Press a to return to main menu";

            bool goBack = Input.GetButtonDown("A_1") || Input.GetButtonDown("A_2") || Input.GetButtonDown("A_3") || Input.GetButtonDown("A_4");
            if (goBack) {
                Application.LoadLevel("MainMenu");
            }
        }

    }
}

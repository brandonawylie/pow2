using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuEventController : MonoBehaviour {

    public GameObject[] menuButtons;
    public Text p1Connect, p2Connect, p3Connect, p4Connect;

    private bool inMultiplayerPhase;
    private int currentIndex;
    private int skipFrames;
    private int currentSkipFrames;

	// Use this for initialization
	void Start () {
        inMultiplayerPhase = false;

	    currentIndex = 0;
        HighlightAtCurrentIndex();

        skipFrames = 20;
        currentSkipFrames = 0;

        AddPlayerToGame(1);
	}

    void HighlightAtCurrentIndex() {
        print("highlighting at index = " + currentIndex);
        menuButtons[currentIndex].GetComponent<Button>().Select();
    }

    void SelectAtCurrentIndex() {
        menuButtons[currentIndex].GetComponent<Button>().onClick.Invoke();
    }

    void AddPlayerToGame(int num) {
        PlayerPrefs.SetInt("p" + num, 1);
    }

    void RemoveAllPlayersFromGame() {
        PlayerPrefs.SetInt("p2", 0);
        PlayerPrefs.SetInt("p3", 0);
        PlayerPrefs.SetInt("p4", 0);
    }

    void StartMultiplayerGame() {
        Application.LoadLevel("Multiplayer_1");
    }
	
	// Update is called once per frame
	void Update () {
        if (inMultiplayerPhase) {
            bool p2 = Input.GetButtonDown("Start_2");
            bool p3 = Input.GetButtonDown("Start_3");
            bool p4 = Input.GetButtonDown("Start_4");

            if (p2) {
                print("p2 connected");
                p2Connect.enabled = true;
                AddPlayerToGame(2);
            }
            if (p3) {
                p3Connect.enabled = true;
                AddPlayerToGame(3);
            }
            if (p4) {
                p4Connect.enabled = true;
                AddPlayerToGame(4);
            }

            bool select = Input.GetButtonDown("A_1");
            bool cancel = Input.GetButtonDown("B_1");

            if (select) {
                StartMultiplayerGame();
            }

            if (cancel) {
                inMultiplayerPhase = false;
                RemoveAllPlayersFromGame();
            }


            } else {
            bool select = Input.GetButtonDown("A_1");
            if (select) {
                SelectAtCurrentIndex();
            }

            if (currentSkipFrames > 0) {
                currentSkipFrames--;
                return;
            }
	        float inputX = Input.GetAxisRaw("L_XAxis_1");
            float inputY = Input.GetAxisRaw("L_YAxis_1");

            bool changed = false;
            if (inputY < 0) {
                currentIndex--;
                changed = true;
            }

            if (inputY > 0) {
                currentIndex++;
                changed = true;
            }

            if (currentIndex < 0)
                currentIndex = 0;
            else if (currentIndex >= menuButtons.Length)
                currentIndex = menuButtons.Length - 1;

            if (changed) {
                HighlightAtCurrentIndex();
                currentSkipFrames = skipFrames;
            }
        }
    }

    public void DoSinglePlayer() {
        print("doing single player");
        Application.LoadLevel("Survive");
    }

    public void DoMultiPlayer() {
        print("doing multiplayer");
        inMultiplayerPhase = true;
        p1Connect.enabled = true;
    }

    public void DoOptions() {
        print("doing options");
    }

    public void DoExit() {
        print("doing exit");
        Application.Quit();
    }

}

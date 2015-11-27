using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // basic props
    public int hp;

    // player properties for multiplayer
    public int playerNumber;

    // name variables for buttons
    private string moveAxisJoystickName;
    private string jumpButtonName;
    private string diveButtonName;

    // Movement variables
    private bool isFacingRight;
    private Vector2 jumpVelocity;
    private float moveVelocity;
    private float diveVelocity;
    
    // utility for stick deadzone
    private float deadzoneThreshold;
    
    // components of the gameobject and children of the gameobject
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D playerRigidbody;

    // jump/dive variables
    private int usedJumps;
    private int totalJumps;
    private float nRotationsJump;
    private float nRotationsDive;
    private int nFramesJump;
    private int nFramesDive;
    private int curFramesJump;
    private int curFramesDive;


	// Use this for initialization
	void Start () {
        moveAxisJoystickName = "L_XAxis_" + playerNumber;
        jumpButtonName = "A_" + playerNumber;
        diveButtonName = "X_" + playerNumber;

        moveVelocity = 500.0f;
        diveVelocity = 35.0f;
        isFacingRight = true;
	    jumpVelocity = new Vector2(moveVelocity, 1250.0f);

        deadzoneThreshold = .2f;

       playerSpriteRenderer = GetComponent<SpriteRenderer>();
       playerRigidbody = GetComponent<Rigidbody2D>();

       usedJumps = 0;
       totalJumps = 2;
       nRotationsJump = 1;
       nRotationsDive = 1;
       nFramesJump = 18;
       nFramesDive = 20;
       curFramesJump = 0;
       curFramesDive = 0;

       SetupPlayerColor();
	}

    void SetupPlayerColor() {
        switch(playerNumber) {
            case 1:
                playerSpriteRenderer.color = Color.green;
                break;
            case 2:
                playerSpriteRenderer.color = Color.cyan;
                break;
            case 3:
                playerSpriteRenderer.color = Color.magenta;
                break;
            case 4:
                playerSpriteRenderer.color = Color.yellow;
                break;         
        }
    }
	
	// Update is called once per frame
	void Update () {
	    DoJumpRotation();
	}

    void DoJumpRotation() {
        if (curFramesJump >= nFramesJump) {
            playerSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        curFramesJump++;
        float nDegreesPerFrame = nRotationsJump * 360.0f / nFramesJump;
        Vector3 dir = !isFacingRight ? Vector3.forward : Vector3.back;
        transform.Rotate(dir * nDegreesPerFrame);
    }

    void FixedUpdate() {
        DoMovement();
        DoJump();
        DoDive();
    }

    void DoMovement() {
        float inputX = Input.GetAxisRaw(moveAxisJoystickName);

        if (inputX > deadzoneThreshold || inputX < -deadzoneThreshold) {
            if (inputX > 0 && !isFacingRight) {
                Flip();
            } else if (inputX < 0 && isFacingRight) {
                Flip();
            }

            this.playerRigidbody.velocity = new Vector2(moveVelocity * inputX * Time.fixedDeltaTime, this.playerRigidbody.velocity.y);
        }
    }

    void DoJump() {
        if (Input.GetButtonDown(jumpButtonName)) {
            if (usedJumps < totalJumps) {      
                usedJumps++;
                //StartJumpRotation();

                Vector2 jumpWithDirectionVector = new Vector2(jumpVelocity.x * (isFacingRight ? 1 : -1), jumpVelocity.y);
                this.playerRigidbody.velocity += jumpWithDirectionVector * Time.fixedDeltaTime;
                if (usedJumps == 1) {
                    curFramesJump = 0;
                }
            }
        }
    }

    void DoDive() {

    }

    /*
        Getters
    */
    public Color GetColor() {
        return playerSpriteRenderer.color;
    }

    /*
        Utility functions
    */
    void Flip() {
        isFacingRight = !isFacingRight;

        Vector3 localScale = playerSpriteRenderer.transform.localScale;
        localScale.x = isFacingRight ? 1 : -1;

        playerSpriteRenderer.transform.localScale = localScale;
    }

    /*
        Collision functions
    */
    void OnTriggerStay2D(Collider2D other) {
		usedJumps = 0;

    }

}

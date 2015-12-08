using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // basic props
    public int hp;
    private bool isGrounded;

    // player properties for multiplayer
    public int playerNumber;
    public GameObject indicator;

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
    private Color playerColor;
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

    private int nFramesJumpReset;
    private int curFramesJumpReset;

    private bool isSpawning;
    private bool isOnWallLeft;
    private bool isOnWallRight;

	// Use this for initialization
	void Start () {
        moveAxisJoystickName = "L_XAxis_" + playerNumber;
        jumpButtonName = "A_" + playerNumber;
        diveButtonName = "X_" + playerNumber;

        moveVelocity = 500.0f;
        diveVelocity = 25.0f;
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
       curFramesJump = nFramesJump;
       curFramesDive = nFramesDive;
       nFramesJumpReset = 10;
       curFramesJumpReset = nFramesJumpReset;

       isSpawning = false;
       isOnWallLeft = false;
       isOnWallRight = false;
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
        playerColor = playerSpriteRenderer.color; 
    }
	
	// Update is called once per frame
	void Update () {
	    //DoJumpRotation();
        DoDiveRotation();
        DoJumpResetFlash();
	}

    void DoJumpResetFlash() {
        if (curFramesJumpReset >= nFramesJumpReset) {
            return;
        }

        playerSpriteRenderer.color = Color.white;

        curFramesJumpReset++;
        if (curFramesJumpReset == nFramesJumpReset) {
            playerSpriteRenderer.color = playerColor;
        }
    }

    void DoJumpRotation() {
        if (curFramesJump >= nFramesJump) {
            return;
        }
        curFramesJump++;
        float nDegreesPerFrame = nRotationsJump * 360.0f / nFramesJump;
        Vector3 dir = !isFacingRight ? Vector3.forward : Vector3.back;
        transform.Rotate(dir * nDegreesPerFrame);

        if (curFramesJump >= nFramesJump) {
           playerSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

    void DoDiveRotation() {
        if (curFramesDive >= nFramesDive) {
            return;
        }
        curFramesDive++;
        float nDegreesPerFrame = nRotationsDive * 360.0f / nFramesDive;
        Vector3 dir = !isFacingRight ? Vector3.forward : Vector3.back;
        transform.Rotate(dir * nDegreesPerFrame);
        if (curFramesDive >= nFramesDive) {
            playerSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FixedUpdate() {
        if (isSpawning) {
            return;
        }
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

            if (inputX > 0 && isOnWallRight) {
                return;
            } else if (inputX < 0 && isOnWallLeft) {
                return;
            }

            this.playerRigidbody.velocity = new Vector2(moveVelocity * inputX * Time.fixedDeltaTime, this.playerRigidbody.velocity.y);
        }
    }

    void DoJump() {
        if (Input.GetButtonDown(jumpButtonName)) {
            if (usedJumps < totalJumps) {      
                usedJumps++;
                isGrounded = false;

                Vector2 jumpWithDirectionVector = new Vector2(jumpVelocity.x * (isFacingRight ? 1 : -1), jumpVelocity.y);
                this.playerRigidbody.velocity = jumpWithDirectionVector * Time.fixedDeltaTime;
                if (usedJumps == 1) {
                    curFramesJump = 0;
                }
            }
        }
    }

    void DoDive() {
        if (Input.GetButtonDown(diveButtonName)) {
            if (!isGrounded) {
                curFramesDive = 0;
                playerRigidbody.velocity = new Vector2(0, -diveVelocity);
            }
        }
    }

    /*
        Getters
    */
    public Color GetColor() {
        return playerSpriteRenderer.color;
    }

    public bool GetIsSpinning() {
        return (curFramesDive < nFramesDive) || (curFramesJump < nFramesJump);
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

    void DoDie() {
        hp -= 1;
        if (hp <= 0) {
            Destroy(indicator.gameObject);
            Destroy(gameObject);
        }

        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("DeathParticleSystem"), this.transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().startColor = playerSpriteRenderer.color;

        go = (GameObject)GameObject.Instantiate(Resources.Load("MinusOneParticleSystem"), this.transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material.SetColor("_DETAIL_MULX2", playerSpriteRenderer.color);
         go.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material.SetColor("_EMISSION", playerSpriteRenderer.color);

        playerSpriteRenderer.enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        indicator.SetActive(false);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(2.0f);
        indicator.SetActive(true);
        this.transform.position = Camera.main.GetComponent<EnvironmentController>().GetSpawnPoint();
        playerSpriteRenderer.enabled = true;
        playerRigidbody.isKinematic = true;
        isSpawning = true;
        yield return new WaitForSeconds(1.0f);
        GetComponent<PolygonCollider2D>().enabled = true;
        playerRigidbody.isKinematic = false;
        isSpawning = false;
    }

    /*
        Collision functions
    */
    void OnTriggerStay2D(Collider2D other) {
		
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag != "NoJumpReset") {
            curFramesJumpReset = 0;
            usedJumps = 0;
            isGrounded = true;
        }

        if (coll.gameObject.tag == "Enemy") {
            if (!GetIsSpinning()) {
                DoDie();
            }
            else {  
                GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("PlusOneParticleSystem"), this.transform.position, Quaternion.identity);
                go.GetComponent<ParticleSystem>().startColor = playerSpriteRenderer.color;
                playerRigidbody.AddForce(jumpVelocity * (isFacingRight ? 1 : -1));
            }
        }

        if (coll.gameObject.tag == "Player") {
            if (!GetIsSpinning()) {
                if (coll.gameObject.GetComponent<PlayerController>().GetIsSpinning() || coll.gameObject.transform.position.y > transform.position.y) {
                    DoDie();
                }
            }
        }

        if (coll.gameObject.tag == "Wall") {
            isOnWallRight = isOnWallLeft = false;
            if (coll.transform.position.x < transform.position.x) {
                isOnWallLeft = true;
            } else if (coll.transform.position.x > transform.position.x) {
                isOnWallRight = true;
            }

        }
    }

     void OnCollisionExit2D(Collision2D coll) {
        if (coll.transform.position.x < transform.position.x) {
            isOnWallLeft = false;
        } else if (coll.transform.position.x > transform.position.x) {
            isOnWallRight = false;
        }
    }

}

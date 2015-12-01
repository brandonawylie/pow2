using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    // basic props
    public int hp;
    private bool isGrounded;

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
       curFramesJump = nFramesJump;
       curFramesDive = nFramesDive;

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
        DoDiveRotation();
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
                isGrounded = false;
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
        if (Input.GetButtonDown(diveButtonName)) {
            if (!isGrounded && curFramesJump >= nFramesJump) {
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
            Destroy(gameObject);
        }

        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("DeathParticleSystem"), this.transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().startColor = playerSpriteRenderer.color;

        go = (GameObject)GameObject.Instantiate(Resources.Load("MinusOneParticleSystem"), this.transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material.SetColor("_DETAIL_MULX2", playerSpriteRenderer.color);
         go.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material.SetColor("_EMISSION", playerSpriteRenderer.color);

        playerSpriteRenderer.enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(2.0f);
        this.transform.position = Camera.main.GetComponent<EnvironmentController>().GetSpawnPoint();
        playerSpriteRenderer.enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    /*
        Collision functions
    */
    void OnTriggerStay2D(Collider2D other) {
		usedJumps = 0;
        isGrounded = true;
    }

    void OnCollisionEnter2D(Collision2D coll) {
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
    }

}

using UnityEngine;
using System.Collections;

public class BackAndForthEnemyScript : MonoBehaviour {
    private int dir;
    private Rigidbody2D enemyRigidbody;
    private Vector2 moveVelocity;

	// Use this for initialization
	void Start () {
        enemyRigidbody = GetComponent<Rigidbody2D>();

	    moveVelocity = new Vector2(10.0f, 0);
        dir = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        this.enemyRigidbody.velocity = moveVelocity * dir;
    }

    /*
        Collision functions
    */
    void OnTriggerEnter2D(Collider2D other) {
		dir = -dir;
    }
}

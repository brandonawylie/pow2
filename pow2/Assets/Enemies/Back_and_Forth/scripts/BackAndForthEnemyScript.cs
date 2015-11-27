using UnityEngine;
using System.Collections;

public class BackAndForthEnemyScript : MonoBehaviour {
    private int dir;
    private Rigidbody2D enemyRigidbody;
    private Vector2 moveVelocity;

    public int hp;

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

    void DoApplyDamage(int n) {
        hp -= n;
        if (hp <= 0)
            DoDie();
    }

    void DoDie() {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("DeathParticleSystem"), this.transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().startColor = GetComponent<MeshRenderer>().material.color;

        Destroy(gameObject);
    }

    /*
        Collision functions
    */
    void OnTriggerEnter2D(Collider2D other) {
		dir = -dir;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player") {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            if (pc.GetIsSpinning()) {
                DoApplyDamage(1);
            } 
        }
    }
}

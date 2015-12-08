using UnityEngine;
using System.Collections;

public class BounceBlockController : MonoBehaviour {
    
    public Vector2 appliedForce;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other) {
	    if (other.attachedRigidbody.transform.position.y > transform.position.y) {
            other.attachedRigidbody.velocity = new Vector2(0,0);
            other.attachedRigidbody.AddForce(appliedForce);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	Rigidbody2D rb;
	public float speed;

    bool inside = false;

	void Start () {
		rb=GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.angularVelocity=speed;
	}

    int stayFrames = 0;
    GameObject playerInside;

    private void FixedUpdate()
    {
        if (stayFrames > 4 && playerInside != null)
        {
            playerInside.GetComponent<Player>().Die();
            stayFrames = 0;
            playerInside = null;
        }

    }

    void OnCollisionStay2D(Collision2D col){

        // If our object is even rotating
        if(Mathf.Abs(speed) > 0)
        {
            if(Mathf.Abs(rb.angularVelocity) < 0.2f && col.gameObject.tag == "Player" && col.gameObject.GetComponent<Player>() != null)
            {
                stayFrames++;
                playerInside = col.gameObject;
            }
        } else
        {
            stayFrames = 0;
        }
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            stayFrames = 0;
        }
    }
}


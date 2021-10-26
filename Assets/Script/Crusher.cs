using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour {

    public AudioClip extend, retract;

	public float crushTime,timeCrushed,power;

    public bool invert;

	bool extended=false;
	Vector2 start;

	TargetJoint2D tj;
	// Use this for initialization
	void Start () {
        if (invert) extended = true;

		tj = GetComponent<TargetJoint2D> ();
		tj.frequency = power;
		start = transform.position;
        tj.target = start;

        Invoke ("Crush", crushTime);
	}

	void OnCollisionStay2D(Collision2D col){


		if (tj.reactionForce.magnitude >= 5000 && col.gameObject.tag=="Player")
			col.gameObject.GetComponent<Player> ().Die ();
	}

	// Update is called once per frame
	void Crush () {
        if (!extended)
        {
            tj.target = start + (((Vector2)transform.up) * .8f);
            GameManager.gameManager.PlayEnviornmentSFX(extend, transform.position);
        }
        else
        {
            tj.target = start;
            GameManager.gameManager.PlayEnviornmentSFX(retract, transform.position);
        }

		extended = !extended;

		if(extended)
			Invoke ("Crush", timeCrushed);
		else
			Invoke ("Crush", crushTime);
	}
}

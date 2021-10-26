using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D col) {
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;

		if (col.gameObject.layer == 8)
			gameObject.tag = "Untagged";

        Destroy(gameObject, 4);
	}
}

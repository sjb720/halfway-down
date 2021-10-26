using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTrigger : MonoBehaviour {

	void Start () {
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player"){
			
			transform.parent.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
		}
	}
}

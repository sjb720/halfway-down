using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeStick : MonoBehaviour {
	bool stuck=false;
    public AudioClip stickSound;

	void OnTriggerEnter2D (Collider2D col) {
		
		if (!stuck&&col.gameObject.layer!=16&&col.gameObject.tag!="Player") {
			transform.parent.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
			transform.parent.GetComponent<Collider2D> ().enabled = false;
			transform.parent.transform.Find ("Handle").gameObject.SetActive (false);

			if(col.gameObject.layer!=8)
				transform.parent.transform.parent = col.gameObject.transform;

			transform.parent.gameObject.GetComponent<Rigidbody2D> ().simulated = false;
			stuck = true;

            GameManager.gameManager.PlayEnviornmentSFX(stickSound, transform.position);
		}
	}

}

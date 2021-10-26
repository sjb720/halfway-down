using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleBox : MonoBehaviour {

	public GameObject linkedBox;
    public float jumpVelocityOutOfBox = 15;
	public float timeBeforeJump = 0.5f;
	bool canTele=true;

	void OnDrawGizmosSelected() {
		if (linkedBox != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position, linkedBox.transform.position);
		}
	}
		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player"&&canTele) {
			StartCoroutine(Teleport (col.gameObject));
		}
	}

	IEnumerator Teleport(GameObject player){
		StartCoroutine (DisableTele());
		StartCoroutine( linkedBox.GetComponent<TeleBox> ().DisableTele ());
		GetComponent<Animator> ().SetTrigger ("close");
		linkedBox.GetComponent<Animator> ().SetTrigger ("close");
		yield return new WaitForSeconds (0.5f);
		player.transform.position = linkedBox.transform.position;
		yield return new WaitForSeconds (timeBeforeJump);
		GetComponent<Animator> ().SetTrigger ("close");
		linkedBox.GetComponent<Animator> ().SetTrigger ("close");
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, jumpVelocityOutOfBox);
	}

	public IEnumerator DisableTele(){
		canTele = false;
		yield return new WaitForSeconds (1f+timeBeforeJump);
		canTele = true;
	}
		
}

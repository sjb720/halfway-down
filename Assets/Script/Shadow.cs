using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

	Animator anim;
	GameObject arm;
	public GameObject limb1,limb2;
	bool swung=false;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
	}

	void Start(){
		arm = transform.Find ("joint").gameObject;
		arm.SetActive (false);
		GetComponent<SpriteRenderer> ().enabled = false;
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player"&&!swung)
			Swing ();
	}

	void Swing(){
		//Special effect here probably
		swung=true;
		GetComponent<SpriteRenderer> ().enabled = true;
		arm.SetActive (true);
		anim.SetTrigger ("swing");
		StartCoroutine (Disappear ());
}
	IEnumerator Disappear(){
		yield return new WaitForSeconds (1.0f);
		limb1.GetComponent<SpriteRenderer> ().enabled = false;
		limb2.GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		transform.Find("ps").gameObject.GetComponent<ParticleSystem>().Play();
	}
}

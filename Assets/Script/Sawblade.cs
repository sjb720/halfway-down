using UnityEngine;
using System.Collections;

public class Sawblade : MonoBehaviour {

	Rigidbody2D rb;
	public float speed;

	void Awake(){
		rb= GetComponent<Rigidbody2D>();
	}

	void Update () {
		rb.angularVelocity=speed;
	}
}

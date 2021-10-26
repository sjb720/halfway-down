using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

	public bool randomize;
	public float randomRange,shootSpeed,shootRate,angularVelocity;
	public GameObject cannonBall;
	public float distance=20;

    public AudioClip cannonShoot;

	float startAngle;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("Shoot", shootRate, shootRate);
		startAngle = transform.eulerAngles.z;
	}

	void Shoot(){

		if (transform.Find ("burst"))
			transform.Find ("burst").GetComponent<ParticleSystem> ().Play ();

        if (GameObject.FindGameObjectWithTag("Player") == null) return;

        GameManager.gameManager.PlayEnviornmentSFX(cannonShoot, transform.position);

		if (Vector2.Distance (transform.position, GameObject.FindGameObjectWithTag ("Player").transform.position) > distance)
			return;

		if (randomize)
			transform.eulerAngles = new Vector3 (0, 0, startAngle + Random.Range (-randomRange, randomRange));

		GameObject cb = (GameObject)Instantiate (cannonBall, transform.position, Quaternion.identity);
		cb.GetComponent<Rigidbody2D>().velocity=transform.right*-shootSpeed;
		cb.GetComponent<Rigidbody2D> ().angularVelocity = angularVelocity;
		Destroy(cb,10);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

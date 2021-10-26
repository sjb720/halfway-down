using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

	bool canKill=true;

    List<Vector2> velocityHistory;
    Rigidbody2D rb;

    public AudioClip thud;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        velocityHistory = new List<Vector2>();
    }

    private void FixedUpdate()
    {

        velocityHistory.Add(rb.velocity);

        if (velocityHistory.Count > 3)
        {
            velocityHistory.RemoveAt(0);
        }
    }

    public float threshold = 15;

    bool HasBeenMovingFast()
    {
        for(int i = 0; i < velocityHistory.Count; i++)
        {
            if (velocityHistory[i].magnitude < threshold) return false;
        }

        return true;
    }

    float AverageSpeed()
    {
        float av = 0;
        for (int i = 0; i < velocityHistory.Count; i++)
        {
            av += velocityHistory[i].magnitude;
        }

        return av / 3f;
    }

    void OnCollisionEnter2D (Collision2D col) {

        float avSpd = AverageSpeed();

        if (avSpd > 3)
            GameManager.gameManager.PlayEnviornmentSFX(thud, transform.position, avSpd / 20);

		if (canKill && col.gameObject.tag == "Player" && col.gameObject.GetComponent<Player>() != null && HasBeenMovingFast())
			col.gameObject.GetComponent<Player> ().Die ();
		
	}
}

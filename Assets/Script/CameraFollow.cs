using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	 public GameObject player;
	TargetJoint2D tj;

	void Start () {
		
		tj=GetComponent<TargetJoint2D>();
	}
	
	void Update () {
		if(player==null)
			FindPlayer();
		else
			tj.target=new Vector2(player.transform.position.x,player.transform.position.y);

	}

	void FindPlayer(){
		player=GameObject.FindGameObjectWithTag("Player");
	}
}

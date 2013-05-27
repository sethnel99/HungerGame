using UnityEngine;
using System.Collections;

public class PlayerBodyPart : MonoBehaviour {
	
	public BodyHit.BodyPartEnum bodyPart;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "EnemyAttack")
		{
			transform.parent.SendMessage("TakenHit", new BodyHit(bodyPart, col));
		}
	}
}

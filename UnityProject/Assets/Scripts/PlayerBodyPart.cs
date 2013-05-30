using UnityEngine;
using System.Collections;

public class PlayerBodyPart : MonoBehaviour {
	
	public PlayerVitals.BodyPart bodyPart;
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
			transform.parent.GetComponent<PlayerVitals>().TakenHit(bodyPart, col.gameObject.GetComponent<EnemyAttackBox>());
		}
	}
}

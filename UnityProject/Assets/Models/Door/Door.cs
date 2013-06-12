using UnityEngine;
using System.Collections;

public class Door : StructurePiece {
	
	float doorTimer=5;
	bool inAnimation = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	protected override void Update () {
		base.Update();
		
		if(doorTimer > 0)
		{
			doorTimer -= Time.deltaTime;
		}
	}
	
	void LateUpdate() {
		if(!isSet && snappingOn && SnapHits.Count!=0)
		{
			float bestAngle = 360;
			Quaternion snapRotate = transform.rotation;
			SnapCollision bestHit = new SnapCollision(null, null, -1);
			foreach(SnapCollision snapHit in SnapHits)
			{
				if(snapHit.Priority >= bestHit.Priority)
				{
					if(snapHit.Priority == 0)//Ground
					{
						snapRotate = transform.rotation; //Hit the ground, don't snap any rotation
						bestHit = snapHit;
					}
					else
					{
						float angle = Quaternion.Angle(transform.rotation, snapHit.SnapCorner.transform.parent.transform.rotation);
						if(angle < bestAngle) //if this Structure piece has a more similar angle to ours than what we've seen
						{
							bestAngle = angle;
							if(angle < 27) //if the angle is less than 20, we will snap to it
							{
								snapRotate = snapHit.SnapCorner.transform.rotation;
							}
							else
							{
								snapRotate = transform.rotation;
							}
							
							bestHit = snapHit;
						}
					}
				}
			}
			transform.rotation = snapRotate;
			if(bestHit.Priority == 0)
			{
				transform.Translate(new Vector3(0f,-.08f,0f),  Space.World);
			}
			else
			{
				transform.Translate(bestHit.SnapCorner.transform.position-bestHit.Corner.transform.position,  Space.World);
			}
			isSet = true;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player" && !inAnimation)
		{
			doorTimer = 5;
			StartCoroutine("COOpen");
		}
	}
	
	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Player" && doorTimer<=0 && !inAnimation)
		{
			StartCoroutine("COOpen");
		}
	}
	
	IEnumerator COOpen() {
		inAnimation = true;
		gameObject.animation.Play("Door_open");
	    yield return new WaitForSeconds(5f);
		gameObject.animation.Play("Door_close");
		inAnimation = false;
	}
}

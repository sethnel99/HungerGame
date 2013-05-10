using UnityEngine;
using System.Collections;
using System.Collections.Generic;


	public class SnapCollision {
		public GameObject Corner;
		public GameObject SnapCorner;
		public int Priority; //Plank = 1, Ground = 0
		
		public SnapCollision(GameObject corner, GameObject snapCorner, int priority)
		{
			Corner = corner;
			SnapCorner = snapCorner;
			Priority = priority;
		}
	}

public class PlankBuild : MonoBehaviour {
	
	public bool isSet = false;
	bool snappingOn = true;

	
	List<SnapCollision> SnapHits = new List<SnapCollision>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//If we aren't set, let's drop it a bit for next frame to check again
		if(!isSet)
		{
			transform.Translate(new Vector3(0f,-.005f,0f));
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
//			Debug.Log(bestHit.Corner.transform.position.ToString() +" snap to " + bestHit.SnapCorner.transform.position.ToString() +
//							" movement= " + (bestHit.SnapCorner.transform.position-bestHit.Corner.transform.position).ToString());
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
	
	
	public void GroundCollision()
	{
		//always snap to ground?
		//transform.position += new Vector3(0f,-.05f,0f);
		//isSet = true;
		
	}
	public void SnapSpotCollision(GameObject corner, GameObject snapSpot, int priority)
	{
		SnapHits.Add(new SnapCollision(corner, snapSpot, priority));
	}
}

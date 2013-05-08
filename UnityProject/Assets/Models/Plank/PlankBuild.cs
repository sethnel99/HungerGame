using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlankBuild : MonoBehaviour {
	
	public bool isSet = false;
	bool snappingOn = true;
	
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
	
	public GameObject[] Corners;
	
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
			int currentHighestPriority = -1;
			float bestAngle = 360;
			Quaternion snapRotate = transform.rotation;
			Vector3 snapTranslate = new Vector3(0,0,0);
			foreach(SnapCollision snapHit in SnapHits)
			{
				if(snapHit.Priority >= currentHighestPriority)
				{
					if(snapHit.Priority == 0)//Ground
					{
						snapRotate = transform.rotation; //Hit the ground, don't snap any rotation
						snapTranslate = new Vector3(0f,-.08f,0f); //Move it down a little
						currentHighestPriority = 0;
					}
					else
					{
						float angle = Quaternion.Angle(transform.rotation, snapHit.SnapCorner.transform.parent.transform.rotation);
						if(angle < bestAngle) //if this Structure piece has a more similar angle to ours than what we've seen
						{
							bestAngle = angle;
							if(angle < 23) //if the angle is less than 20, we will snap to it
							{
								snapRotate = snapHit.SnapCorner.transform.parent.transform.rotation;
							}
							else
							{
								snapRotate = transform.rotation;
							}
							snapTranslate = snapHit.SnapCorner.transform.position-snapHit.Corner.transform.position;
							currentHighestPriority = snapHit.Priority;
						}
					}
				}
			}
					
			transform.rotation = snapRotate;
			transform.position += snapTranslate;
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
//		if(snappingOn)
//		{
//			
//			//ASK IAN ABOUT THIS
//			if(Quaternion.Angle(transform.rotation, snapSpot.transform.parent.transform.rotation) < 20f)
//			{
//				transform.rotation = snapSpot.transform.parent.transform.rotation;
//			}
//			
//			Vector3 snapVector = snapSpot.transform.position-corner.transform.position;
//			transform.position += snapVector;
//			
//
//			
//			isSet = true;
//		}
		SnapHits.Add(new SnapCollision(corner, snapSpot, priority));
	}
}

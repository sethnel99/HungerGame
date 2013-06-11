using UnityEngine;
using System.Collections;

public class RoofPiece1 : StructurePiece {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
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
}

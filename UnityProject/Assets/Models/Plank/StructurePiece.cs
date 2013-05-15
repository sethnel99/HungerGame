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

public class StructurePiece : MonoBehaviour {
	
	public bool isSet = false;
	public bool snappingOn = true;
	
	protected List<SnapCollision> SnapHits = new List<SnapCollision>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		//If we aren't set, let's drop it a bit for next frame to check again
		//Not sure all pieces want this though
		if(!isSet)
		{
			transform.Translate(new Vector3(0f,-.005f,0f));
		}
	}
	
	public void SnapSpotCollision(GameObject corner, GameObject snapSpot, int priority)
	{
		SnapHits.Add(new SnapCollision(corner, snapSpot, priority));
	}
}

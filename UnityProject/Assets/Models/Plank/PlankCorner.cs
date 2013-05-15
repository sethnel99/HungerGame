using UnityEngine;
using System.Collections;

public class PlankCorner : MonoBehaviour {
	
	StructurePiece parentScript;
	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<StructurePiece>() as StructurePiece;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter(Collider col){
		if (parentScript!=null && !parentScript.isSet)
		{
			if(col.gameObject.tag == "Terrain"){
				//Debug.Log("TERRAIN HIT");
				SendSnapCollisionToParent(gameObject, col.gameObject, 0);
			}
			else if(col.gameObject.tag == "StructureSnapSpot" && col.gameObject.transform.parent.gameObject.tag != "BuildingGhost"){
				//Debug.Log("SNAP SPOT HIT");
				SendSnapCollisionToParent(gameObject, col.gameObject, 1);
			}
			else if(col.gameObject.tag == "StructurePiece" && col.gameObject != transform.parent.gameObject){
				//Debug.Log("STRUCTURE HIT");
				//THIS STILL NEEDS SOME WORK
				
				//SendSnapCollisionToParent(gameObject, col.gameObject, 0);
			}
//			else if(col.gameObject.tag == "StructureGhost"){
//				Debug.Log("GHOST HIT");
//				parentScript.SnapSpotCollision(gameObject, col.gameObject, 1);
//			}
			else{
				
			}
			//Debug.Log("STRUCTURE HIT");
		}
	}
	void OnCollisionEnter(Collision cols){
	}
	
	void OnTriggerStay(Collider col){
		OnTriggerEnter(col);
	}
	
	void SendSnapCollisionToParent(GameObject corner, GameObject collider, int priority)
	{
		parentScript.SnapSpotCollision(corner, collider, priority);
	}
	
}

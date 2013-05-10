using UnityEngine;
using System.Collections;

public class PlankCorner : MonoBehaviour {
	
	enum ParentTypeEnum{
		Build,
		Equipped,
		Max};
	ParentTypeEnum ParentType;
	PlankBuild parentBuildScript;
	EquippedPlank parentEquippedScript;
	// Use this for initialization
	void Start () {
		
		parentBuildScript = transform.parent.GetComponent("PlankBuild") as PlankBuild;
		parentEquippedScript = transform.parent.GetComponent("EquippedPlank") as EquippedPlank;
		
		if(parentBuildScript != null)
		{
			ParentType = ParentTypeEnum.Build;
		}
		else if(parentEquippedScript != null)
		{
			ParentType = ParentTypeEnum.Equipped;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter(Collider col){
    	if (ParentType == ParentTypeEnum.Equipped || !parentBuildScript.isSet)
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
				//What should we do here?
			}
//			else if(col.gameObject.tag == "StructureGhost"){
//				Debug.Log("GHOST HIT");
//				parentScript.SnapSpotCollision(gameObject, col.gameObject, 1);
//			}
			else{
				
			}
		}
	}
	
	void OnTriggerStay(Collider col){
		OnTriggerEnter(col);
	}
	
	void SendSnapCollisionToParent(GameObject corner, GameObject collider, int priority)
	{
		if(ParentType == ParentTypeEnum.Build)
		{
			parentBuildScript.SnapSpotCollision(corner, collider, priority);
		}
		else if(ParentType == ParentTypeEnum.Equipped)
		{
			parentEquippedScript.SnapSpotCollision(corner, collider, priority);
		}
	}
	
}

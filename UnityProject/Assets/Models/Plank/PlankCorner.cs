using UnityEngine;
using System.Collections;

public class PlankCorner : MonoBehaviour {
	
	PlankBuild parentScript;
	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent("PlankBuild") as PlankBuild;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter(Collider col){
    	if (!parentScript.isSet)
		{
			if(col.gameObject.tag == "Terrain"){
				Debug.Log("TERRAIN HIT");
				parentScript.SnapSpotCollision(gameObject, col.gameObject, 0);
			}
			else if(col.gameObject.tag == "StructureSnapSpot"){
				Debug.Log("SNAP SPOT HIT");
				parentScript.SnapSpotCollision(gameObject, col.gameObject, 1);
			}
			else if(col.gameObject.tag == "StructurePiece" && col.gameObject != transform.parent.gameObject){
				Debug.Log("STRUCTURE HIT");
				//What should we do here?
			}
			else{
				
			}
		}
	}
	
}

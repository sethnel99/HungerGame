using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedPlank : MonoBehaviour {
	GameObject textHints;
	bool showingSnapRenderer = false;
	bool snappingOn= true;
	public GameObject Plank;
	Stack<GameObject> createdPlanks;
	EquippedItem parentScript;
	Renderer snapRenderer;
	
	bool isInAction = false;
	// Use this for initialization
	void Start () {
		createdPlanks = new Stack<GameObject>();
		textHints = GameObject.Find("TextHintGUI");
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;
		//Debug.Log(parentScript);
	}
	
	List<SnapCollision> SnapHits = new List<SnapCollision>();
	// Update is called once per frame
	void Update () {
		
		//FIX ROTATION HACK
		Quaternion rotation = transform.rotation;
		rotation.x = 0;
		rotation.z = 0;
		transform.rotation = rotation;
		
		if(Input.GetButtonDown("Fire1") && !isInAction){
			if(snapRenderer != null)
			{
				snapRenderer.enabled = false;
			}
			snapRenderer = null;
			showingSnapRenderer = false;
			
			StartCoroutine(COAction());
			float rand = Random.Range(0f, 100f);
			if(rand >90)
			{
				textHints.SendMessage("ShowHint",
						"You can undo misplaced pieces by using the Undo button (Backspace).");
			}
			
			//audio.PlayOneShot(testSound);
		}
		if(Input.GetButtonDown("Undo") && !isInAction)
		{
			if(createdPlanks.Count!=0){
				GameObject.Destroy(createdPlanks.Pop());
			}
//			else{
//				parentScript.EquipItem((int)EquippedItem.Equippable.Plank);
//			}
		}
		
	}
	
	public AudioClip swing2Sound;
	IEnumerator COAction() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f); // trigger delay
	    // create bullet and flash effect...
		
		
		//gameObject.animation.Play("axeSwing2");
		Quaternion rotation = transform.rotation;
		rotation.x = 0;
		//rotation.y = 0;
		rotation.z = 0;
		//rotation.w = 0;
	    createdPlanks.Push(Instantiate(Plank, transform.position, rotation) as GameObject);
		
		//yield return 0; // wait 1 frame
	    // stop flash
	    //yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(swing2Sound);
		//parentScript.Reset((int)EquippedItem.Equippable.Plank);
	    yield return new WaitForSeconds(1.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	
	
	
	
	//SNAP PREVIEWS,  SHOULD PROBABLY INHERIT THIS FROM PLANK BUILD OR SOMETHING
	void LateUpdate() {
		Renderer newSnapRenderer= null;
		if(snappingOn && SnapHits.Count!=0 && !showingSnapRenderer)
		{
			float bestAngle = 360;
			Quaternion snapRotate = transform.rotation;
			SnapCollision bestHit = new SnapCollision(null, null, -1);
			foreach(SnapCollision snapHit in SnapHits)
			{
				if(snapHit.Priority >= bestHit.Priority && snapHit.SnapCorner != null)
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
							if(angle < 25) //if the angle is less than 20, we will snap to it
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
			//transform.parent = null;
			//transform.rotation = snapRotate;
			
//			Debug.Log("GHOST: "+bestHit.Corner.transform.position.ToString() +" snap to " + bestHit.SnapCorner.transform.position.ToString() +
//							" movement= " + (bestHit.SnapCorner.transform.position-bestHit.Corner.transform.position).ToString());
			if(bestHit.Priority == 0)
			{
				//transform.Translate(new Vector3(0f,-.08f,0f),  Space.World);
				//transform.position = transform.parent.position+parentScript.equippableTransform[(int)EquippedItem.Equippable.Plank];// - new Vector3(0f, -.08f, 0f);
			}
			else
			{
				//transform.Translate(bestHit.SnapCorner.transform.position-bestHit.Corner.transform.position,  Space.World);
			}
			
			
			if(bestHit.Priority ==1)
			{
				newSnapRenderer = (bestHit.SnapCorner.GetComponent("MeshRenderer") as Renderer);;
				if(snapRenderer != null && snapRenderer != newSnapRenderer)
				{
					Debug.Log("DISABLED NEW HIT");
					snapRenderer.enabled = false;
				}
				snapRenderer = newSnapRenderer;
				showingSnapRenderer=true;
				Debug.Log("ENABLED");
				snapRenderer.enabled = true;
				StartCoroutine(COSnapRenderer());
			}
		}
		
		if(snapRenderer != null && snapRenderer!= newSnapRenderer && !showingSnapRenderer && snapRenderer.enabled)
		{
			Debug.Log("DISABLED RECUR");
			snapRenderer.enabled = false;
		}
	}
	IEnumerator COSnapRenderer() {
	    //yield return new WaitForSeconds(0.1f); // trigger delay
	    // create bullet and flash effect...
	    yield return new WaitForSeconds(.2f); // extra delay before you can shoot again
		//snapRenderer.enabled = false;
		showingSnapRenderer=false;
		SnapHits.Clear();
	}
	
//	void OnPostRender()
//	{
//		if(snappingOn)
//		{
//			snapRenderer.enabled = false;
//			SnapHits.Clear();
//			Debug.Log(SnapHits.Count);
//			//parentScript.Reset((int)EquippedItem.Equippable.Plank);
//		}
//	}
	
	
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

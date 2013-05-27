using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedStructurePiece : StructurePiece {
	
	public GameObject Piece;
	protected Stack<GameObject> createdPieces;
	
	protected GameObject textHints;
	protected bool showingSnapRenderer = false;
	protected EquippedItem parentScript;
	protected Renderer snapRenderer;
	
	protected bool isInAction = false;

    //update the equipped piece's quantity
    Inventory inventory;
	
	// Use this for initialization
	protected virtual void Start () {
		createdPieces = new Stack<GameObject>();
		textHints = GameObject.Find("TextHintGUI");
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		//FIX ROTATION HACK
		Quaternion rotation = transform.rotation;
		rotation.x = 0;
		rotation.z = 0;
		transform.rotation = rotation;
		
		if(Input.GetButtonDown("Fire1") && !isInAction){
            Debug.Log("in action");

            if (inventory.getEquippedEquipable().quantity == 0) {
                textHints.SendMessage("ShowHint", "You are out of planks!");
                return;
            }

            //the player now has one less piece to place
            inventory.decrementEquipable();
            

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
			/*else if(rand<=90 && rand>80)
			{
				textHints.SendMessage("ShowHint",
						"Press the 1 and 2 keys to switch between planks and roofing respectively.");
			}*/
		}
		if(Input.GetButtonDown("Undo") && !isInAction)
		{
			if(createdPieces.Count!=0){
                GameObject pieceToDestroy = createdPieces.Pop();
                GameObject.Destroy(pieceToDestroy);
                //add that item back to their inventory
                if (this is EquippedPlank) {
                    inventory.addItem(new SidePlankItem(1));
                } else if (this is EquippedRoofPiece1) {
                    inventory.addItem(new RoofPlankItem(1));
                }
                
			}
		}
		
	}
	
	public AudioClip swing2Sound;
	IEnumerator COAction() {
	    isInAction = true;
		Quaternion rotation = transform.rotation;
		rotation.x = 0;
		rotation.z = 0;
	    createdPieces.Push(Instantiate(Piece, transform.position, rotation) as GameObject);
	    audio.PlayOneShot(swing2Sound);
	    yield return new WaitForSeconds(1.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	
	//SNAP PREVIEWS
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
			
			if(bestHit.Priority ==1)
			{
				newSnapRenderer = (bestHit.SnapCorner.GetComponent("MeshRenderer") as Renderer);;
				if(snapRenderer != null && snapRenderer != newSnapRenderer)
				{
					//Debug.Log("DISABLED NEW HIT");
					snapRenderer.enabled = false;
				}
				snapRenderer = newSnapRenderer;
				showingSnapRenderer=true;
				//Debug.Log("ENABLED");
				snapRenderer.enabled = true;
				StartCoroutine(COSnapRenderer());
			}
		}
		
		if(snapRenderer != null && snapRenderer!= newSnapRenderer && !showingSnapRenderer && snapRenderer.enabled)
		{
			//Debug.Log("DISABLED RECUR");
			snapRenderer.enabled = false;
		}
	}
	IEnumerator COSnapRenderer() {
	    yield return new WaitForSeconds(.2f);
		showingSnapRenderer=false;
		SnapHits.Clear();
	}
	
	void OnDestroy()
	{
		if(snapRenderer != null)
		{
			snapRenderer.enabled = false;
		}
	}

}

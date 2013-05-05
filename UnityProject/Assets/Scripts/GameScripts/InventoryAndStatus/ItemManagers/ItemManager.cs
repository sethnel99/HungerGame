using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	
	protected Item item;
	
	bool inZone;
	float buttonDownTime;
	public float gatherTime = 2.0f;
	
	public GUIText InteractionTextGUI;
	public GameObject interactionTimer;
	public AudioClip collectSound;
	
	// Use this for initialization
	void Start () {
		inZone = false;
		buttonDownTime = 0.0f;
		
		item = new Item("TestItem", "TestItems",15,0);
		item.addItemType(Item.material);
	}
	
	// Update is called once per frame
	void Update () {
		if (inZone && Input.GetButton ("Interact")) {
			buttonDownTime += Time.deltaTime;
			interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(buttonDownTime/gatherTime);
			
			if (buttonDownTime > gatherTime) {
				GameObject player  = GameObject.FindWithTag("Player");
				player.GetComponent<Inventory>().addItem (this.item);
				InteractionTextGUI.gameObject.GetComponent<InteractionTextManager>().showMessage("You collected "+item.quantity + " " + ((item.quantity > 1)  ? item.pluralName:item.name),2);
				interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0);

				AudioSource.PlayClipAtPoint(collectSound, transform.position);
				Destroy (gameObject);
			}	
		}
		else {
			buttonDownTime = 0.0f;
			interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0);
		}
	}
	
	
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = true;
			InteractionTextGUI.gameObject.GetComponent<InteractionTextManager>().showMessage("Hold [E] to pick up the " + item.name);
		}
	}
	
	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = false;
			InteractionTextGUI.gameObject.GetComponent<InteractionTextManager>().hideMessage();
		}
		
	}
}

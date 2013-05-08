using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	Vector3 rot = new Vector3(5f, 5f, 5f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(rot);
	}
	
	public GUIText textHints;
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			col.gameObject.GetComponentInChildren<EquippedItem>().EquipItem((int)EquippedItem.Equippable.Plank);
			col.gameObject.GetComponent<TestInventory>().Win();
			textHints.SendMessage("ShowHint",
					"You can now build by placing planks!\nGo outside and build a house-like structure." +
					" Let the devs know when you are happy with what you have built.");
			Destroy(gameObject);
		}
	}
}

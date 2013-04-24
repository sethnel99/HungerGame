using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	Item item;
	
	// Use this for initialization
	void Start () {
		item = new Item("TestItem",15);
		item.addItemType(Item.material);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			col.gameObject.GetComponent<Inventory>().addItem (this.item);
			Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;


public class TreeManager : ItemManager {
	
	// Use this for initialization
	void Start () {
		item = new Item("Piece of Wood", "Pieces of Wood", 1,Random.Range(2,5));
		item.addItemType(Item.material);
	}


}

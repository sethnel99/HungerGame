using UnityEngine;
using System.Collections;

public class StoneManager : ItemManager {
	
	// Use this for initialization
	void Start () {
		item = new Item("Sharp Stone",2);
		item.addItemType(Item.material);
	}


}

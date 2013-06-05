using UnityEngine;
using System.Collections;


public class ArrowManager : ItemManager {
	
	// Use this for initialization
	new void Start (){
        base.Start();
        displayName = "Arrow";

        lootTables = new Item[1][];
        lootTables[0] = new Item[1];

        lootTables[0][0] = new ArrowItem(1);
      

        lootFrequencyTables = new int[1][];
        lootFrequencyTables[0] = new int[1];

        lootFrequencyTables[0][0] = 100;

        numberOfItemsDropped = new int[1];
        numberOfItemsDropped[0] = 1;
	}


}

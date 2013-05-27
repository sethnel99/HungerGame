using UnityEngine;
using System.Collections;


public class StoneManager : ItemManager {
	
	// Use this for initialization
	new void Start (){
        base.Start();
        displayName = "Sharp Stone";

        lootTables = new Item[1][];
        lootTables[0] = new Item[3];

        lootTables[0][0] = new SharpStoneItem(1);
        lootTables[0][1] = new SharpStoneItem(2);
        lootTables[0][2] = new SharpStoneItem(3);
      

        lootFrequencyTables = new int[1][];
        lootFrequencyTables[0] = new int[3];

        lootFrequencyTables[0][0] = 20;
        lootFrequencyTables[0][1] = 60;
        lootFrequencyTables[0][2] = 20;

        numberOfItemsDropped = new int[1];
        numberOfItemsDropped[0] = 1;
	}


}

using UnityEngine;
using System.Collections;


public class StoneManager : ItemManager {
	
	// Use this for initialization
	new void Start (){
        base.Start();
        displayName = "Sharp Stone";

        lootTable = new Item[3];

        lootTable[0] =  new SharpStoneItem(1);
        lootTable[1] = new SharpStoneItem(2);
        lootTable[2] = new SharpStoneItem(3);
      

        lootFrequencyTable = new int[3];
        lootFrequencyTable[0] = 20;
        lootFrequencyTable[1] = 60;
        lootFrequencyTable[2] = 20;

        numberOfItemsDropped = 1;
	}


}

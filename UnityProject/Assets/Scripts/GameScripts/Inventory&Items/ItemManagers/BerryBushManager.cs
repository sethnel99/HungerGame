using UnityEngine;
using System.Collections;


public class BerryBushManager : ItemManager {
	
	// Use this for initialization
	new void Start () {
        base.Start();
        displayName = "Berry Bush";

        lootTable = new Item[3];

        lootTable[0] = new BerryItem(1);
        lootTable[1] = new BerryItem(2);
        lootTable[2] = new BerryItem(3);


        lootFrequencyTable = new int[3];
        lootFrequencyTable[0] = 20;
        lootFrequencyTable[1] = 60;
        lootFrequencyTable[2] = 20;

        numberOfItemsDropped = 1;
	}


    public override string collectMessage() {
        return "Hold [E] to collect berries from the bush";
    }


}

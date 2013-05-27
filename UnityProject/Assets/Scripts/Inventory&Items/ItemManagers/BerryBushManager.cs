using UnityEngine;
using System.Collections;


public class BerryBushManager : ItemManager {
	
	// Use this for initialization
	new void Start () {
        base.Start();
        displayName = "Berry Bush";

        lootTables = new Item[1][];
        lootTables[0] = new Item[3];

        lootTables[0][0] = new BerryItem(1);
        lootTables[0][1] = new BerryItem(2);
        lootTables[0][2] = new BerryItem(3);


        lootFrequencyTables = new int[1][];
        lootFrequencyTables[0] = new int[3];

        lootFrequencyTables[0][0] = 20;
        lootFrequencyTables[0][1] = 60;
        lootFrequencyTables[0][2] = 20;

        numberOfItemsDropped = new int[1];
        numberOfItemsDropped[0] = 1;
	}


    public override string collectMessage() {
        return "Hold [E] to collect berries from the bush";
    }


}

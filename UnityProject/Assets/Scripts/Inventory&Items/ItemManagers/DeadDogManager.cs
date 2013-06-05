using UnityEngine;
using System.Collections;


public class DeadDogManager : ItemManager {
	
	// Use this for initialization
	new void Start (){
        base.Start();
        displayName = "Dog Carcass";

        lootTables = new Item[2][];
        lootTables[0] = new Item[3];
        lootTables[1] = new Item[3];

        lootTables[0][0] = new SmallUncookedMeatItem(2);
        lootTables[0][1] = new SmallUncookedMeatItem(3);
        lootTables[0][2] = new LargeUncookedMeatItem(1);

        lootTables[1][0] = new SmallHideItem(2);
        lootTables[1][1] = new SmallHideItem(3);
        lootTables[1][2] = new SmallHideItem(4);

        lootFrequencyTables = new int[2][];
        lootFrequencyTables[0] = new int[3];
        lootFrequencyTables[1] = new int[3];

        lootFrequencyTables[0][0] = 40;
        lootFrequencyTables[0][1] = 40;
        lootFrequencyTables[0][2] = 20;

        lootFrequencyTables[1][0] = 20;
        lootFrequencyTables[1][1] = 60;
        lootFrequencyTables[1][2] = 20;

        numberOfItemsDropped = new int[2];
        numberOfItemsDropped[0] = 1;
        numberOfItemsDropped[1] = 1;
	}


}

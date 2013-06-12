using UnityEngine;
using System.Collections;


public class DeadLizardManager : ItemManager {
	
	// Use this for initialization
	new void Start (){
        base.Start();
        displayName = "Lizard Carcass";

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

        lootFrequencyTables[0][0] = 10;
        lootFrequencyTables[0][1] = 50;
        lootFrequencyTables[0][2] = 40;

        lootFrequencyTables[1][0] = 10;
        lootFrequencyTables[1][1] = 40;
        lootFrequencyTables[1][2] = 50;

        numberOfItemsDropped = new int[2];
        numberOfItemsDropped[0] = 1;
        numberOfItemsDropped[1] = 1;
	}


}

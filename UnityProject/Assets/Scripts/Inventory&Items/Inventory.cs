using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
	
/*
 * This class represents a players inventory. The inventory itself is a dictionary of items, where the keys are 
 * the names of an item (i.e. an inventory can contain only unique names), and the values are Item objects themselves.
 * The inventoryNamesArray is an array of strings which represents the order of the inventory. These strings can 
 * be used to index into the dictionary to access the actual Item object.
 */
public class Inventory : MonoBehaviour{
	
	private Dictionary<string,Item> inventory = new Dictionary<string,Item>(); //an underlying dictionary from name->Item
	private string[] inventoryNamesArray = new string[20]; //an array representing the order of the inventory.
	
	public int maxSize = 20;

    private EquipmentItem equippedEquipable;
    private EquipmentItem equippedSecondaryEquipable;
    private EquipmentItem equippedJacket;
    private EquipmentItem equippedBoots;

    public Dictionary<Item, Item[]> craftingDictionary;

    public GUIText InventoryTextGUI;
    public AudioClip collectSound;

    EquippedItemManager equippedItemScript;
    PlayerVitals vitals;
    GameObject textHints;

    int multiCraftCount = 0;


    void Start() {
        initializeCraftingDictionary();
        equippedItemScript = GameObject.Find("EquippedItem").GetComponent<EquippedItemManager>();

        vitals = gameObject.transform.root.GetComponent<PlayerVitals>();
        textHints = GameObject.Find("TextHintGUI");

        //EquipItem(new BowItem(1));
        //EquipItem(new ArrowItem(50));
        EquipItem(new KnifeItem(1));

    }

    void initializeCraftingDictionary() {
        craftingDictionary = new Dictionary<Item, Item[]>();

        craftingDictionary.Add(new SidePlankItem(2), new Item[] { new WoodItem(1) });
        craftingDictionary.Add(new RoofPlankItem(2), new Item[] { new WoodItem(1) });
        craftingDictionary.Add(new DoorItem(1), new Item[] { new WoodItem(10), new StoneItem(4) });
        craftingDictionary.Add(new FireItem(1), new Item[] { new WoodItem(3), new SharpStoneItem(2)});
        craftingDictionary.Add(new SmallCookedMeatItem(1), new Item[] { new SmallUncookedMeatItem(1) });
        craftingDictionary.Add(new LargeCookedMeatItem(1), new Item[] { new LargeUncookedMeatItem(1) });
        craftingDictionary.Add(new LargeHideItem(1), new Item[] { new SmallHideItem(3) });
        craftingDictionary.Add(new StringItem(2), new Item[] { new SmallHideItem(1) });
        craftingDictionary.Add(new BandageItem(1), new Item[] { new StringItem(1) });
        craftingDictionary.Add(new AxeItem(1), new Item[] { new SharpStoneItem(5), new WoodItem(3) });
        craftingDictionary.Add(new KnifeItem(1), new Item[] { new SharpStoneItem(2), new WoodItem(3) });
        craftingDictionary.Add(new BowItem(1), new Item[] { new WoodItem(4), new StringItem(4)});
        craftingDictionary.Add(new ArrowItem(10), new Item[] { new WoodItem(1), new SharpStoneItem(1) });
        craftingDictionary.Add(new BootsItem(1, 5), new Item[] { new SmallHideItem(2), new StringItem(6) });
        craftingDictionary.Add(new BootsItem(1, 10), new Item[] { new SmallHideItem(16), new StringItem(30) });
        craftingDictionary.Add(new JacketItem(1, 10), new Item[] { new LargeHideItem(3), new SmallHideItem(6), new StringItem(6) });
        craftingDictionary.Add(new JacketItem(1, 15), new Item[] { new LargeHideItem(8), new SmallHideItem(10), new StringItem(12) });
        craftingDictionary.Add(new JacketItem(1, 30), new Item[] { new LargeHideItem(15), new SmallHideItem(24), new StringItem(30) });
    }
	
	void Update () {
       

	}
	
	
	//add an item. Item types, quantity, etc. expected on the item
	public bool addItem(Item i, bool displayMessage=true){
        if (i == null) {
            return false;
        }


        if (i.quantity == 0) {
            return false;
        }

        //if this is a fire, just use it right away
        if (i is FireItem) {
            i.useItem();
            return true;
        }

       

		//if you already have this item in your inventory, utilize stacking
        if (inventory.ContainsKey(i.name)) {
            //Debug.Log(i.name + " in addItem q: " + i.quantity.ToString());
            Item oldItemInstance = inventory[i.name];
            oldItemInstance.quantity += i.quantity;
 
        } else if(i is EquipmentItem && equippedEquipable != null &&  i.name.Equals(equippedEquipable.name)){
            equippedEquipable.quantity += i.quantity;
        } else if(i is EquipmentItem && equippedSecondaryEquipable != null && i.name.Equals(equippedSecondaryEquipable.name)){
            equippedSecondaryEquipable.quantity += i.quantity;
        }else {

            

            //if the inventory is full, return false
            if (inventory.Count > maxSize) {
                return false;
            }


            //otherwise add the item to the dictionary
            inventory.Add(i.name, (Item)Activator.CreateInstance(i.GetType(), i));

            //find a location in the inventory array for it and add it to that as well
            for (int j = 0; j < inventoryNamesArray.Length; j++) {
                if (inventoryNamesArray[j] == null) {
                    inventory[i.name].inventoryLocation = j;
                    inventoryNamesArray[j] = i.name;
                    break;
                }
            }
        }
        if (displayMessage) {
            InventoryTextGUI.gameObject.GetComponent<InventoryTextManager>().enqueueMessage("You collected " + i.quantity + " " + ((i.quantity > 1) ? i.pluralName : i.name));
        }
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
		return true;
	}
	
	//remove an item from the inventory by name
	public bool removeItem(string name, int quantityToRemove){	
		
		//get the item out of the dictionary
		Item oldItemInstance = inventory[name];
		
		//if the item did not exist, return false
		if(oldItemInstance == null){
			return false;
		}
		
		//remove the quantity specified
		oldItemInstance.quantity -= quantityToRemove;
		
		//if you have no more of that item remaining, remove it from the hashset and name array
		if (oldItemInstance.quantity <= 0){
            inventory.Remove(oldItemInstance.name);
			inventoryNamesArray[oldItemInstance.inventoryLocation] = null;
		}
		
		return true;
			
	}

    //remove an item from the list by providing the item
    public bool removeItem(Item i) {
        return removeItem(i.name, i.quantity);
    }
	
	//checks if the given item name is contained in the array
	public bool contains(string itemName, int quantity = 0){
        Item item;
        if (!inventory.TryGetValue(itemName, out item)) {
            item = null;
        }
        return item != null && inventory[itemName].quantity >= quantity;
	}

    public Item getItem(string itemName) {
        if (contains(itemName)) {
            return inventory[itemName];
        } else {
            return null;
        }
    }
	
	//returns the item at a specific location in the inventory
	public Item getItemAtLocation(int location){
        string n = inventoryNamesArray[location];
        if (n == null) {
            return null;
        }

		return inventory[n];
	}
	
	//moves an item to a specific location in the inventory. Swaps if that location is alrady occupied.
	public bool moveItem(string itemName, int newLocation){
		//check to make sure the new location is in bounds
		if(newLocation >= this.maxSize){
			return false;
		}
		
		//check to make sure this itemName is in the inventory to begin with
		Item targetItem = inventory[itemName];
		if(targetItem == null){
			return false;
		}
		
		//save old values
		string oldItemInNewLocation = inventoryNamesArray[newLocation];
		int oldLocationOfItem = targetItem.inventoryLocation;
		//update target item
		inventoryNamesArray[newLocation] = itemName;
		targetItem.inventoryLocation = newLocation;
		
		//update old item in that slot, if it exists
		if(oldItemInNewLocation != null){
			inventoryNamesArray[oldLocationOfItem] = oldItemInNewLocation;
			inventory[oldItemInNewLocation].inventoryLocation = oldLocationOfItem;
		}
		
		return true;

	}

    public bool useItem(Item i) {
        bool used = i.useItem();

        //equipped items don't get decremented when they are "used" (equipped)
        if (!(i is EquipmentItem) && used){
            return removeItem(i.name, 1);
        }

        return true;
    }


    /*Crafting Code*/

    public bool canCraft(Item targetCraft) {
        Item[] craftIngredients = craftingDictionary[targetCraft];

        //special case: meat must be cooked near a fire. Is the player near a fire?
        if (targetCraft is SmallCookedMeatItem || targetCraft is LargeCookedMeatItem){
            if (!GameObject.FindWithTag("Player").GetComponent<PlayerVitals>().IsNearFire()) {
                return false;
            }
        }

        for (int i = 0; i < craftIngredients.Length; i++) {
            if (!this.contains(craftIngredients[i].name, craftIngredients[i].quantity)) {
                //Debug.Log("cannot craft " + craftIngredients[i].name);
                
                return false;
            }
        }
        return true;
    }

    public bool craftItem(Item targetCraft, bool multiCraft=false) {
        if (!canCraft(targetCraft)) {
            return false;
        }

        Item[] ingredientsList = craftingDictionary[targetCraft];

        for (int i = 0; i < ingredientsList.Length; i++) {

            //Try to remove each item
            if (!removeItem(ingredientsList[i])) {
                //if for whatever reason that fails (it shouldn't), add the already removed items back and return false
                for (int j = 0; j < i; j++) {
                    addItem(ingredientsList[j],false);
                }
                return false;
            }
        }

        //if you've made it this far, removing the ingredient items was successful. Add the crafted item and return true
        addItem(targetCraft, !multiCraft);
        if (multiCraft) {
            multiCraftCount += targetCraft.quantity;
            InventoryTextGUI.gameObject.GetComponent<InventoryTextManager>().updateMessage("You collected " + multiCraftCount + " " + ((multiCraftCount > 1) ? targetCraft.pluralName : targetCraft.name));
        }
        return true;
    }

    public void resetMultiCraftCount() {
        multiCraftCount = 0;
    }

    /*Equipment code*/

    public bool EquipItem(EquipmentItem i) {

        //first make sure that you aren't equipping slots with injured arms that you shouldn't be able to be
        if ((i.equipmentType == EquipmentItem.EquipmentType.equipable && vitals.bodyPartHealth[(int)PlayerVitals.BodyPart.RightArm] <= 0) ||
            (i.equipmentType == EquipmentItem.EquipmentType.secondary_equipable && vitals.bodyPartHealth[(int)PlayerVitals.BodyPart.LeftArm] <= 0)) {
                textHints.SendMessage("ShowHint", (i.equipmentType == EquipmentItem.EquipmentType.equipable) ? "You cannot equip that with a broken right arm!" : "You cannot equip that with a broken left arm!");
                return false;
        }

        //cut this item out of the inventory
        inventory.Remove(i.name);
        inventoryNamesArray[i.inventoryLocation] = null;

        EquipmentItem curEquipped;
        switch (i.equipmentType) {
            case EquipmentItem.EquipmentType.equipable:
                curEquipped = equippedEquipable;
                equippedEquipable = i;
                //Debug.Log(curEquipped.name + " " + curEquipped.quantity.ToString());
                addItem(curEquipped);


                if (i is AxeItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.Axe, i.statPower);
                } else if (i is SidePlankItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.Plank);
                } else if (i is RoofPlankItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.RoofPiece1);
                } else if (i is BowItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.Bow, i.statPower);
                } else if (i is KnifeItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.Knife, i.statPower);
                } else if (i is DoorItem) {
                    equippedItemScript.EquipItem((int)EquippedItemManager.Equippable.Door);
                }

                if (GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>().controlsAreDisabled) {
                    GameObject.Find("GUIButtons").GetComponent<OpenCloseGUIs>().disableNewItem();
                }

                break;
            case EquipmentItem.EquipmentType.secondary_equipable:
                curEquipped = equippedSecondaryEquipable;
                equippedSecondaryEquipable = i;
                addItem(curEquipped);
                
                break;

            case EquipmentItem.EquipmentType.jacket:
                curEquipped = equippedJacket;
                equippedJacket = i;
                addItem(curEquipped);
                
                break;

            case EquipmentItem.EquipmentType.boots:
                curEquipped = equippedBoots;
                equippedBoots = i;
                addItem(curEquipped);
                
                break;


        }

        return true;

    }

    public void unequipSlot(EquipmentItem.EquipmentType type) {
       
        EquipmentItem curEquipped;
        switch (type) {
            case EquipmentItem.EquipmentType.equipable:
                curEquipped = equippedEquipable;
                equippedEquipable = null;
                addItem(curEquipped);

                break;
            case EquipmentItem.EquipmentType.secondary_equipable:
                curEquipped = equippedSecondaryEquipable;
                equippedSecondaryEquipable = null;
                addItem(curEquipped);

                break;

            case EquipmentItem.EquipmentType.jacket:
              curEquipped = equippedJacket;
              equippedJacket = null;
                addItem(curEquipped);

                break;

            case EquipmentItem.EquipmentType.boots:
                curEquipped = equippedBoots;
                equippedEquipable = null;
                addItem(equippedBoots);

                break;

        }
    }

    public void decrementEquipable() {
        equippedEquipable.quantity--;
    }

    public void decrementSecondaryEquipable() {
        equippedSecondaryEquipable.quantity--;
    }

    public EquipmentItem getEquippedEquipable(){
        return equippedEquipable;
    }

    public EquipmentItem getEquippedSecondaryEquipable() {
        return equippedSecondaryEquipable;
    }

    public EquipmentItem getEquippedJacket() {
        return equippedJacket;
    }

    public EquipmentItem getEquippedBoots() {
        return equippedBoots;
    }





	
}

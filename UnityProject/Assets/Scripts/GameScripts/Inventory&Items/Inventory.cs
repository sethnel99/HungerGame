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

    public Dictionary<Item, Item[]> craftingDictionary;

    public GUIText StatusTextGUI;
    public AudioClip collectSound;


    void Start() {
        initializeCraftingDictionary();
	
    }

    void initializeCraftingDictionary() {
        craftingDictionary = new Dictionary<Item, Item[]>();

        craftingDictionary.Add(new FireItem(1), new Item[] { new SharpStoneItem(2), new WoodItem(3)});
        craftingDictionary.Add(new SmallCookedMeatItem(1), new Item[] { new SharpStoneItem(1) });
        craftingDictionary.Add(new LargeCookedMeatItem(1), new Item[] { new LargeUncookedMeatItem(1) });
        craftingDictionary.Add(new LargeHideItem(1), new Item[] { new SmallHideItem(3) });
        craftingDictionary.Add(new StringItem(2), new Item[] { new SmallHideItem(1) });
        craftingDictionary.Add(new AxeItem(1), new Item[] { new SharpStoneItem(2), new WoodItem(3) });
        craftingDictionary.Add(new SpearItem(1), new Item[] { new SharpStoneItem(1), new WoodItem(5) });
        craftingDictionary.Add(new BowItem(1), new Item[] { new WoodItem(4), new StringItem(4)});
        craftingDictionary.Add(new ArrowItem(1), new Item[] { new WoodItem(1) });
    }
	
	void Update () {

	}
	
	//void OnDrawGizmosSelected() {
	//Gizmos.DrawSphere(transform.position, 5);
	//}
	
	//add an item. Item types, quantity, etc. expected on the item
	public bool addItem(Item i){


        //if the inventory is full, return false
        if (inventory.Count > maxSize) {
            return false;
        }

		//if you already have this item in your inventory, utilize stacking
        if (inventory.ContainsKey(i.name)) {
            Item oldItemInstance = inventory[i.name];
            oldItemInstance.quantity += i.quantity;
        } else {
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

        StatusTextGUI.gameObject.GetComponent<StatusTextManager>().enqueueMessage("You collected " + i.quantity + " " + ((i.quantity > 1) ? i.pluralName : i.name));
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
		return true;
	}
	
	//remove an item from the list by name
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
            inventory.Remove(name);
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
	
	//returns the item at a specific location in the inventory
	public Item getItemAtLocation(int location){
		return inventory[inventoryNamesArray[location]];
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
        i.useItem();
        return removeItem(i.name, 1);
    }


    /*Crafting Code from here down*/

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

    public bool craftItem(Item targetCraft) {
        if (!canCraft(targetCraft)) {
            return false;
        }

        Item[] ingredientsList = craftingDictionary[targetCraft];

        for (int i = 0; i < ingredientsList.Length; i++) {

            //Try to remove each item
            if (!removeItem(ingredientsList[i])) {
                //if for whatever reason that fails (it shouldn't), add the already removed items back and return false
                for (int j = 0; j < i; j++) {
                    addItem(ingredientsList[j]);
                }
                return false;
            }
        }

        //if you've made it this far, removing the ingredient items was successful. Add the crafted item and return true
        addItem(targetCraft);
        return true;
    }


	
}

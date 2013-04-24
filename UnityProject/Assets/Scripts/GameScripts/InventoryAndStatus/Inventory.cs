using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
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
	
	
	void Update () {
		if(Input.GetButtonUp("Fire1")){
			Debug.Log (inventory.Count);
			Debug.Log (inventoryNamesArray[0]);
		}
	}
	
	public bool testFunc(){
		return true;
	}
	
	//add an item. Item types, quantity, etc. expected on the item
	public bool addItem(Item i){
		
		//if you already have this item in your inventory, utilize stacking
		if(inventory.ContainsKey(i.name)){
			Item oldItemInstance = inventory[i.name];
			oldItemInstance.quantity += i.quantity;
			return true;
		}
		
		//if the inventory is full, return false
		if(inventory.Count > maxSize){
			return false;
		}
		
		//otherwise add the item to the dictionary
		inventory.Add(i.name,i);
		
		//find a location in the inventory array for it and add it to that as well
		for(int j = 0; j < inventoryNamesArray.Length; j++){
			if(inventoryNamesArray[j] == null){
				i.inventoryLocation = j;
				inventoryNamesArray[j] = i.name;
				break;
			}
		}
		
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
			inventory[name] = null;
			inventoryNamesArray[oldItemInstance.inventoryLocation] = null;
		}
		
		return true;
			
	}
	
	//checks if the given item name is contained in the array
	public bool contains(string itemName){
		for(int i = 0; i < inventoryNamesArray.Length; i++){
			if(inventoryNamesArray[i].Equals (itemName)){
				return true;
			}
		}
		return false;
	}
	
	//returns the item at a specific location in the inventory
	public Item get(int location){
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
	
}

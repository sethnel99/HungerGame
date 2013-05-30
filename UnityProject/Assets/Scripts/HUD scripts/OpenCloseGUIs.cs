using UnityEngine;
using System.Collections;

public class OpenCloseGUIs : MonoBehaviour {
    public CraftingGUI craftingGUI;
    public IGUI inventoryGUI;

    float debounceTime = .25f;
    float debounceTimer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (debounceTimer > debounceTime) {
            debounceTimer = 0f;
        } else if (debounceTimer > 0f) {
            debounceTimer += Time.deltaTime;
            return;
        }else if (Input.GetButton("CraftingGUI")) {
            if (craftingGUI.enabled) {
                enableControls(craftingGUI);
                craftingGUI.enabled = false;
            } else {
                disableControls(craftingGUI);
                craftingGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("InventoryGUI")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                inventoryGUI.enabled = false;
            } else {
                disableControls(inventoryGUI);
                inventoryGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("esc")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                inventoryGUI.enabled = false;
            } else if (craftingGUI.enabled) {
                enableControls(craftingGUI);
                craftingGUI.enabled = false;
            }
        }
	}

    public void disableControls(MonoBehaviour disabler) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
	
        //Debug.Log("disable controls");
       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",true);
		
        if (disabler is CraftingGUI){
            inventoryGUI.enabled = false;
        }else if (disabler is InventoryGUI){
            craftingGUI.enabled = false;
        }else{
			inventoryGUI.enabled = false;
			craftingGUI.enabled = false;
		}
    }

    public void enableControls(MonoBehaviour enabler) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
        GameObject.Find("EquippedItem").GetComponent<EquippedItem>().enabled = true;

        //Debug.Log("enable controls");

       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",false);
		

    }

    public void disableNewItem() {
       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",true);
		
    }
	
}

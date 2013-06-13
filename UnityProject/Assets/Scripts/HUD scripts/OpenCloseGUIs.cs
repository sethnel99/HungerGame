using UnityEngine;
using System.Collections;

public class OpenCloseGUIs : MonoBehaviour {
    public CraftingGUI craftingGUI;
    public IGUI inventoryGUI;
	public EquippableGUI equipmentGUI;

    float debounceTime = .25f;
    float debounceTimer = 0f;

    public bool controlsAreDisabled = false;

	// Use this for initialization
	void Start () {
        lockCursor();
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
                setCraftingEnabled(false);
            } else {
                disableControls(craftingGUI);
                setCraftingEnabled(true);
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("InventoryGUI")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                setInventoryEnabled(false);
            } else {
                disableControls(inventoryGUI);
                 setInventoryEnabled(true);
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetButton("esc")) {
            if (inventoryGUI.enabled) {
                enableControls(inventoryGUI);
                setInventoryEnabled(false);
            } else if (craftingGUI.enabled) {
                enableControls(craftingGUI);
                setCraftingEnabled(false);
            }
        } else if (Input.GetButton("left_ctrl")) {
            if (Screen.lockCursor) {
                unlockCursor();
            } else {
                lockCursor();
            }
            debounceTimer += Time.deltaTime;
        } else if (Input.GetKeyDown (KeyCode.T)) {
            if (equipmentGUI.enabled) {
                enableControls(equipmentGUI);
                setEquipmentEnabled(false);
            } else {
                disableControls(equipmentGUI);
                setEquipmentEnabled(true);
            }
		}

	}

    public void disableControls(MonoBehaviour disabler) {
        controlsAreDisabled = true;

        unlockCursor();
       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",true);
		
        if (disabler is CraftingGUI){
            setInventoryEnabled(false);
			setEquipmentEnabled(false);
        }else if (disabler is InventoryGUI){
            setCraftingEnabled(false);
			setEquipmentEnabled(false);
        }else if (disabler is EquippableGUI){
			setCraftingEnabled(false);
			setInventoryEnabled(false);
		}
		else{
            setInventoryEnabled(false);
            setCraftingEnabled(false);
			setEquipmentEnabled(false);
		}
    }

    public void enableControls(MonoBehaviour enabler) {
        controlsAreDisabled = false;

        //GameObject.Find("EquippedItem").GetComponent<EquippedItemManager>().enabled = true;
        //Debug.Log("enable controls");
        lockCursor();
       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",false);
		

    }

    public void disableNewItem() {
       GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("DisableByGUI",true);
		
    }

    public void lockCursor() {
        Screen.lockCursor = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
    }

    public void unlockCursor() {
        Screen.lockCursor = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
    }

    public void setInventoryEnabled(bool e) {
        inventoryGUI.enabled = e;
    }

    public void setCraftingEnabled(bool e) {
        craftingGUI.enabled = e;
    }
	
	public void setEquipmentEnabled(bool e) {
        equipmentGUI.enabled = e;
    }
}

using UnityEngine;
using System.Collections;

public class InventoryButton : MonoBehaviour
{
    private IGUI InventoryDisplay;

	
	// Use this for initialization
	void Start ()
	{
        InventoryDisplay = GameObject.Find("InventoryGUI").GetComponent<IGUI>();
        InventoryDisplay.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	void OnMouseDown()
	{
		InventoryDisplay.enabled = !InventoryDisplay.enabled;
        if (InventoryDisplay.enabled) {
            disableControls();
        } else {
            enableControls();
        }
	}

    public void disableControls() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
    }

    public void enableControls() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
    }
}


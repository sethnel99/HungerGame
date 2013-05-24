using UnityEngine;
using System.Collections;

public class CraftingButton : MonoBehaviour
{
    private CraftingGUI craftingDisplay;

	
	// Use this for initialization
	void Start ()
	{
        craftingDisplay = GameObject.Find("CraftingGUI").GetComponent<CraftingGUI>();
        craftingDisplay.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	void OnMouseDown()
	{
		craftingDisplay.enabled = !craftingDisplay.enabled;
        if (craftingDisplay.enabled) {
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


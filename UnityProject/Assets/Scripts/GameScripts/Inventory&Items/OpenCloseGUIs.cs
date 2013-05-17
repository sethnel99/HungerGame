using UnityEngine;
using System.Collections;

public class OpenCloseGUIs : MonoBehaviour {
    public CraftingGUI craftingGUI;

    float debounceTime = .5f;
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
                enableControls();
                craftingGUI.enabled = false;
            } else {
                disableControls();
                craftingGUI.enabled = true;
            }
            debounceTimer += Time.deltaTime;
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

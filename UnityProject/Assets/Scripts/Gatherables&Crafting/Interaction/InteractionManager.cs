using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour {
    public GameObject interactTarget;
    HashSet<GameObject> potentialInteractors;
    public GUIText interactText;

	// Use this for initialization
	void Start () {
        potentialInteractors = new HashSet<GameObject>();
	}

    public void addPotentialInteractor(GameObject g) {
        potentialInteractors.Add(g);
    }

    public void removePotentialInteractor(GameObject g) {
        if (potentialInteractors.Contains(g)) {
            potentialInteractors.Remove(g);
        }
    }
	
	// Update is called once per frame
	void Update () {
        interactTarget = findClosestInteractTarget();
        if (interactTarget != null) {
            interactText.text = interactTarget.GetComponent<ItemManager>().collectMessage();
            //Debug.Log(interactText.text);
        } else {
            interactText.text = "";
        }
	}

    GameObject findClosestInteractTarget() {
        Vector3 currentPlayerLoc = transform.root.transform.position;
        float minDistance = float.MaxValue;
        GameObject minObject = null;

        if (potentialInteractors.Count == 0) {
            return null;
        }

        foreach (GameObject g in potentialInteractors) {
            float distanceFromPlayer = Vector3.Distance(currentPlayerLoc, g.transform.position);
            if (distanceFromPlayer < minDistance) {
                minDistance = distanceFromPlayer;
                minObject = g;
            }
        }

        return minObject;
    }
}

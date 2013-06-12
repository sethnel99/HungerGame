using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

    public Texture2D instructionLayover;
    Rect fullScreenRect;
    GameObject player;

	// Use this for initialization
	void Start () {
        fullScreenRect = new Rect(0f, 0f, Screen.width, Screen.height);
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            Debug.Log("MOVE THE FUCKING INSTRUCTIONS");
            player.GetComponent<CharacterController>().enabled = true;

            Destroy(this);
        }
	}

    void OnGUI() {

        GUI.BeginGroup(fullScreenRect);
        GUI.DrawTexture(fullScreenRect, instructionLayover);
        GUI.EndGroup();

    }
}

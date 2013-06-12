using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

    public Texture2D instructionLayover;
    public Texture2D surviveLayover;
    public AudioClip surviveSound;

    Rect fullScreenRect;
    GameObject player;

    public bool surviveText = false;
    public float surviveTextLength = 5f;

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
            surviveText = true;
            AudioSource.PlayClipAtPoint(surviveSound, player.transform.position);
        }

        if (surviveText) {
            surviveTextLength -= Time.deltaTime;

            if (surviveTextLength <= 0f) {
                Destroy(this);
            }
        }
	}

    void OnGUI() {

        GUI.BeginGroup(fullScreenRect);

        if (surviveText) {
            GUI.DrawTexture(fullScreenRect, surviveLayover);
        } else {
            GUI.DrawTexture(fullScreenRect, instructionLayover);
        }

        GUI.EndGroup();

    }
}

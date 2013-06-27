using UnityEngine;
using System.Collections;

public class InventoryTextManager : MonoBehaviour {
    float timer = 0.0f;
    public float displayLength = 2.0f;
    public float breakLength = 0.3f;

    Queue messageQueue = new Queue();
    bool downTime = false;

	// Use this for initialization
	void Start () {
	
	}

    void Update() {

        if (guiText.enabled) {
            timer += Time.deltaTime;
            //Debug.Log("Timer: " + timer.ToString() + " downTime: " + downTime.ToString());

            //if either the message has been displayed long enough, or we are done with the break in-between messages
            if ((timer >= displayLength && !downTime) || (timer >= breakLength && downTime)) {

                if (!downTime) {
                    downTime = true;
                    guiText.text = "";
                }else if(messageQueue.Count == 0) {
                    guiText.enabled = false;
                    downTime = false;
                } else {
                    downTime = false;
                    guiText.text = (string)messageQueue.Dequeue();
                } 

                timer = 0.0f;
            }
        }
    }

    public void enqueueMessage(string message) {
        if (guiText.enabled) {
            //Debug.Log("enqueing message: " + message);
            messageQueue.Enqueue(message);
        } else {
            //Debug.Log("just putting it up there");
            guiText.enabled = true;
            guiText.text = message;
        }
    }

    public void updateMessage(string message) {
        if (!guiText.enabled) {
            guiText.enabled = true;
        }
        timer = Time.deltaTime;
        guiText.text = message;
        downTime = false;
    }

}

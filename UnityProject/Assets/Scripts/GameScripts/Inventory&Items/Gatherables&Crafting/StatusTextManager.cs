using UnityEngine;
using System.Collections;

public class StatusTextManager : MonoBehaviour {
    float timer = 0.0f;
    public float displayLength = 1.0f;
    public float breakLength = 0.3f;

    Queue messageQueue = new Queue();
    bool downTime = false;

	// Use this for initialization
	void Start () {
	
	}

    void Update() {

        if (guiText.enabled && displayLength > 0) {
            timer += Time.deltaTime;

            //if either the message has been displayed long enough, or we are done with the break in-between messages
            if ((timer >= displayLength && !downTime) || (timer > breakLength && downTime)) {
                if (messageQueue.Count == 0) {
                    guiText.enabled = false;
                } else if (downTime) {
                    downTime = false;
                    guiText.text = (string)messageQueue.Dequeue();
                } else {
                    downTime = true;
                    guiText.text = "";
                }

                timer = 0.0f;
            }
        }
    }

    public void enqueueMessage(string message) {
        if (guiText.enabled) {
            messageQueue.Enqueue(message);
        } else {
            guiText.enabled = true;
            guiText.text = message;
        }
    }

}

using UnityEngine;
using System.Collections;

public class Swimming : MonoBehaviour {
	
	public GameObject SwimGoggle;
	GameObject goggle;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Water"))
		{
			//Debug.Log("SWIM SEND");
			SendMessage("IsSwimming", true);
			
			goggle = Instantiate(SwimGoggle, Camera.main.transform.position, Camera.main.transform.rotation) as GameObject;
			goggle.transform.parent = Camera.main.transform;
			goggle.transform.Translate(new Vector3(0f, 0f,1f));
			goggle.transform.Rotate(new Vector3(0f, 90f,0f));
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Water"))
		{
			SendMessage("IsSwimming", false);
			
			Destroy(goggle);
		}
	}
}

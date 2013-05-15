using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	public string HintString;
	
	Vector3 rot = new Vector3(5f, 5f, 5f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(rot);
	}
	
	public GUIText textHints;
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			
			EquippedItem equipScript = col.gameObject.GetComponentInChildren<EquippedItem>() as EquippedItem;
			if(!equipScript.hasFirstStar)
			{
				equipScript.EquipItem((int)EquippedItem.Equippable.Plank);
				equipScript.hasFirstStar = true;
			}
//			if(equipScript.stars == 0)
//			{
				textHints.SendMessage("ShowHint",
						HintString);
//						"You can now build by placing planks!\nGo outside and build a house-like structure.\n" +
//						"Let the devs know when you are happy with what you have built.");
//			}
//			else if(equipScript.stars == 1)
//			{
//				textHints.SendMessage("ShowHint",
//						"That wasn't too hard now was it?\nThanks for playing!");
//			}
//			else if(equipScript.stars == 2)
//			{
//				equipScript.hasSecondStar=true;
//				textHints.SendMessage("ShowHint",
//						"That wasn't too hard now was it?\nThanks for playing!");
//			}

			col.gameObject.GetComponent<TestInventory>().Win();
			

			Destroy(gameObject);
		}
	}
}

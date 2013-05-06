using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedItem : MonoBehaviour {
	
	public enum Equippable{
		Axe,
		MAX
	};
	
	bool isInAction = false;
	
	public GameObject[] equippableItems;
	public GameObject equippedItem;
	
	private Vector3[] equippableTransform;
	private Vector3[] equippableRotate;
	
	// Use this for initialization
	void Start () {
		equippableTransform = new Vector3[(int)Equippable.MAX];
		equippableRotate = new Vector3[(int)Equippable.MAX];
		
		equippableTransform[(int)Equippable.Axe] = new Vector3(.5f,-1f,.2f);
		equippableRotate[(int)Equippable.Axe] = new Vector3(15,95,0);
		
		EquipItem((int)Equippable.Axe);
	}
 
IEnumerator COAction() {
    isInAction = true;
    //yield return new WaitForSeconds(0.1f); // trigger delay
    // create bullet and flash effect...
	equippedItem.animation.Play("axeSwing1");
    //yield return 0; // wait 1 frame
    // stop flash
    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
    audio.PlayOneShot(testSound);
    yield return new WaitForSeconds(0.8f); // extra delay before you can shoot again
    isInAction = false;
}
	
	public AudioClip testSound;
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && !isInAction){
			StartCoroutine(COAction());
			//audio.PlayOneShot(testSound);
			
		}
	}
	
	void EquipItem (int equippable){
		//+ equippableTransform[equippable]
		//Debug.Log(equippableItems.Length);
		equippedItem = Instantiate(equippableItems[equippable], transform.position+equippableTransform[equippable], Quaternion.identity) as GameObject;
		equippedItem.transform.Rotate(equippableRotate[equippable]);
		equippedItem.transform.parent = gameObject.transform;
		equippedItem.transform.localScale = Vector3.one;
		
		//equippedItem.transform.position = new Vector3(0, -.8f, 0);
	}
}

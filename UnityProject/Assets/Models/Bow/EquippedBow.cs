using UnityEngine;
using System.Collections;

public class EquippedBow : EquippedItem {
		

    Inventory inventory;
	public GameObject Arrow;
	
	new void Start () {
        base.Start();
        damage = 8f;
	}

    protected override void Update() {
        EquipmentItem secondaryEquipped = inventory.getEquippedSecondaryEquipable();

		if(Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI && secondaryEquipped is ArrowItem && secondaryEquipped.quantity > 0){
			StartCoroutine(COAction());
            secondaryEquipped.quantity--;
        } else if (Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI && secondaryEquipped is ArrowItem && secondaryEquipped.quantity <= 0) {
            textHints.SendMessage("ShowHint", "You are out of arrows!");
        } else if (Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI && !(secondaryEquipped is ArrowItem)) {
            textHints.SendMessage("ShowHint", "You must have an arrow equipped to shoot your bow!");
        }
	}
	
	public AudioClip shoot1Sound;
	//NEED TO REDO THESE TIMINGS
	IEnumerator COAction() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f);
		gameObject.animation.Play("Bow_Shoot");
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(shoot1Sound);
		GameObject arrow = (GameObject)Instantiate(Arrow, transform.position,transform.rotation);
        arrow.SendMessage("SetDamage",damage);
		arrow.transform.Rotate(0f, 90f, 0f);
		arrow.rigidbody.AddForce(.00011f*transform.parent.transform.forward);
		yield return new WaitForSeconds(0.8f); // extra delay before you can shoot again
	    isInAction = false;

       
        //for recollecting arrows
        arrow.AddComponent<ArrowManager>();
        arrow.AddComponent<SphereCollider>();
        arrow.GetComponent<SphereCollider>().isTrigger = true;
        arrow.GetComponent<SphereCollider>().radius = 3;
	}

    public override void Hit() {
        //method stub, because the bow itself doesn't actually hit anything
    }

}
using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	
	protected Item[] lootTable; //the possible drops from this object
    protected int[] lootFrequencyTable; //the frequencies of these drops (must add up to 100)
    protected int numberOfItemsDropped;

    public string displayName;

	bool inZone;
	float buttonDownTime;
	public float gatherTime = 1.0f;


	public GameObject interactionTimer;

    InteractionManager interactionManager;
	
	// Use this for initialization
	public void Start () {
		inZone = false;
		buttonDownTime = 0.0f;
       
        interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>();

        displayName = "TestObject";

        lootTable = new Item[0];
        lootFrequencyTable = new int[0];
       

        numberOfItemsDropped = 0;
	}


    // Update is called once per frame
    void Update() {
       
		if (inZone && Input.GetButton ("Interact")) {
			buttonDownTime += Time.deltaTime;
            //Debug.Log(buttonDownTime / gatherTime);
            if (!interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(buttonDownTime / gatherTime, gameObject)) {
                buttonDownTime = 0f;
            }
			
			if (buttonDownTime > gatherTime) {
				GameObject player  = GameObject.FindWithTag("Player");

                Item[] drops = chooseLoot();

                for (int i = 0; i < drops.Length; i++) {
                   
                    player.GetComponent<Inventory>().addItem(drops[i]);
                }

                interactionManager.removePotentialInteractor(gameObject);
                interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0, gameObject);
                gameObject.transform.parent.SendMessage("removeNode");
			}	
		}
		else {
			buttonDownTime = 0.0f;
            interactionTimer.GetComponent<InteractionTimer>().setInteractionTimerLevel(0, gameObject);
		}
	}
	
	
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = true;
            interactionManager.addPotentialInteractor(gameObject);
		}
	}
	
	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = false;
            interactionManager.removePotentialInteractor(gameObject);
		}
		
	}

    public virtual Item[] chooseLoot(){

        Item[] drops = new Item[numberOfItemsDropped];

        for(int i = 0; i < numberOfItemsDropped; i++){
            int roll = Random.Range(1, 101);
            
            int c = 0;
            for (int j = 0; j < lootFrequencyTable.Length; j++) {
                c += lootFrequencyTable[j];
                if (roll < c) {
                    drops[i] = lootTable[j];
            
                    break;
                }
            }
        }

        return drops;
    }

    public virtual string collectMessage() {
        return "Hold [E] to pick up the " + displayName;
    }
    
}

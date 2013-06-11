using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemManager : MonoBehaviour {
	
	protected Item[][] lootTables; //the possible drops from this object
    protected int[][] lootFrequencyTables; //the frequencies of these drops (must add up to 100)
    protected int[] numberOfItemsDropped;

    public string displayName;

	protected bool inZone;
	protected float buttonDownTime;
	public float gatherTime = 1.0f;


	protected InteractionTimer interactionTimer;

    protected InteractionManager interactionManager;
	
	// Use this for initialization
	virtual public void Start () {
		inZone = false;
		buttonDownTime = 0.0f;
        //Debug.Log(GameObject.FindGameObjectWithTag("Player"));
        //Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>());
        interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>();
        interactionTimer = GameObject.Find("InteractionTimer").GetComponent<InteractionTimer>();

        displayName = "TestObject";

        lootTables = new Item[0][];
        lootFrequencyTables = new int[0][];
       

        numberOfItemsDropped = new int[0];
	}


    // Update is called once per frame
    virtual public void Update() {
       
		if (inZone && Input.GetButton ("Interact")) {
			buttonDownTime += Time.deltaTime;
            //Debug.Log(buttonDownTime / gatherTime);
            if (!interactionTimer.setInteractionTimerLevel(buttonDownTime / gatherTime, gameObject)) {
                buttonDownTime = 0f;
            }
			
			if (buttonDownTime > gatherTime) {
				GameObject player  = GameObject.FindWithTag("Player");

                Item[] drops = chooseLoot();

                for (int i = 0; i < drops.Length; i++) {
                   
                    player.GetComponent<Inventory>().addItem(drops[i]);
                }

                interactionManager.removePotentialInteractor(gameObject);
                interactionTimer.setInteractionTimerLevel(0, gameObject);

                //shitty hacky check until i create a node for enemies
                if (gameObject.transform.parent == null) {
                    GameObject.Destroy(this.gameObject);
                } else {
                    gameObject.transform.parent.SendMessage("removeNode");
                }
			}	
		}
		else {
			buttonDownTime = 0.0f;
            interactionTimer.setInteractionTimerLevel(0, gameObject);
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
            if (this.gameObject == interactionManager.interactTarget) {
                interactionTimer.setInteractionTimerLevel(0, gameObject);
            }
            interactionManager.removePotentialInteractor(gameObject);
		}
		
	}

    public virtual Item[] chooseLoot(){
        int totalNumItem = numberOfItemsDropped.Sum(c => c);
        Item[] drops = new Item[totalNumItem];

        int dropIndex = 0;

        for(int i = 0; i < lootTables.Length; i++){
            for (int j = 0; j < numberOfItemsDropped[i]; j++) {
                int roll = Random.Range(1, 101);

                int c = 0;
                for (int k = 0; k < lootFrequencyTables[i].Length; k++) {
                    c += lootFrequencyTables[i][k];
                    if (roll < c) {
                        drops[dropIndex++] = lootTables[i][k];
                        break;
                    }
                }
            }
            
            
        }

        return drops;
    }

    public virtual string collectMessage() {
        return "Hold [E] to pick up the " + displayName;
    }
    
}

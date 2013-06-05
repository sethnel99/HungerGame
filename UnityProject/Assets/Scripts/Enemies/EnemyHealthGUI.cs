using UnityEngine;
using System.Collections;

public class EnemyHealthHUD : MonoBehaviour {

    public GUISkin enemyHealthHUDSkin;
    public Rect enemyHealthRect;
    Rect inventoryRectNormalized;

    GameObject enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        if (enemy != null) {
            

        }
    }

    public void setEnemy(GameObject e) {

    }

    
}

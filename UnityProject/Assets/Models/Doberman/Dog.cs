using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class Dog : MonoBehaviour, IAgent 
{
	Tree m_tree;
	public GameObject player;
	public GameObject bite;
	
	private bool inAnimation; // are we already animating for a behavior?
	
	private float TickTime =0f; // time in between each AI tick
	
	private bool doneLooking = false;
	private float lookTimer = 0f;
	private float lookMaxTime = 5f;

	private bool doneWandering = false;
	private float wanderTimer = 0f;
	private float wanderMaxTime = 20f;

    private bool dead = false;
	
	// AI Vars
	float sightCastRadius = 6f;
	float sightDistance = 30;
	float alignRadius = 5f;
	float attackRange = 3f;
    float senseRange = 20;
    float leashDistance = 45;

    public bool leashingBackToSpawn = false;

	public bool senseSomething =false;
	public bool seeTarget = false;
	public bool closeEnough = false;
	public bool aligned = false;
	// /AI Vars
	
	/*/Points to randomly go to
	private Vector3[] PatrolPoints = new Vector3[3]{
		 new Vector3(299.5799f, 71.53264f, -957.917f),
		 new Vector3(333.1228f, 69.27077f, -963.5134f),
		 new Vector3(336.9609f, 70.33675f, -991.8299f)
	};*/
	//private int PatrolDestination;
	private Vector3[] PatrolPoints = new Vector3[3];
    private Vector3 spawnPoint;

	private float takingHitTimer = 0f;
	private float takingHitTimerMax = 1f;
	
	IEnumerator Start () {
        spawnPoint = this.gameObject.transform.position;
        PatrolPoints[0] = spawnPoint + new Vector3(Random.Range(5,8), Random.Range(5,8), 0);
        PatrolPoints[1] = spawnPoint + new Vector3(Random.Range(-8,-5), Random.Range(5,8), 0);
        PatrolPoints[2] = spawnPoint + new Vector3(Random.Range(5,8), Random.Range(-8,-5), 0);

        player = GameObject.FindGameObjectWithTag("Player");
		
		m_tree = BLBehaveLibrary0.InstantiateTree(BLBehaveLibrary0.TreeType.EnemyBehaviors_PatrolerTree,this);
		while(Application.isPlaying && m_tree != null)
		{	
			yield return new WaitForSeconds(1.0f / m_tree.Frequency/2);
			AIUpdate();
		}
	}
	
	void AIUpdate()
	{
		RunChecks();
        if (m_tree != null) {
            m_tree.Tick();
        }
		TickTime = 0; //set our AI tick timer to 0 b/c we just ticked
	}
	
	void Update () {
		TickTime += Time.deltaTime;
		if(takingHitTimer>0)
		{
			takingHitTimer -= Time.deltaTime;
		}
	}
	
	// IAgent
	public BehaveResult Tick (Tree sender, bool init)
	{
		return BehaveResult.Success;
	}
	public void Reset(Tree sender)
	{
		
	}
	public int SelectTopPriority(Tree sender, params int[] IDs)
	{
		return IDs[0];
	}
	// /IAgent
	
	public BehaveResult TickExitIfDecorator(Tree sender, string exp, float floatParameter, IAgent agent, object data)
	{
		if(EvaluateExpression(exp))
		{
			inAnimation = false;
			return BehaveResult.Failure;
		}
		{
			return BehaveResult.Success;
		}
	}
	
//	public BehaveResult TickCheckAction(Tree sender)
//	{
//		RunChecks();
//		
//		return BehaveResult.Success;	
//	}
	
	void RunChecks()
	{
        float distanceFromSpawn = Vector3.Distance(new Vector3(transform.position.x,transform.position.y,0), new Vector3(spawnPoint.x,spawnPoint.y,0));
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log("distance from spawn: " + distanceFromSpawn);
        //leash to spawn
        if (distanceFromSpawn > leashDistance || (seeTarget && distanceFromPlayer > leashDistance) || (leashingBackToSpawn && distanceFromSpawn > 3.0f)) {
            senseSomething = false;
            seeTarget = false;
            closeEnough = false;
            aligned = false;
            leashingBackToSpawn = true;
            Debug.Log("leashing back to spawn");
            return;
        } else if (leashingBackToSpawn && distanceFromSpawn < 3.0f) {
            gameObject.animation.CrossFade("Stand_Idle");
            inAnimation = false;
            leashingBackToSpawn = false;
        }



		senseSomething = distanceFromPlayer <= senseRange;
		if(senseSomething)
		{

            if (!seeTarget) {
                RaycastHit[] hits= Physics.SphereCastAll(transform.position-10*transform.forward, sightCastRadius, transform.forward, sightDistance);
                foreach (RaycastHit hit in hits) {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
                        seeTarget = true;
                    }
                }
            }

			if(seeTarget)
			{	
				closeEnough = distanceFromPlayer <= attackRange;
                Vector3 dirToPlayer = Vector3.Normalize(player.transform.position - transform.position);
                dirToPlayer.y = 0;
                Vector3 dogForward = this.gameObject.transform.forward;
                dogForward.y = 0;
                aligned = Vector3.Angle(dirToPlayer, dogForward) < 20;
                //Debug.Log("Dog to player: " + dirToPlayer + " Dog forward: " + dogForward + " Angle: " + Vector3.Angle(dirToPlayer, dogForward));
			}
			else
			{
				closeEnough = false;
				aligned = false;
			}
		}
		else
		{
			closeEnough = false;
			aligned = false;
		}

        //Debug.Log("Sense: " + senseSomething + " See Target: " + seeTarget + " close Enough: " + closeEnough + " aligned: " + aligned);

	}
	
	
	public BehaveResult InitLookAroundAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitWanderAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitAttackAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}
	public BehaveResult InitCloseDistanceAndAlignAction(Tree sender)
	{
		inAnimation = false;
		return BehaveResult.Running;
	}

    public BehaveResult InitLeashBackToSpawnAction(Tree sender) {
        inAnimation = false;
        return BehaveResult.Running;
    }

	
	public BehaveResult TickLookAroundAction(Tree sender)
	{
		gameObject.SendMessage("StopMoving","Look Around");
		if(!doneLooking) //Haven't found a target and aren't done durdling
		{
			lookTimer += TickTime;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			if(!inAnimation) //If we aren't already animating for this behavior, start
			{/*
				System.Random rnd = new System.Random();
				int randomTwitch = rnd.Next(0, 3);
				if(randomTwitch==1)
				{
					StartCoroutine(COTwitch());
				}
				else if(randomTwitch ==2)
				{
					StartCoroutine(COBark());
				}*/
			}
			if(lookTimer > lookMaxTime)
			{
				doneLooking = true;
			}
			
			return BehaveResult.Running;
		}
		else
		{
			lookTimer = 0;
			doneLooking = false;
			inAnimation = false;
			return BehaveResult.Success;
		}
		
	}
	
	public BehaveResult TickWanderAction(Tree sender)
	{
		if(!doneWandering)
		{
			wanderTimer += TickTime;
			
			if(!inAnimation) //If we aren't already animating for this behavior, start
			{
				System.Random rnd = new System.Random();
				int randomPatrolPoint = rnd.Next(0, PatrolPoints.Length);

                //going to the same place? twitch instead
                if (Vector3.Distance(PatrolPoints[randomPatrolPoint], gameObject.transform.position) < 3) {
				    int randomTwitch = rnd.Next(0, 3);
				    if(randomTwitch==1)
				    {
				    	StartCoroutine(COTwitch());
				    }
				    else if(randomTwitch ==2)
				    {
				    	StartCoroutine(COBark());
				    }
                }

				gameObject.SendMessage("StartMovingTo", PatrolPoints[randomPatrolPoint]);
				StartCoroutine(COWander());
			}
			
			if(wanderTimer > wanderMaxTime)
			{
				gameObject.SendMessage("StopMoving","TickWanderActionTimeOut");
				doneWandering = true;
				gameObject.animation.CrossFade("Walk_End_1");
			}
			return BehaveResult.Running;
		}
		else
		{
			gameObject.SendMessage("StopMoving","TickWanderActionBottom");
			wanderTimer = 0;
			doneWandering = false;
			inAnimation = false;
			return BehaveResult.Success;
		}
	}
	
	public BehaveResult TickAttackAction(Tree sender)
	{
		transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
		if(!inAnimation) //If we aren't already animating for this behavior, start
		{
			StartCoroutine(COAttack());
		}
		return BehaveResult.Running;
	}
	
	public BehaveResult TickCloseDistanceAndAlignAction(Tree sender)
	{
        //Debug.Log("CDAA");
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        gameObject.SendMessage("StartMovingTo", player.transform.position - 1 * player.transform.forward);
		if(!inAnimation) //If we aren't already animating for this behavior, start
		{
			StartCoroutine(COChase());
		}
		
		return BehaveResult.Running;
	}

    public BehaveResult TickLeashBackToSpawnAction(Tree sender) {
  
        //Debug.Log("current pos: " + this.gameObject.transform.position + " spawn point: " + spawnPoint);
        gameObject.SendMessage("StartMovingTo", spawnPoint);
        if (!inAnimation) //If we aren't already animating for this behavior, start
		{
            StartCoroutine(COChase());
        }

        return BehaveResult.Running;
    }
	
	
	IEnumerator COBark() {
		inAnimation = true;
		gameObject.animation.CrossFade("Stand_Idle");
	    yield return new WaitForSeconds(.5f);
	    gameObject.animation.CrossFade("Bark_1");
		yield return new WaitForSeconds(1f);
		gameObject.animation.CrossFade("Stand_Idle");
		inAnimation = false;
	}
	IEnumerator COTwitch() {
		inAnimation = true;
		gameObject.animation.CrossFade("Stand_Idle");
	    yield return new WaitForSeconds(.5f);
	    gameObject.animation.CrossFade("Idle_Twitch_1");
	    yield return new WaitForSeconds(2f);
		gameObject.animation.CrossFade("Stand_Idle");
		inAnimation = false;
	}
	
	IEnumerator COWander() {
		inAnimation = true;
		gameObject.SendMessage("SetSpeed", 1f);
	    gameObject.animation.CrossFade("Walk_Begin_1");
	    yield return new WaitForSeconds(.2f);
	    gameObject.animation.CrossFade("Walk_Loop_1");
	}
	IEnumerator COChase() {
		inAnimation = true;
		gameObject.SendMessage("SetSpeed", 5f);
	    gameObject.animation.CrossFade("Run_Begin_1");
	    yield return new WaitForSeconds(.2f);
	    gameObject.animation.CrossFade("Run_Loop_1");
	}
	IEnumerator COAttack() {
		inAnimation = true;
		gameObject.animation.CrossFade("Poise_Loop_1");
		yield return new WaitForSeconds(.4f);
		bite.collider.enabled = true;
	    gameObject.animation.CrossFade("Bite_1");
		yield return new WaitForSeconds(.6f);
		bite.collider.enabled = false;
		gameObject.animation.CrossFade("Poise_Loop_1");
		inAnimation = false;
	}
	IEnumerator COHitRecoil() {
		inAnimation = true;
		gameObject.animation.CrossFade("Hit_Recoil_1");
	    yield return new WaitForSeconds(.4f);
		inAnimation = false;
	}

    IEnumerator CODie() {
        inAnimation = true;
        Debug.Log("beginning death animation");
        yield return new WaitForSeconds(.4f);
        gameObject.animation.CrossFade("Death_1");
        //Debug.Log("yielding");
        yield return new WaitForSeconds(gameObject.animation.GetClip("Death_1").length);
    }
    
    
    void StartDeathAnim() {
        //Debug.Log("beginning die function");
        if (this.transform.parent != null) {
            this.transform.parent.GetComponent<EnemyNodeManager>().startRespawnTimer();
        }
        dead = true;
        m_tree = null;  //stop behavior tree from running
        StartCoroutine(CODie()); //run death animation

        this.gameObject.AddComponent<DeadDogManager>(); //add item manager to dog carcass

        //disable colliders which are no longer needed
        this.gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        //resize box collider to be used for gathering interaction
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(.5f, .6f, .06f);
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(2.0f, 1.0f, 2.8f);

    }
	

    void OnTriggerEnter(Collider col) {
        if (!dead && col.gameObject.tag == "PlayerWeaponBone") {
            //Debug.Log("I've been hit!");
            StartCoroutine(COHitRecoil());
            takingHitTimer = takingHitTimerMax;
            col.gameObject.transform.parent.parent.SendMessage("Hit");
            this.gameObject.SendMessage("TakeDamage", (col.gameObject.transform.parent.parent.GetComponent("DamageDealer") as DamageDealer).damage);
        }
    }

	//Poorly named, actually used for chasing also
	void WanderingDone(int retValue)
	{
		switch(retValue)
		{
		case -1:
			//we got stuck or something weird happened
			doneWandering = true;
            Debug.Log("shit im stuck");
			break;
		case 1:
			//we got to or close enough to our wander target
			doneWandering = true;
			break;
		default:
			break;
		}
	}
	
	private bool EvaluateExpression( string expr ) {
	    string[] exprs;
	    exprs = expr.Split( new string[] { "&&"} , System.StringSplitOptions.None );
	    if( exprs.Length > 1 ) {
	        foreach( string e in exprs ) {
	            if( !EvaluateExpression( e.Trim() ) ) {
	                return false;
	            }
	        }
	        return true;
	    } else {
	        exprs = expr.Split( new string[] { "||" }, System.StringSplitOptions.None );
	        if( exprs.Length > 1 ) {
	            foreach( string e in exprs ) {
	                if( EvaluateExpression( e.Trim() ) ) {
	                    return true;
	                }
	            }
	            return false;
	        } else {
	            bool not = false;
	            if( expr.StartsWith( "!" ) ) {
	                not = true;
	                expr = expr.Replace( "!" , "" );
	            }
	            bool b = (bool)this.GetType().GetField( expr ).GetValue( this );
	            if( not ) {
	                return !b;
	            } else {
	                return b;
	            }
	        }
	    }
	}
}

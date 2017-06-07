using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotV2 : MonoBehaviour {
    public LineOfSight lineOfSight;
    public PathFinding movement;
    public float maxDistanceForCharge;
    public float timeTrackedPlayer;
    public float timeBeforeCharge;
    public float timeCharging;
    float tC;
    public bool newDestination = false;
    float tD = 0;
    public float timeForNewPos;
    public float chargForce;
    bool charge;
    public bool chargeDone;
    public enum state
    {
        Patroling,
        Tracking,
        Charging,
        
    }
    public state currentState;
    // Use this for initialization
    void Start () {
        lineOfSight = GetComponent<LineOfSight>();
        movement = GetComponent<PathFinding>();
        chargeDone = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!newDestination && currentState == state.Tracking)
            newDestination = StandardFunctions.instance.Timer(ref tD, timeForNewPos);

        StateLogic();
    }

    void Logic()
    {

        switch (currentState)
        {
            case state.Patroling:
                {
                    if (!movement.Patroling)
                    {
                        movement.PathFind(movement.MoveToRandomPoint(), movement.nodeSystem);
                        movement.Patroling = true;
                     
                    }
                  
                }
                break;

            case state.Tracking:
                {
                    movement.Patroling = false;
                    if (AiManager.instance.playerMoved || newDestination)
                    {
                        movement.FollowPlayer();
                        
                    }
                        
                }
                 break;
            case state.Charging:
                {
                    movement.Patroling = false;
                    charge = false;
                    Debug.Log("Charge");
                    chargeDone = StandardFunctions.instance.Timer(ref tC, timeCharging);
                    if (!chargeDone)
                        Charging();
                }
                break;
        }
    }

    public void StateLogic()
    {
        if (lineOfSight.canSeePlayer && currentState != state.Charging)
        {
            if (currentState == state.Tracking)
            {
                if (!charge)
                    charge = StandardFunctions.instance.Timer(ref timeTrackedPlayer, timeBeforeCharge);

                float distaneToPlayer = Vector3.Distance(transform.position, AiManager.instance.player.transform.position);
                if (charge && distaneToPlayer < maxDistanceForCharge)
                {
                    currentState = state.Charging;
                    chargeDone = false;
                }
                    
            }
            else if (currentState == state.Patroling)
            {

                if (lineOfSight.canSeePlayer)
                {
                    newDestination = true;
                    currentState = state.Tracking;
                }
            }
        }
        else if  (currentState == state.Charging)
        {
            Debug.Log("Charge");
            if (chargeDone)
            {
                movement.rb.velocity = Vector3.zero;
                currentState = state.Patroling;
            }
        }

        Logic();
    }

    public void Charging()
    {
        movement.move = false;
        movement.rb.velocity = ( AiManager.instance.player.transform.position - transform.position) * chargForce;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        charge = false;
        Debug.Log("hit");
        Debug.Log(collision.transform.tag);
        chargeDone = true;
        tC = 0;
        if (collision.gameObject.CompareTag("Player"))
            lineOfSight.transform.LookAt(collision.transform.position);

        if (lineOfSight.canSeePlayer)
        {
            movement.rb.velocity = Vector3.zero;
            currentState = state.Tracking;
         
          
        }
        else
        {
           
            
            currentState = state.Patroling;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
           
      
    }

}

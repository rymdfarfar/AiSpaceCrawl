using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    public GameObject head;
    public float maxAngle;
    public float minAngle;
    public float maxViewingDis;
    public float radiusOfRay;
    public bool inLineOfSight = false;
    public bool checkingLineOfSight = false;
    public bool canSeePlayer = false;
    float t;
    public float timerLineOfSight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (checkingLineOfSight && StandardFunctions.instance.Timer(ref t, timerLineOfSight))
            InLineOfSight(AiManager.instance.player.transform.position);
	}
    //Checks if the player is within line of sight
    public void InLineOfSight(Vector3 playerPos)
    {
      
        Vector3 vec = -head.transform.right;
        Vector3 playerDir = head.transform.position - playerPos;
        float dot = Vector3.Dot(vec.normalized, playerDir.normalized);
        Debug.Log(dot);
        Debug.DrawRay(transform.position, vec.normalized * (minAngle+1), Color.green, 0.2f);
        Debug.DrawRay(transform.position, playerPos.normalized, Color.red, 0.2f);
        if (dot < maxAngle && dot > minAngle)
        {

            inLineOfSight = true;
            checkingLineOfSight = false;
            radiusOfRay = dot;
            CanSeePlayer();
        }
        else
        {
            inLineOfSight = false;
            checkingLineOfSight = true;
        }
           

    }

    public void CanSeePlayer()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, radiusOfRay, AiManager.instance.player.transform.position, out hit, maxViewingDis))
        {
            if (hit.collider.CompareTag("Player"))
                canSeePlayer = true;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    public GameObject head;
    public float maxAngle;
    public float minAngle;
    public float maxViewingDis;
    public float radiusOfPlayer;
    public bool inLineOfSight = false;
    public bool checkingLineOfSight = false;
    public bool canSeePlayer = false;
    float t;
    float tCheckAgain;
    public float lostVisionTime;
    public float timerLineOfSight;
    public float dot2D;
    public float dot3D;
    // Use this for initialization
    void Start () {
        radiusOfPlayer = AiManager.instance.player.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * 0.8f;
	}

    // Update is called once per frame
    void Update()
    {
        LineOfSightLogic();
        if (canSeePlayer)
            head.transform.LookAt(AiManager.instance.player.transform.position);
      
    }
    //Checks if the player is within line of sight
    public void InLineOfSight(Vector3 playerPos)
    {
        bool in2DCone = false;
        bool in3DCone = false;
        Vector2 playerPosition2D = new Vector2(playerPos.x, playerPos.z);
        Vector2 enemylookDir2D =  new Vector2 (head.transform.forward.x, head.transform.forward.z) ;
        Vector2 enemyPos2D= new Vector2(head.transform.position.x, head.transform.position.z);
        Vector3 lookLeft = Quaternion.AngleAxis(30, Vector3.up) * new Vector3(enemylookDir2D.x, 0, enemylookDir2D.y);
        Vector3 lookRight = Quaternion.AngleAxis(30, Vector3.down) * new Vector3(enemylookDir2D.x, 0, enemylookDir2D.y);
        Debug.DrawRay(head.transform.position, lookLeft, Color.green, 5);
        Debug.DrawRay(head.transform.position, lookRight, Color.green, 5);
        Vector2 playerDir2D = playerPosition2D - enemyPos2D;
     
       
      

   
        playerDir2D = playerDir2D.normalized;
      
        dot2D = Vector2.Dot(playerDir2D, enemylookDir2D);

        if (dot2D > Mathf.Cos(30 * Mathf.Deg2Rad))
            in2DCone = true;

        if (in2DCone)
        {

            Vector2 playerPosition3D = new Vector2(playerPos.y, playerPos.z);
            Vector2 enemyLookDir3D = new Vector2(head.transform.forward.y, head.transform.forward.z);
            Vector2 enemyPos3D = new Vector2(head.transform.position.y, head.transform.position.z);
            Vector2 playerDir3D = playerPosition3D - enemyPos3D;
            lookLeft = Quaternion.AngleAxis(30, Vector3.left) * new Vector3(0, enemyLookDir3D.x, enemyLookDir3D.y);
             lookRight = Quaternion.AngleAxis(30, Vector3.right) * new Vector3(0, enemyLookDir3D.x, enemyLookDir3D.y);
            Debug.DrawRay(head.transform.position, lookLeft, Color.green, 5);
            Debug.DrawRay(head.transform.position, lookRight, Color.green, 5);
            Debug.DrawRay(head.transform.position, new Vector3(0, playerDir3D.x, playerDir3D.y), Color.green, 5);

            playerDir3D = playerDir3D.normalized;
            dot3D = Vector2.Dot(playerDir3D, enemyLookDir3D);
            if (dot3D > Mathf.Cos(30 * Mathf.Deg2Rad))
                in3DCone = true;

        }
        else
            return;

        if (in3DCone)
        {
            inLineOfSight = true;
            checkingLineOfSight = false;
            CanSeePlayer();
            
        }
        else
            return;
       
       


    }

    public void CanSeePlayer()
    {
       
      
        if (maxViewingDis >= Vector3.Distance(head.transform.position, AiManager.instance.player.transform.position))
        {
            RaycastHit hit;
           
            Vector3 playerDir = AiManager.instance.player.transform.position - head.transform.position;
            Debug.DrawRay(head.transform.position, playerDir, Color.red, 5);
            if (Physics.Raycast(head.transform.position, playerDir, out hit, maxViewingDis))
                {
                Debug.Log(hit.collider.gameObject.name);
                    Debug.Log("ray");
                    Debug.Log(hit.collider.tag);
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("canSeePlayer");
                        canSeePlayer = true;
                    
                    }


                }
            
        }
        if (!canSeePlayer)
        {
            inLineOfSight = false;
            checkingLineOfSight = true;

        }

       
    }
    //Calls functions for line of sight and rayacst 
    public void LineOfSightLogic()
    {
        if (!canSeePlayer)
        {
            if (checkingLineOfSight && StandardFunctions.instance.Timer(ref t, timerLineOfSight))
                InLineOfSight(AiManager.instance.player.transform.position);
        }
        else
        {
            if (StandardFunctions.instance.Timer(ref tCheckAgain, lostVisionTime))
            {
                Debug.Log("!");
                CanSeePlayer();
            }

        }
    }
}

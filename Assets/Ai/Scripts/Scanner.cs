using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour {

 //   public robot robot;
 //   public Turret[] turrets;

 //   public LineRenderer scaning;
 //   public LineRenderer tracking;
 //   public GameObject scanPoint;
 //   public float speed;
 //   public float scanDistance;
 //   public int scans;
 //   public float timeBetweenShoots;
 //   Vector3 scanPos;
 //   public Vector3 jumpBetScans;
 //   public float timeTracked;
 //   public float timeNeededTracked;
 //   public float timeBeforeAttack;
 //   public float tAttack = 0;
 //   public float maxDisAttack;
 //   public float distanceToPlayer;
 //   public LayerMask player;
 //   int scanNumb = 0;
 //   public bool foundPlayer;
 //   bool checking;
 //   public float rotSpeed;
    

 //   Vector3[] pointsScan = new Vector3[2];
 //   Vector3[] pointsTrack = new Vector3[2];
 //   // Use this for initialization
 //   void Start () {
 //       scaning.SetPosition(0, scanPoint.transform.position);
 //       tracking.SetPosition(0, scanPoint.transform.position);
 //       turrets = GetComponentsInChildren<Turret>();
 //       robot = GetComponentInParent<robot>();
 //       scanPos = scanPoint.transform.forward + (-scanPoint.transform.up);
        

 //   }
	
	//// Update is called once per frame
	//void Update () {

        
	//}


 //   public void Scanning()
 //   {
 //       transform.Rotate(Vector3.up * speed * Time.deltaTime);

        

 //       if (scanNumb > scans  )
 //       {
 //           scanNumb = 0;
 //           scanPos = scanPoint.transform.forward + (-scanPoint.transform.up);

 //       }
 //       scanPos = scanPoint.transform.forward + (-scanPoint.transform.up) + jumpBetScans * scanNumb;

 //           RaycastHit hit;
 //           scaning.enabled = true;
 //           tracking.enabled = false;
     
        
 //           Debug.DrawRay(scanPoint.transform.position, scanPos, Color.red, 0.5f);

           
 //           if (Physics.Raycast(scanPoint.transform.position, scanPos, out hit, scanDistance))
 //           {

 //               pointsScan[0] = scanPoint.transform.position;
 //               pointsScan[1] = hit.point;
 //               scaning.SetPositions(pointsScan);
               
 //               if (hit.collider.CompareTag("Player"))
 //               {
 //                   Debug.Log("Found Player");
 //                   robot.robotState = robot.state.Tracking;
 //                   scanNumb = 0;
 //                   scanPos = scanPoint.transform.forward + (-scanPoint.transform.up);
                   
 //               }
 //           }



       
 //       ++scanNumb;
       
 //   }
 //   public void Tracking()
 //   {
 //       scaning.enabled = false;
 //       RaycastHit hit;
 //       transform.LookAt(AiManager.instance.player.transform);
 //       tracking.enabled = true;
       
       
 //       Debug.DrawRay(scanPoint.transform.position, transform.forward, Color.green);
 //       if (Physics.Raycast(scanPoint.transform.position, transform.forward, out hit, scanDistance))
 //       {
 //           tracking.SetPosition(0, scanPoint.transform.position);

 //           tracking.SetPosition(1, hit.point);

 //           timeTracked += Time.deltaTime;
 //           if (timeTracked > timeNeededTracked)
 //           {
 //               distanceToPlayer = hit.distance;
 //               robot.robotState = robot.state.ScanForAttack;
 //               Debug.Log("ScanForAttack");
 //               timeTracked = 0;

 //           }
 //           else
 //           {
 //               foreach (Turret t in turrets)
 //               {
 //                   bool canShoot = Timer(ref tAttack, 0.5f);
 //                   if (canShoot)
 //                   {
 //                       t.Shoot();
 //                       Debug.Log("Shoot");
 //                   }
                        
 //               }
 //           }
            

 //       }
 //       else
 //       {
 //           timeTracked = 0;
 //           robot.robotState = robot.state.Idle;
 //       }
 //   }

 //   public void ScanToAttack()
 //   {
        
 //       Debug.Log(distanceToPlayer);
        
 //           if (distanceToPlayer < maxDisAttack)
 //           {

 //               if (Physics.BoxCast(scanPoint.transform.position, robot.gameObject.transform.lossyScale / 2.5f, transform.forward, Quaternion.identity, distanceToPlayer - 0.2f, ~player))
 //               {

 //                   robot.robotState = robot.state.Tracking;


 //               }
 //               else
 //               {
 //                   robot.robotState = robot.state.Charging;
 //                   transform.LookAt(AiManager.instance.player.transform);
 //                   Debug.Log("Attack");
 //               }
 //           }
        
       
       
 //   }
 //   public bool Timer(ref float t, float maxT)
 //   {
       
 //        t += Time.deltaTime;
 //       if (t > maxT)
 //       {
 //           t = 0;

 //           return true;
          
 //       }
 //       else
 //           return false;
 //   }

 //   public void RotTowardsObject()
 //   {
 //       float step = rotSpeed * Time.deltaTime;
 //       Vector3 rot = Vector3.RotateTowards(transform.rotation.eulerAngles, AiManager.instance.player.transform.rotation.eulerAngles, rotSpeed, 0);
 //       transform.rotation = Quaternion.Euler(rot);
 //   }

    
}

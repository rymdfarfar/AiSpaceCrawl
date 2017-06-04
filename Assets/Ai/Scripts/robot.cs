using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot : MonoBehaviour {

 //   public Scanner scanner;
 //   public PathFinding movement;
 //   Rigidbody rb;
 //   public float maxDistaneTofollow;
 //   public float chargSpeed;
 //   public float timeForCharge;
 //   public float timeToStabalize;
 //   Quaternion orgRot;
 //   float tCharge;
 //   float tStabalize;
 //   public float rotSpeed;
 //   public AudioSource source;
 //   public AudioClip clip;
 //   public ParticleSystem boost;
 //   public bool onTheMove;
 //   public bool hitSomething;
 //   public bool charging;
 //   bool counterForce;
 //   public state robotState;
 //   public enum state
 //   {
 //       Idle,
 //       Tracking,
 //       ScanForAttack,
 //       Charging,
 //       Catching,
 //       Stabalize
 //   }
	//// Use this for initialization
	//void Start () {

 //       orgRot = transform.rotation;
 //       scanner = GetComponentInChildren<Scanner>();
 //       movement = GetComponent<PathFinding>();
 //       rb = GetComponent<Rigidbody>();
       
	//}
	
	//// Update is called once per frame
	//void Update () {
        
       
 //       Logic();
		
	//}

 //   public void Logic()
 //   {

 //       switch (robotState)
 //       {
 //           case state.Idle:
 //               {
                   
 //                   scanner.Scanning();
 //               }
 //               break;
 //           case state.ScanForAttack:
 //               {
                    
 //                   Debug.Log("Attack");
 //                   scanner.ScanToAttack();
                    
 //               }
 //               break;
 //           case state.Tracking:
 //               {
 //                   NewDestiatnion();
 //                   transform.rotation.SetFromToRotation(transform.rotation.eulerAngles, AiManager.instance.player.transform.rotation.eulerAngles);
 //                   scanner.Tracking();
 //               }
 //               break;
 //           case state.Charging:
 //               {
                   
 //                   Charge();
                   
 //               }
 //               break;
 //           case state.Stabalize:
 //               {
 //                   Stabalize();
 //               }
 //               break;
 //       }
 //   }

 //   public void Charge()
 //   {
 //       if (!charging)
 //       {
 //           transform.LookAt(AiManager.instance.player.transform.position);
 //           scanner.transform.LookAt(AiManager.instance.player.transform.position);
 //           charging = true;
 //           movement.move = false;

 //       }

 //       tCharge += Time.deltaTime;
 //       if (timeForCharge > tCharge)
 //       {
 //           if (!source.isPlaying)
 //           {
 //               source.PlayOneShot(clip);
 //           }
 //           boost.Emit(1);
            
 //           Debug.Log("Charge");
 //           rb.AddForce(transform.forward, ForceMode.Force);
 //       }
 //       else
 //       {
 //           tCharge = 0;
 //           charging = false;
 //           robotState = state.Stabalize;
 //       }
       
        
      
 //   }

 //   public void OnCollisionEnter(Collision collision)
 //   {
 //       Debug.Log(collision.gameObject);

        


 //       robotState = state.Stabalize;
 //       tCharge = 0;
 //       charging = false;


 //   }

 //   public void Stabalize()
 //   {
 //       bool knowledegeofCurrentPos = false;
 //       movement.move = false;
 //       tStabalize += Time.deltaTime;
 //       if (timeToStabalize > tStabalize)
 //       {
 //           transform.LookAt(Vector3.forward);
            
            
           
           

 //       }
 //       else
 //       {

 //           rb.velocity = Vector3.zero;
 //           rb.angularVelocity = Vector3.zero;
 //           scanner.transform.rotation = transform.rotation;
 //           if (movement.current != null)
 //           {
 //               foreach (Node n in movement.current.connectingNodes)
 //               {
 //                   if (n.cube.Contains(transform.position))
 //                   {
 //                       knowledegeofCurrentPos = true;
 //                       movement.current = n;
 //                       break;
 //                   }
 //               }
 //           }
            
            
 //           if (knowledegeofCurrentPos)
 //           {
 //               //movement.PathFind(AiManager.instance.playesCurrentNodeCharging);
 //               movement.move = false;
 //               robotState = state.Idle;
 //           }
 //           else
 //           {
 //               movement.first = true;
 //               //movement.PathFind(AiManager.instance.playesCurrentNodeCharging);
 //               movement.move = false;
 //               robotState = state.Idle;


 //           }
          
            
            
 //           tStabalize = 0;
 //       }
      
       
        
        
 //   }
 //   public void NewDestiatnion()
 //   {
 //       bool knowledgeOfcurNode = false;
 //       if (!movement.move)
 //       {
 //           if (movement.current != null)
 //           {
 //               foreach (Node n in movement.current.connectingNodes)
 //               {
 //                   if (n.cube.Contains(transform.position))
 //                   {
                       
 //                       movement.current = n;
 //                       knowledgeOfcurNode = true;
 //                       break;

 //                   }
 //               }
 //           }
 //           movement.first = true;
 //           //movement.PathFind(AiManager.instance.playesCurrentNodeCharging);
 //           movement.move = true;
           
 //       }
 //   }
}

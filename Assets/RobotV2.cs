using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotV2 : MonoBehaviour {
    public LineOfSight lineOfSight;
    public PathFinding movement;
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
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    void Logic()
    {

        switch (currentState)
        {
            case state.Patroling:
                {

                }
                break;

            case state.Tracking:
                {

                }
                 break;
            case state.Charging:
                {
                }
                break;
        }
    }

}

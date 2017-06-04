using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTestScript : MonoBehaviour {
    public NodeManager manager;
    public Vector3 girdPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        girdPos = manager.WorldPosToGridPos(transform.position, 2);
	}
}

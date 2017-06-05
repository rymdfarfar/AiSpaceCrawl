using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTestScript : MonoBehaviour {
    public NodeManager manager;
    public Vector3 girdPos;
    public GameObject circle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        girdPos = AiManager.instance.WorldPosToGridPos(transform.position, 2, manager);
	}
}

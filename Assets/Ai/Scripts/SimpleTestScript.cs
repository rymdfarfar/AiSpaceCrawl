using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTestScript : MonoBehaviour {
    public NodeManager manager;
    public Vector3 girdPos;
    public Collider box;
  
	// Use this for initialization
	void Start () {
        box = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}
}

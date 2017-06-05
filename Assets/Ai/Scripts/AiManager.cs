﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    public static AiManager instance;
    public GameObject player;
    public NodeManager[] nodeManagers;
    public float timeToUpdatePlayerPos;
    float t =0;
	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        else if (instance!= this)
            Destroy(this);
        nodeManagers = FindObjectsOfType(typeof(NodeManager)) as NodeManager[];
        
	}
	
	// Update is called once per frame
	void Update () {

        if (StandardFunctions.instance.Timer(ref t, timeToUpdatePlayerPos))
            PlayerPosForGrids();
        

	}

    public Vector3 WorldPosToGridPos(Vector3 pos, int nodeSys, NodeManager man)
    {
        Node node = man.nodeSystems[nodeSys].nodes[0];
        Vector3 deltaVec = pos - node.cube.center;
        float gridX = deltaVec.x / node.cube.size.x;
        float girdY = deltaVec.y / node.cube.size.y;
        float gridZ = deltaVec.z / node.cube.size.z;

        return new Vector3(gridX, girdY, gridZ);

    }

    public int PosToIndex(Vector3 pos, int ns, NodeManager man)
    {
        float index = pos.x + pos.z * man.nodeSystems[ns].widht + pos.y * (man.nodeSystems[ns].widht * man.nodeSystems[ns].depth);
        return (int)index;
    }
    //Keeps the pos with in each different nodeManager update
    public void PlayerPosForGrids()
    {
        foreach (NodeManager nm in nodeManagers)
        {
            nm.PlayerGirdPos();
        }
    }

    public Vector3 GivePosCloseToPlayer(int nodeSys,  NodeManager man)
    {
        bool nodeOnPos = false;
        int index = 0;
        Vector3 pos = new Vector3();
        SimpleTestScript playerScript = player.GetComponent<SimpleTestScript>();
        Collider box = playerScript.circle.GetComponent<Collider>();
        while (!nodeOnPos)
        {

        
            float x = Random.Range(box.bounds.min.x, box.bounds.max.x);
            float y = Random.Range(box.bounds.min.y, box.bounds.max.y);
            float z = Random.Range(box.bounds.min.z, box.bounds.max.z);
            pos = new Vector3(x, y, z);
            Vector3 girdPos = WorldPosToGridPos(pos, nodeSys, man);
            index = PosToIndex(girdPos, nodeSys, man);
            if (index < man.nodeSystems[nodeSys].nodes.Count)
            {
                if (man.nodeSystems[nodeSys].nodes[index] != null && man.nodeSystems[nodeSys].nodes[index].type != NodeManager.NodeTypes.Invalid )
                {
                    nodeOnPos = true;

                }
            }
           
            
              
        }
        return pos;




    }
   
}

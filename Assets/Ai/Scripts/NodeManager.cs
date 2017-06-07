using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeManager : MonoBehaviour
{
   
    
    public GameObject character;
    public GameObject node;
    public Bounds dimensionsOfRoom;
    public string[] tags;
    public List<NodeSystem> nodeSystems;

    public float maxDistanceBetweenDoors;
    public bool diagonally;
    

    [HideInInspector]
    public List<Node> tempNodes;
    [HideInInspector]
    public bool partitionCreated;


    public List<GameObject> level;


    float height;
    float width;
    float depth;
  
    [HideInInspector]
    public int numberOfNodes;
    [HideInInspector]
    public int xNumber;
    [HideInInspector]
    public int yNumber;
    [HideInInspector]
    public int zNumber;
    [HideInInspector]
    public int nodesSpawned;
    public float rayTimer = 0.2f;


    Vector3 cube;
    public int playerIndex;
    public int playerNodsys;


   

    public enum NodeTypes
    {
        Door,
        Standard,
        Invalid,
        Hallway
    }



    // Use this for initialization
    void Start()
    {
        PlayerGirdPos();


    }

    // Update is called once per frame
    void Update()
    {

        

    }

    //Updates the players indexpos for the grid
    public void PlayerGirdPos()  
    {
        Debug.Log("Update");
        Vector3 playerPos = AiManager.instance.player.transform.position;
        foreach (NodeSystem ns in nodeSystems)
        {
            if (ns.area.Contains(playerPos))
            {
                playerNodsys = ns.id;
                break;
            }
        }
        Vector3 gridPos = AiManager.instance.WorldPosToGridPos(playerPos, playerNodsys, this);
        int temp = AiManager.instance.PosToIndex(gridPos, playerNodsys, this);
        if (playerIndex < nodeSystems[playerNodsys].nodes.Count && playerIndex > 0)
        {
            playerIndex = temp;
        }
       
    }

   


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(dimensionsOfRoom.center, dimensionsOfRoom.size);
    }



    public void FillLevel(GameObject parent)
    {
        
       // this figures out how big all nodes bounding boxes will be and fills the current bounds with nodes
            height = character.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * 1.1f;
            depth = character.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * 1.1f;
            width = character.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * 1.1f;
        
            cube = new Vector3(width, height, depth);

            xNumber = (int)(dimensionsOfRoom.size.x / width);
            zNumber = (int)(dimensionsOfRoom.size.z / depth);
            yNumber = (int)(dimensionsOfRoom.size.y / height);
            
           

       numberOfNodes =  (xNumber * zNumber) * yNumber;
        Debug.Log(numberOfNodes);
        bool zFirst = true;
        int indexY =0;
        int indexZ = 0;
        int indexX = 0;
        Vector3 spawnPosY = dimensionsOfRoom.min + ((cube) / 2);
        Vector3 spawnPosX = dimensionsOfRoom.min + ((cube) / 2);
        Vector3 spawnPosZ = dimensionsOfRoom.min + ((cube) / 2);
        for (int y = 0; y < yNumber; ++y)
        {
            Bounds bTemp = new Bounds();
            bTemp.size = cube;
            bTemp.center = spawnPosY;
            spawnPosZ = spawnPosY;

            indexY = y;
            indexZ = 0;
            zFirst = true;
            if (bTemp.max.y < dimensionsOfRoom.max.y)
            {
                spawnNode(spawnPosY, parent, indexY, indexZ, indexX);
                spawnPosY.y += cube.y;
            }

          

            for (int z = 0; z < zNumber; ++z)
            {
                
              
                bTemp.center = spawnPosZ;
                spawnPosX = spawnPosZ;
                spawnPosX.x += cube.x;
                indexZ = z;

                if (bTemp.max.z < dimensionsOfRoom.max.z && zFirst != true)
                {
                    spawnNode(spawnPosZ, parent, indexY, indexZ, indexX);
                    spawnPosZ.z += cube.z;
                }
            

                for (int x = 0; x < xNumber; ++x)
                {
                    if (zFirst)
                    {
                        spawnPosZ.z += cube.z;
                        zFirst = false;
                    }
                    
                    bTemp.center = spawnPosX;
                    indexX = x +1;
                    if (bTemp.max.x < dimensionsOfRoom.max.x)
                    {
                        spawnNode(spawnPosX, parent, indexY, indexZ, indexX);
                        spawnPosX.x += cube.x;
                    }
                    indexX = 0;
                   
                 
                }
               
               
            }

        }
            
          
    }

    public void spawnNode(Vector3 pos, GameObject parent, int y, int z, int x)
    {
        //this spawns a node and build a bounding box around it
        

    
       GameObject temp = Instantiate(node, pos, Quaternion.Euler(Vector3.zero));
        Node nodeTemp = temp.GetComponent<Node>();
        nodeTemp.cube.size = cube;
        nodeTemp.cube.center = pos;
        nodeTemp.transform.position = pos;
        nodeTemp.x = x;
        nodeTemp.y = y;
        nodeTemp.z = z;
        //nodeTemp.pos = new Vector3(x, y, z);
        nodeTemp.id = nodesSpawned;
        nodeTemp.index = x + z * xNumber + y * (xNumber * zNumber);
        nodeTemp.myNodeManager = this;
        nodeTemp.connectingNodeSys = nodeTemp.myNodeSysId;
        nodeTemp.type = NodeTypes.Standard;
        if (diagonally)
            nodeTemp.diagonally = true;
        //nodeTemp.myNodeSysId = nodeSystems.Count + 1;
        temp.name = ("Node" + " " + nodeTemp.id.ToString());
        tempNodes.Add(nodeTemp);
        temp.transform.SetParent(parent.transform);
        ++nodesSpawned;
        Debug.Log("NodeSpawned");

        
        
        
    }

 

   
    public void CreatePartitionOfNodes()
    {
        //Adds a nodesystem to nodesystems and fills the current bound of the variable roomDimensions
        GameObject go = new GameObject();
        go.transform.SetParent(this.transform);
        NodeSystem nodeSys = go.AddComponent<NodeSystem>();
        nodeSys.area = dimensionsOfRoom;
   
       
        FillLevel(go);
        nodeSys.widht = xNumber;
        nodeSys.height = yNumber;
        nodeSys.depth = zNumber;
        nodeSys.id = nodeSystems.Count;

        foreach (Node n in tempNodes)
        {
            n.myNodeSysId = nodeSystems.Count;
            nodeSys.nodes.Add(n);
          
        }
        nodeSystems.Add(nodeSys);
        go.transform.position = nodeSys.nodes[0].transform.position;
        nodeSys.sysName = "Node System" + " " + nodeSystems.Count.ToString();
        go.name = nodeSystems[nodeSystems.Count- 1].sysName;
        partitionCreated = true;
        tempNodes.Clear();
    }

   

}

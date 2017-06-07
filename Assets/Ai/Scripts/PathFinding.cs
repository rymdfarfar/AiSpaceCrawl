using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public NodeManager myNodeManager;
    public SystemPathFinding sysFinder;
    public RobotV2 mrRoboto;
    public bool checkStartGoal;
    public bool foundGoal;


    public List<Node> openList;
    public bool nextNode;
    public List<Node> pathToGoal;
    public Rigidbody rb;
    public Vector3 gridPos;
    public float gridId;
    public Node start;
    public Node end;
    public bool Patroling;
    public int nodeSystem;
    bool aiMoveToPlayer;
    public bool move;
    public bool first;
    bool firstMove;
    int indexPos;
    public float speed;
    float step;
    // Use this for initialization
    void Start()
    {
        //firstMove = true;
        mrRoboto = GetComponent<RobotV2>();
        sysFinder = GetComponent<SystemPathFinding>();
        first = true;
        rb = GetComponent<Rigidbody>();
        move = true;
        
       
    }

    // Update is called once per frame

    public void FixedUpdate()
    {
        MoveAI();
        //Draws line to the destination
        if (pathToGoal.Count > 0 && move)
        {
            foreach (Node n in pathToGoal)
            {
                int index = pathToGoal.IndexOf(n) + 1;
                if (index < pathToGoal.Count)
                {

                    Debug.DrawLine(n.cube.center, pathToGoal[index].cube.center, Color.cyan, 0.2f);
                }

            }
        }
    }

    public void MoveAI()
    {
        //Moves the ai to the next node until it gets to it's destination
        if (move && foundGoal && pathToGoal.Count > 1)
        {
            if (firstMove)
            {
               
                indexPos = pathToGoal.Count - 1;
                firstMove = false;
            }
            nodeSystem = pathToGoal[indexPos].myNodeSysId;
            step = speed * Time.deltaTime;
          

            Vector3 moveTo =  pathToGoal[indexPos].transform.position - transform.position;
            moveTo = moveTo.normalized * step;
            gridPos = AiManager.instance.WorldPosToGridPos(transform.position, nodeSystem, myNodeManager);

            gridId = AiManager.instance.PosToIndex(gridPos, nodeSystem, myNodeManager);


            nextNode = gridId == pathToGoal[indexPos].id;
            if(moveTo != Vector3.zero)
                transform.forward = moveTo;


            rb.velocity = moveTo;
          


            if (nextNode)
            {
               
                --indexPos;


                step = 0;

            }
            if (indexPos < 0)
            {
                move = false;
                checkStartGoal = false;
                rb.velocity = Vector3.zero;
                if (Patroling)
                {
                    MoveToRandomPoint();
                }
                else
                {
                    FollowPlayer();
                }
                 

                Patroling = false;

            }


        }

    }

    //Gives the AI a new Pos when the player moves or a certain time has passed
    public void FollowPlayer()
    {

        
            Debug.Log("NewPos");
           
        
                if (gridId > 0 && gridId < myNodeManager.nodeSystems[nodeSystem].nodes.Count && mrRoboto.currentState == RobotV2.state.Tracking)
                    PathFind(AiManager.instance.GivePosCloseToPlayer(myNodeManager.playerNodsys, myNodeManager), myNodeManager.playerNodsys);
            
            mrRoboto.newDestination = false;
        

    }

    
    



    public int  MoveToRandomPoint()
    {
        //Moves to random Point
        int rnd = Random.Range(0,myNodeManager.nodeSystems[nodeSystem].nodes.Count - 1);
        while (myNodeManager.nodeSystems[nodeSystem].nodes[rnd].type == NodeManager.NodeTypes.Invalid)
        {
            rnd = Random.Range(0, myNodeManager.nodeSystems[nodeSystem].nodes.Count - 1);
        }
        Debug.Log(rnd);
        Node temp = myNodeManager.nodeSystems[nodeSystem].nodes[rnd];
        
        return temp.id;
    }

    public void PathFind(int endIndex, int endSys)
    {
        checkStartGoal = false;
        Debug.Log("pathFinding");
    
        int startSysNumb = 0;
        foreach (NodeSystem ns in myNodeManager.nodeSystems)
        {
           
            if (ns.area.Contains(transform.position))
            {
                startSysNumb = ns.id;
                nodeSystem = ns.id;
            }
        }

        Node endNode = myNodeManager.nodeSystems[endSys].nodes[endIndex];
        Node startNode = myNodeManager.nodeSystems[startSysNumb].nodes[AiManager.instance.PosToIndex(AiManager.instance.WorldPosToGridPos(transform.position, startSysNumb, myNodeManager), startSysNumb, myNodeManager)];
        
        if (endNode.myNodeSysId != startNode.myNodeSysId)
        {
            sysFinder.SysToOpen(startNode.myNodeSysId, endNode.myNodeSysId);
        }
        else
        {
            sysFinder.syssToOpen.Add(myNodeManager.nodeSystems[startNode.myNodeSysId]);
        }
        if (!checkStartGoal)
        {

            openList.Clear();
            pathToGoal.Clear();
        }
        bool open = false;
        foreach (NodeSystem ns in myNodeManager.nodeSystems)
        {
            foreach (NodeSystem nsToOpen in sysFinder.syssToOpen)
            {
                if (nsToOpen.id == ns.id)
                    open = true;
            }
            foreach (Node n in ns.nodes)
            {
                if (open)
                {
                    n.closed = false;
                }
                else
                {
                    n.closed = true;
                }
            }
            open = false;

        }
        
       
       



        if (startNode != null)
        {
            
            end = endNode;
            start = startNode;
            checkStartGoal = true;
            firstMove = true;
            SetStartAndGoal(start, end);
        }
        
      
        if (checkStartGoal)
        {

            ContinuePath();
        }





    }

    public void SetStartAndGoal(Node start, Node Goal)
    {
        start.G = 0;
        start.H = start.ManHattanDistance(Goal);
        start.F = start.G + start.H;
        start.parent = null;
        openList.Add(start);
    }
    public Node GetNextNode()
    {
        float bestF = 99999.0f;
        int nodeIndex = -1;
        Node nextNode = null;
        Node temp = null;
        for (int index = 0; index < openList.Count; ++index)
        {
            temp = openList[index];
            if (temp.F < bestF)
            {
                bestF = temp.F;
                nodeIndex = index;
            }
        }
        if (nodeIndex >= 0)
        {
            nextNode = openList[nodeIndex];
            nextNode.closed = true;
            openList.Remove(nextNode);
        }

        return nextNode;
    }

    public void ContinuePath()
    {
        while (openList.Count > 0)
        {

            Node currentNode = GetNextNode();
            if (currentNode.id == end.id && currentNode.myNodeSysId == end.myNodeSysId)
            {
                end.parent = currentNode.parent;
                foundGoal = true;
                break;
            }
            else
            {
                foreach (Node n in currentNode.connectingNodes)
                {

                    PathOpened(n, 1, currentNode);
                }
            }

        }

        if (foundGoal)
        {

            Node getPath;
            for (getPath = end; getPath != null; getPath = getPath.parent)
            {
                pathToGoal.Add(getPath);
               
            }
        }

    }
    public void PathOpened(Node currentNode, float newCost, Node parent)
    {
        if (currentNode != null)
        {

            int id = currentNode.id;

            foreach (NodeSystem ns in myNodeManager.nodeSystems)
            {
                if (currentNode.myNodeSysId == ns.id)
                {
                    if (ns.nodes[id].closed || ns.nodes[id].type == NodeManager.NodeTypes.Invalid)
                        return;
                }
                
            }

          


            currentNode.G = parent.G + newCost;
            currentNode.H = parent.ManHattanDistance(end);
            currentNode.F = currentNode.G + currentNode.H;
            currentNode.parent = parent;
            Node temp = null;
            for (int index = 0; index < openList.Count; ++index)
            {
                temp = openList[index];
                if (id == temp.id)
                {
                    float newF = currentNode.G + temp.H;
                    if (temp.F > newF)
                    {
                        temp.G = currentNode.G;
                        currentNode.F = currentNode.G + currentNode.H;
                        temp.parent = currentNode;
                    }

                    return;
                }

            }
            openList.Add(currentNode);
        }
      
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public NodeManager myNodeManager;
    public SystemPathFinding sysFinder;
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
        sysFinder = GetComponent<SystemPathFinding>();
        first = true;
        rb = GetComponent<Rigidbody>();
        move = true;
        Debug.Log(start.myNodeSysId + "STart");
        Debug.Log(end.myNodeSysId + "End");
        PathFind(end.transform.position);
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

                    Debug.DrawLine(n.cube.center, pathToGoal[index].cube.center, Color.cyan, 1);
                }

            }
        }
    }

    public void MoveAI()
    {
        //Moves the ai to the next node until it gets to it's destination
        if (move && foundGoal)
        {
            if (firstMove)
            {
               
                indexPos = pathToGoal.Count - 1;
                firstMove = false;
            }

            step = speed * Time.deltaTime;
            Vector3 moveTo =  pathToGoal[indexPos].transform.position * step;
            rb.velocity = moveTo;
            //transform.position = moveTo;
            gridPos = myNodeManager.WorldPosToGridPos(transform.position, nodeSystem);

            gridId = myNodeManager.PosToIndex(gridPos, nodeSystem);
           

            nextNode = gridId != pathToGoal[indexPos].id;



            if (nextNode)
            {
               
                --indexPos;


                step = 0;

            }
            if (indexPos < 0)
            {
                move = false;
                checkStartGoal = false;

            }


        }

    }
   

    void Update()
    {
       
    }
    

   
    public Node MoveToRandomPoint()
    {
        //Moves to random Point
        int rnd = Random.Range(0,myNodeManager.nodeSystems[nodeSystem].nodes.Count - 1);
        while (myNodeManager.nodeSystems[nodeSystem].nodes[rnd].type == NodeManager.NodeTypes.Invalid)
        {
            rnd = Random.Range(0, myNodeManager.nodeSystems[nodeSystem].nodes.Count - 1);
        }
        Debug.Log(rnd);
        Node temp = myNodeManager.nodeSystems[nodeSystem].nodes[rnd];
        return temp;
    }

    public void PathFind(Vector3 endPos)
    {
        int endSysNumb = 0;
        int startSysNumb = 0;
        foreach (NodeSystem ns in myNodeManager.nodeSystems)
        {
            if (ns.area.Contains(endPos))
            {
                endSysNumb = ns.id;
            }
            else if (ns.area.Contains(transform.position))
            {
                startSysNumb = ns.id;
            }
        }
        Node endNode = myNodeManager.nodeSystems[endSysNumb].nodes[myNodeManager.PosToIndex(myNodeManager.WorldPosToGridPos(endPos, endSysNumb), endSysNumb)];
        Node startNode = myNodeManager.nodeSystems[startSysNumb].nodes[myNodeManager.PosToIndex(myNodeManager.WorldPosToGridPos(transform.position, startSysNumb), startSysNumb)];
        Debug.Log("pathFinding");
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

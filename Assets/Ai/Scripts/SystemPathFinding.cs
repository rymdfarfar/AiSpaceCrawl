using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPathFinding : MonoBehaviour {

    public NodeManager mananger;
    public List<NodeSystem> openList;
    public List<NodeSystem> syssToOpen;
    public int start;
    public int end;
    public bool checkStartAndEnd;
    public bool foundGoal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SysToOpen(int startSys,int endSys)
    {
        checkStartAndEnd = false;
       
        if (!checkStartAndEnd)
        {
            openList.Clear();
            syssToOpen.Clear();
        }


        foreach (NodeSystem ns in mananger.nodeSystems)
        {
            ns.closed = false;

        }
        end = endSys;
        start = startSys;
        SetStartAndGoal(startSys, endSys);
        checkStartAndEnd = true;

        if (checkStartAndEnd)
        {

            ContinuePath();
        }

    }

    public void SetStartAndGoal(int start, int Goal)
    {
        mananger.nodeSystems[start].G = 0;
        mananger.nodeSystems[start].H = mananger.nodeSystems[start].ManHattanDistance(mananger.nodeSystems[Goal]);
        mananger.nodeSystems[start].F = mananger.nodeSystems[start].G + mananger.nodeSystems[start].H;
        mananger.nodeSystems[start].parent = null;
        openList.Add(mananger.nodeSystems[start]);
    }

    public NodeSystem GetNextSys()
    {
        float bestF = 99999.0f;
        int sysIndex = -1;
        NodeSystem nextSys = null;
        NodeSystem temp = null;
        for (int index = 0; index < openList.Count; ++index)
        {
            temp =openList[index];
            if (temp.F < bestF)
            {
                bestF = temp.F;
                sysIndex = index;
            }
        }
        if (sysIndex >= 0)
        {
            nextSys = openList[sysIndex];
            nextSys.closed = true;
            openList.Remove(nextSys);
            
        }

        return nextSys;
    }

    public void ContinuePath()
    {
        while (openList.Count > 0)
        {

            NodeSystem currentSys = GetNextSys();
            if (currentSys.id == end)
            {
                mananger.nodeSystems[end].parent  = currentSys.parent;
                foundGoal = true;
                break;
            }
            else
            {
                foreach (int ns in currentSys.connectingSys)
                {

                    PathOpened(mananger.nodeSystems[ns], 1, currentSys);
                }
            }

        }

        if (foundGoal)
        {

            NodeSystem getPath;
            for (getPath = mananger.nodeSystems[end]; getPath != null; getPath = getPath.parent)
            {
                syssToOpen.Add(getPath);

            }
        }

    }

    public void PathOpened(NodeSystem currentSys, float newCost, NodeSystem parent)
    {
        if (currentSys.id > 0 && currentSys.id< mananger.nodeSystems.Count)
        {

            int id = currentSys.id;

            if (mananger.nodeSystems[id].closed)
                return;






            currentSys.G =parent.G + newCost;
            currentSys.H = parent.ManHattanDistance(mananger.nodeSystems[ end]);
            currentSys.F = currentSys.G + currentSys.H;
            currentSys.parent = parent;
            NodeSystem temp = null;
            for (int index = 0; index < openList.Count; ++index)
            {
                temp =openList[index];
                if (id == temp.id)
                {
                    float newF = currentSys.G + temp.H;
                    if (temp.F > newF)
                    {
                        temp.G = currentSys.G;
                        currentSys.F = currentSys.G + currentSys.H;
                        temp.parent = currentSys;
                    }

                    return;
                }

            }
            openList.Add(currentSys);
        }

    }

}

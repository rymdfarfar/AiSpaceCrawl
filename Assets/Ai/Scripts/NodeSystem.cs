using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NodeSystem : MonoBehaviour {
    public string sysName;
    public List<int> connectingSys = new List<int>();
    public List<Node> nodes = new List<Node>();
    public List<Node> doorNodes = new List<Node>();
    public Bounds area;
    public int id;
    public int widht;
    public int height;
    public int depth;
    [HideInInspector]
    public float G;
    [HideInInspector]
    public float H;
    [HideInInspector]
    public float F;
    public NodeSystem parent;
    public bool closed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(area.center, area.size);
    }

    public float ManHattanDistance(NodeSystem sysEnd)
    {
        float x = Mathf.Abs(area.center.x - sysEnd.area.center.x);
        float y = Mathf.Abs(area.center.y - sysEnd.area.center.y);
        float z = Mathf.Abs(area.center.z - sysEnd.area.center.z);

        return x + y + z;
    }

}

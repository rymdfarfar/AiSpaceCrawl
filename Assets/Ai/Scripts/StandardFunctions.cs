using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardFunctions : MonoBehaviour
{
   public static StandardFunctions instance;

	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public bool Timer(ref float t, float maxT)
    {

        t += Time.deltaTime;
        if (t > maxT)
        {
            t = 0;

            return true;

        }
        else
            return false;
    }
}

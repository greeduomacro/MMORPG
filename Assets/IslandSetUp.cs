using UnityEngine;
using System.Collections;

public class IslandSetUp : MonoBehaviour 
{
	void Update () 
    {
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if(t != this.transform)
            {
                t.tag = this.tag;
            }
        }
	}
}

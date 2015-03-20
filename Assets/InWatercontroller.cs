using UnityEngine;
using System.Collections;

public class InWatercontroller : MonoBehaviour 
{
    public float touchWaterPosition;
    public bool InWater, IsAtSurface;
    public float swimSpeed = 3.5f, riseSpeed;
    public float ColliderHeight = 0.6f;
    public Vector3 ColliderCenter = new Vector3(0f, 0.4f, 0f);
    public Vector3 MoveDirection = Vector3.zero;


    public float virtical, horizontal, swimup, swimdown;
    void LateUpdate()
    {
        if(GetComponent<Animator>().GetBool(""))
        {
            var v = 0f;
            var f = 0f;
            GetComponent<Animator>().SetBool("IsInWater", InWater);

            if (InWater)
            {
                GetComponent<CharacterController>().center = ColliderCenter;
                GetComponent<CharacterController>().height = ColliderHeight;
                GameObject.FindObjectOfType<CameraRig>().targetHeight = 0.2f;

                if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("swim"))
                {
                    GetComponent<Animator>().Play("swim", 0);
                }

                v = virtical * swimSpeed;
                MoveDirection = new Vector3(0, f, 0);
                transform.Translate(Vector3.forward * v);

                GetComponent<CharacterController>().Move(MoveDirection);

                if (!IsAtSurface && InWater)
                {
                    if (swimup > 0)
                    {
                        f = swimup * riseSpeed;
                    }
                }
                else if (InWater)
                {
                    if (swimdown > 0)
                    {
                        f = swimdown * riseSpeed;
                    }
                }
            }
            else
            {

                // Return everything
                IsAtSurface = InWater;
                GetComponent<CharacterController>().center = new Vector3(0, 0.71f, 0);
                GetComponent<CharacterController>().height = 1.5f;
            }
        }

    }


    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.layer == 4) { if (col.name == "WaterSurface") { IsAtSurface = true; } InWater = true; } 
    }

    void OnTriggerExit (Collider col)
    {
        if (col.name == "WaterSurface") { IsAtSurface = false; }
        if (col.name == "WaterBody") { InWater = false; IsAtSurface = InWater; GameObject.FindObjectOfType<CameraRig>().targetHeight = 1.2f; }
    }
    
}

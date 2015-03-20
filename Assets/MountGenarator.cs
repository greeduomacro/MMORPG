using UnityEngine;
using System.Collections;

public class MountGenarator : MonoBehaviour 
{
    public GameObject Mount;
    public GameObject Effect;
    public float Delay = 1f;
	public void Genarate (Transform player) 
    {
        Mount = Resources.Load("_Okami Gamez C#/Prefabs/Characters/Animals/Dragon@Mount") as GameObject;
        Effect = Resources.Load("_Okami Gamez C#/Prefabs/Misc/MG Effect") as GameObject;

        GameObject e = Instantiate(Effect, transform.position, Quaternion.identity) as GameObject;
        GameObject m = Instantiate(Mount, transform.position, player.transform.rotation) as GameObject;
        m.GetComponent<DragonMountControl>().Owner = player.transform;
        player.GetComponent<MovementScript>().Mount = m;
        
        m.GetComponent<DragonMountControl>().Mount();

        Destroy(e, Delay);
        Destroy(this.gameObject, Delay + 2f);
	}

    public void DeGenarate(Transform player)
    {
        Mount = player.GetComponent<MovementScript>().Mount;
        
        Effect = Resources.Load("_Okami Gamez C#/Prefabs/Misc/MG Effect") as GameObject;
        player.GetComponent<MovementScript>().Mount = null;
        var e = Instantiate(Effect, transform.position, Quaternion.identity);
        Mount.GetComponent<DragonMountControl>().DisMount();
        Destroy(Mount, Delay);
        Destroy(e, Delay);
        Destroy(this.gameObject, Delay + 2f);
    }
}

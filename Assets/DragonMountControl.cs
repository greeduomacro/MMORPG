using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DragonMountControl : MonoBehaviour 
{
    public Transform Owner;
    public bool Alive = true, Awake = false;
    public float Vertical;
    public float horizontal;

    public bool land;
    public bool FlyUp, FlyDown;
    public bool enterexit;


    public float ForwordSpeed = 3.5f;
    public float RiseSpeed = 3.5f;

    public AudioClip WingSound_DownToUp, WingSound_UpToDown;

    public Transform MountSeat;
    public Vector3 seatoffset = new Vector3(0f, 0.5f, 1f);
    private Vector3 targetPostition;
    public bool mounted;
    public float FloatForce;
    public void Mount ()
    {
        if(Owner)
        {
            Debug.Log("Mounting " + Owner.name);
            Owner.GetComponent<Animator>().SetBool("IsOnMount", true);
            mounted = true;
        }
    }
    public void DisMount ()
    {
        if(Owner)
        {
            Debug.Log("Dismounting " + Owner.name);
            Owner.GetComponent<Animator>().SetBool("IsOnMount", false);
            mounted = false;
        }
    }
    

    public void NewEvent() { }
    public void WingSound() { }


    public void WingSoundUp() 
    {
        Transform source = transform.GetComponentInChildren<AudioSource>().transform;
        AudioSource.PlayClipAtPoint(WingSound_DownToUp, source.position, 0.2f);
    }
    public void WingSoundDown()
    {
        Transform source = transform.GetComponentInChildren<AudioSource>().transform;
        AudioSource.PlayClipAtPoint(WingSound_UpToDown, source.position, 0.2f);
    }

    void Update()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Alive", Alive);
        anim.SetBool("Awake", Awake);
        anim.SetFloat("Speed", Vertical);

        if(mounted)
        {
           if(Owner)
           {

               Vertical = Owner.GetComponent<MovementScript>().mountVertical;
               horizontal = Owner.GetComponent<MovementScript>().mounthorizontal;
               FlyUp = Owner.GetComponent<MovementScript>().mountFlyUp;
               FlyDown = Owner.GetComponent<MovementScript>().mountFlyDown;
               enterexit = Owner.GetComponent<MovementScript>().mountenterexit;
               land = Owner.GetComponent<MovementScript>().mountland;

               if (Owner.parent == MountSeat)
               {
                   Owner.localPosition = Vector3.zero;
                   Owner.localRotation = Quaternion.Euler(Vector3.zero);
               }
               else
               {
                   Owner.parent = MountSeat;
               }
           }
        }else if(!mounted)
        {
            if(Owner)
            {
                if (Owner.parent == MountSeat)
                {
                    Owner.parent = null;
                }
            }
        }

        Transform[] trans = GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans)
        {
            if (t.name.Contains("TailFire"))
            {
                t.gameObject.SetActive(Awake);
            }
        }



        if (Alive && !Awake)
        {
            if(FlyUp)
            {
                Awake = true;
            }
        }
        else if(Alive && Awake)
        {
            anim.SetBool("GoInAir", FlyUp) ;
            anim.SetBool("Landing", land);

            GetComponent<CharacterController>().enabled = !anim.GetCurrentAnimatorStateInfo(0).IsName("Brain Tree");

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Brain Tree"))
            {
                anim.SetLayerWeight(1, 1);
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Brain Tree"))
            {
                anim.SetLayerWeight(1, 0);
                if(FlyUp)
                {
                    if (transform.position.y < 500f)
                    {
                        transform.Translate(Vector3.up * RiseSpeed * Time.deltaTime);
                    }
                    
                }
                else if (FlyDown)
                {
                    transform.Translate(-Vector3.up * RiseSpeed * Time.deltaTime);
                }
                transform.Translate(Vector3.forward * Vertical * ForwordSpeed * Time.deltaTime);
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0), 0.25f);
            }

            if (MountSeat)
            {

                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name.Contains("Spine2"))
                    {
                        MountSeat.position = t.position + seatoffset;
                    }
                }

            }
            else
            {
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name.Contains("Seat"))
                    {
                        MountSeat = t;
                    }
                }
            }
        }
    }
}

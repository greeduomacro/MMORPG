using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour 
{
    public float 
        jumpSpeed  = 18.0f, 
        gravity = 32.0f,
        Sgravity = 0.15f,
        runSpeed = 15.0f,
        walkSpeed = 45.0f,
        rotateSpeed = 150.0f;
 
    public  Vector3 moveDirection = Vector3.zero;
    Vector3 targetPostition;
    public bool isWalking = false;
    public string moveStatus = "idle";
 
    public bool dead = false;
    public bool IsGrounded ()
    {
        RaycastHit hit;
        Vector3 rayOrigen = transform.FindChild("RaycastSender").position;
        if(Physics.Raycast(rayOrigen, -Vector3.up, out hit, 1f))
        {
            Debug.DrawLine(rayOrigen, hit.point, Color.yellow);
            return true;
        }else
        {
            return false;
        }
    }

    public void ExitJump ()
    {
        GetComponent<Animator>().SetBool("Jump", false);    
    }

    public LayerMask WaterMaks, LandMask;
    public GameObject Mount;

    public float mountVertical;
    public float mounthorizontal;
    public bool mountFlyUp;
    public bool mountFlyDown;
    public bool mountenterexit;
    public bool mountland;


    public void Update()
    {
        if (dead == false)
        {
            if(GetComponent<Animator>().GetBool("IsInWater"))
            {
                InWatercontroller extracontrol = GetComponent<InWatercontroller>();
                extracontrol.virtical = Input.GetAxis("Vertical");
                extracontrol.horizontal = Input.GetAxis("Horizontal");
                extracontrol.swimup = Input.GetAxis("SwimUp");
                extracontrol.swimdown = Input.GetAxis("SwimDown");

                if (Input.GetAxis("Vertical") > 0)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0), 0.25f);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    targetPostition = Vector3.Slerp(targetPostition, new Vector3(Camera.main.transform.position.x,
                                        this.transform.position.y,
                                        Camera.main.transform.position.z), 0.25f);
                    this.transform.LookAt(targetPostition);
                }
            }
            else if (GetComponent<Animator>().GetBool("IsOnMount")) 
            {
                GetComponent<InWatercontroller>().enabled = false;
                GetComponent<CharacterController>().center = new Vector3(0, 0.16f, 0);
                GetComponent<CharacterController>().height = 0.96f;
                DragonMountControl extracontrol = Mount.GetComponent<DragonMountControl>();
                if(transform.parent)
                {
                    if (transform.parent == extracontrol.MountSeat)
                    {
                        mountVertical = Input.GetAxis("Vertical");
                        mounthorizontal = Input.GetAxis("Horizontal");
                        mountFlyUp = Input.GetButton("FlyUp");
                        mountFlyDown = Input.GetButton("FlyDown");
                        mountenterexit = Input.GetButton("Get On/Off");
                        mountland = Input.GetButton("Land");
                    }
                }

            }else if(!GetComponent<Animator>().GetBool("IsInWater") || !GetComponent<Animator>().GetBool("IsOnMount"))
            {
                GetComponent<InWatercontroller>().enabled = true;
                GetComponent<CharacterController>().center = new Vector3(0, 0.71f, 0);
                GetComponent<CharacterController>().height = 1.5f;
                // Only allow movement and jumps while grounded
                if (IsGrounded() || !IsGrounded())
                {
                    moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0), 0, Input.GetAxis("Vertical"));
                    // if moving forward and to the side at the same time, compensate for distance
                    // TODO: may be better way to do this?
                    if (Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
                    {
                        moveDirection *= 0.7f;
                    }
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= isWalking ? walkSpeed : runSpeed;

                    moveStatus = "idle";
                    if (moveDirection != Vector3.zero)
                        moveStatus = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).ToString();

                    // Jump!
                    //if(Input.GetButton("Jump"))
                    GetComponent<Animator>().SetBool("Jump", Input.GetKeyDown(KeyCode.Space));

                    // Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down.
                    if (Input.GetMouseButton(1))
                    {
                        this.transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
                    }
                    else
                    {
                        if (Input.GetAxis("Vertical") > 0)
                        {
                            this.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                        }
                        else if (Input.GetAxis("Vertical") < 0)
                        {
                            Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
                                                this.transform.position.y,
                                                Camera.main.transform.position.z);
                            this.transform.LookAt(targetPostition);
                        }
                    }
                    //Move controller
                    GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(moveDirection.z));
                }
            }
        }
    }
}

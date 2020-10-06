using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Controller : MonoBehaviour
{
    #region Own Classes
    [System.Serializable] public class PlayerStats
    {
        [Range(0, 200)]
        public float moveSpeed = 0;
        [Space]
        [Range(1, 3)]
        public float runMultiplier = 1;
        public KeyCode runKey = KeyCode.LeftShift;
        [Space]
        public GameObject playerBody;
    }

    [System.Serializable] public class InfoBools
    {
        [HideInInspector] public enum PlayerMood { idle, walking, running, melee_ATK, range_ATK };
        public PlayerMood player_mood;
        [Space]
        public bool isIdle; //true if isWalking, isDead are false
        public bool isWalking = false; //true if horizontal or vertical axis are not equal with Vector3.zero. Function Movement() controls this bool
        public bool isRunning = false; //true if run key is pressed while isWalking = true. Function CanRun() controls this bool
        public bool isMeleeATK = false; //true if player is equiped with no weapon (fists) or if player uses melee attack of a ranged weapon
        public bool isRangeATK = false; //true if player is equiped with a range weapon and uses the ranged attack of that weapon
        [Space]
        public int layer_mask_for_CameraRay;
    }

    #endregion


    #region Public Variables
    public PlayerStats playerStats = new PlayerStats();
    [Space]
    [Space]
    public InfoBools infoBools = new InfoBools();
    #endregion


    #region Private Variables
    private Rigidbody rb;
    private Animator anim;
    private F_HEALTH __script_HP;

    private float horizontal_Movement = 0;
    private float vertical_Movement = 0;
    private Vector3 moveDirection;

    private float cooldownDrop = 0.5f;//used in function      Lifeline_Span()
    private float nextCooldownDrop = 0;//used in function  Lifeline_Span()
    [SerializeField] private float pouringContainer = 0;//used in function  Lifeline_Span()

    //private Vector3 directionDown; //used for raycast to check if player is airborne or not

    //private float rotation_X = 0; //used by nested class CameraWorks
    //private float rotation_Y = 0; //used by nested class CameraWorks

    //private float minValue;
    #endregion



    #region Pre-defined functions
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        __script_HP = gameObject.GetComponent<F_HEALTH>();
        infoBools.layer_mask_for_CameraRay = LayerMask.GetMask("Water");

    }//Start


    private void Update()
    {
        Lifeline_Span();
        Attacking();

        MouseRotating();

    }//Update


    void FixedUpdate()
    {
        Movement();

    }//Update
    #endregion



    #region Own Functions

    private void Movement()
    {
        if (pouringContainer != 0) return;

        if (CanRun() != infoBools.isRunning) infoBools.isRunning = CanRun();// make sure isRunning is equals with what is returned by the CanRun function


        if (GetDirection() != Vector3.zero)// if input is received
        {
            infoBools.isWalking = true;// input detected, is walking            

            Vector3 y_fixVelocity = new Vector3(0, rb.velocity.y, 0); //temp Vector3 variable with x and z 0 and y controlled by rigidbody
            rb.velocity = GetDirection() * SpeedDecision() * Time.deltaTime;
            rb.velocity += y_fixVelocity; //add the temp Vector3 to the rb.velocity to allow rigidbody to control y

            if(infoBools.isRunning == false)
            {
                anim.SetInteger("Pain", 1);
            }
            else
            {
                anim.SetInteger("Pain", 2);
            }
        }


        if (GetDirection() == Vector3.zero)// if no input is received
        {
            infoBools.isWalking = false;// no input, so it's no longer walking
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.SetInteger("Pain", 0);
        }

    }//Movement


    private void Attacking()
    {
        if (Input.GetKey(KeyCode.Mouse0) && pouringContainer == 0)
        {
            infoBools.isWalking = false;// no input, so it's no longer walking
            infoBools.isRunning = false;// no walk means no run
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            infoBools.isMeleeATK = true;

            int randomAttack = Random.Range(3, 6);

            if(randomAttack == 3)
            {
                pouringContainer += 3.15f;
                anim.SetInteger("Pain", 3);
            }

            if (randomAttack == 4)
            {
                pouringContainer += 1.65f;
                anim.SetInteger("Pain", 4);
            }

            if (randomAttack == 5)
            {
                pouringContainer += 2.265f;
                anim.SetInteger("Pain", 5);
            }

        }



    }//Attacking


    public void Lifeline_Span()//things like attacking that have long animations will add their duration here to prevent Idle animation taking over until the timer is 0 again
    {
        if (pouringContainer == 0) return;
        if (pouringContainer < 0) pouringContainer = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) pouringContainer -= pouringContainer;

        if(Time.time > nextCooldownDrop)
        {
            nextCooldownDrop = Time.time + cooldownDrop;
            pouringContainer -= cooldownDrop;
        }

        if (pouringContainer <= 0)
        {
            infoBools.isMeleeATK = false;
            infoBools.isRangeATK = false;
        }

    }//Lifeline_Span


    private void MouseRotating()
    {
        if (infoBools.isMeleeATK) return;

        //if (__script_HP.curr_valor_HP <= 0) return;

        #region Face moving direction
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (infoBools.isWalking)//face walking direction
        {
            Vector3 facingrotation = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
            if (facingrotation != Vector3.zero) playerStats.playerBody.transform.forward = facingrotation;

            return;
        }
        //_______________________________________________________________________
        #endregion

        #region Face Mouse direction when not moving or in range attack
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 50, infoBools.layer_mask_for_CameraRay);
        Vector3 dir = hit.point - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        playerStats.playerBody.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        //_______________________________________________________________________
        #endregion


    }//MouseRotating

    #endregion



    #region Functions that return something

    private Vector3 GetDirection()// function that simply returns the normalized Vector3 that is made from the sum of horizontal and vertical movements
    {
        horizontal_Movement = Input.GetAxisRaw("Horizontal"); //getting the horizontal movement
        vertical_Movement = Input.GetAxisRaw("Vertical"); //getting the vertical movement
        moveDirection = (horizontal_Movement * transform.right + vertical_Movement * transform.forward).normalized; //assembly the 2 movement variables together in this Vector3 and normalize it

        return moveDirection;
    }//GetDirection


    private bool CanRun()// function that returns true or false from bool isRunning
    {
        if (infoBools.isWalking)// if it's already walking
        {
            if (Input.GetKey(playerStats.runKey)) return true;// if runKey is pressed it can run
            else return false;// if runKey is no longer pressed, it cannot run
        }
        else// if it's no longer walking
        {
            return false;// it cannot run even if runKey is pressed
        }

    }//CanRun


    private float SpeedDecision()// return the speed to be used to move according to isRunning bool
    {
        if (infoBools.isRunning) return playerStats.moveSpeed * playerStats.runMultiplier;
        else return playerStats.moveSpeed * (playerStats.runMultiplier / playerStats.runMultiplier);

    }//SpeedDecision

    #endregion


}

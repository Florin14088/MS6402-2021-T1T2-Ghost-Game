using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class F_UserControlBoard : MonoBehaviour
{
    //in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you

    #region PUBLIC VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public KeyCode runKey = KeyCode.LeftShift;
    [Space]
    public float attackDuration = 2;
    //_______________________________________________________________________
    #endregion


    #region PRIVATE VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private F_CharacterController __script_F_CharCont;//this script works in pair with this script
    private Transform mainCamera;//you need ONLY ONE active camera with tag "MainCamera"
    private Animator anim;

    private Vector3 where_Cam_face_Forward;
    private Vector3 movement;

    private float cooldownDrop = 0.5f;//used in function Lifeline_Span()
    private float nextCooldownDrop = 0;//used in function Lifeline_Span()
    private float pouringContainer = 0;//used in function Lifeline_Span()

    private float horizontal_Input;
    private float vertical_Input;
    //_______________________________________________________________________
    #endregion
        


    #region Start()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void Start()
    {
        mainCamera = Camera.main.transform;
        __script_F_CharCont = GetComponent<F_CharacterController>();
        anim = GetComponent<Animator>();

    }//Start
     //_______________________________________________________________________
    #endregion


    #region Update()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void Update()
    {
        Lifeline_Span();

        if (Input.GetKey(KeyCode.Mouse0) && pouringContainer == 0)
        {
            pouringContainer += attackDuration;
            anim.SetInteger("Pain", 1);
        }
    }
    //_______________________________________________________________________
    #endregion


    #region Fixed Update()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void FixedUpdate()
    {
        horizontal_Input = Input.GetAxis("Horizontal");
        vertical_Input = Input.GetAxis("Vertical");

        
        if (mainCamera != null)
        {            
            where_Cam_face_Forward = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
            movement = vertical_Input * where_Cam_face_Forward + horizontal_Input * mainCamera.right;
        }
        else movement = vertical_Input * Vector3.forward + horizontal_Input * Vector3.right;


        //unless Run key is pressed, move half speed
        if (!Input.GetKey(runKey)) movement *= 0.5f;

        
        __script_F_CharCont.Behaviour_Movement(movement);

    }//FixedUpdate
     //_______________________________________________________________________
    #endregion

    #region My Functions
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public void Lifeline_Span()//things like attacking that have long animations will add their duration here to prevent Idle animation taking over until the timer is 0 again
    {
        if (pouringContainer == 0) return;
        if (pouringContainer < 0) pouringContainer = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) pouringContainer -= pouringContainer;

        if (Time.time > nextCooldownDrop)
        {
            nextCooldownDrop = Time.time + cooldownDrop;
            pouringContainer -= cooldownDrop;
        }

        if (pouringContainer <= 0)
        {
            anim.SetInteger("Pain", 0);
            //infoBools.isMeleeATK = false;
            //infoBools.isRangeATK = false;
        }

    }//Lifeline_Span
     //_______________________________________________________________________
    #endregion


}//END

//in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you
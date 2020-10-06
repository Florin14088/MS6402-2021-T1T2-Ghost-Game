using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class F_CharacterController : MonoBehaviour
{
    //in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you

    #region PUBLIC VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public float m_MovingTurnSpeed = 360;
    public float m_StationaryTurnSpeed = 180;
    [Space]
    [HideInInspector] public float m_AnimSpeedMultiplier = 1f;
    [Space]
    [HideInInspector] public float m_RunCycleLegOffset = 0.2f;
    [HideInInspector] public float m_GroundCheckDistance = 0.1f;
    //_______________________________________________________________________
    #endregion


    #region PRIVATE VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private Rigidbody rb;
    private Animator anim;

    private bool b_grounded;

    private const float kHalf_constant = 0.5f;
    private float amount_turning;
    private float amount_forward;

    private Vector3 groundNorm;

    private RaycastHit hitInfo;
    //_______________________________________________________________________
    #endregion




    #region Start()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }//Start
     //_______________________________________________________________________
    #endregion


    #region My Functions
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public void Behaviour_Movement(Vector3 __movement__)
    {
        if (__movement__.magnitude > 1f) __movement__.Normalize();

        __movement__ = transform.InverseTransformDirection(__movement__);

        CheckGroundStatus();

        __movement__ = Vector3.ProjectOnPlane(__movement__, groundNorm);

        amount_turning = Mathf.Atan2(__movement__.x, __movement__.z);
        amount_forward = __movement__.z;

        ApplyExtraTurnRotation();
        AminatorManagement(__movement__);

    }//Behaviour_Movement

    

    void AminatorManagement(Vector3 __movement__)
    {
        anim.SetFloat("Forward", amount_forward, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", amount_turning, 0.1f, Time.deltaTime);


        if (b_grounded && __movement__.magnitude > 0) anim.speed = m_AnimSpeedMultiplier;
        else anim.speed = 1;

    }//AminatorManagement

    

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, amount_forward);
        transform.Rotate(0, amount_turning * turnSpeed * Time.deltaTime, 0);

    }//ApplyExtraTurnRotation

    

    void CheckGroundStatus()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            groundNorm = hitInfo.normal;
            b_grounded = true;
            anim.applyRootMotion = true;
        }
        else
        {
            b_grounded = false;
            groundNorm = Vector3.up;
            anim.applyRootMotion = false;
        }

    }//CheckGroundStatus
    //_______________________________________________________________________
    #endregion



}//END

//in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you

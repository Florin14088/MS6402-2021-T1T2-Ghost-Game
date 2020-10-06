using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class F_Possession_Change : MonoBehaviour
{
    public bool b_Switch_to_Player_Controll = false; //true = player controlls the Object       false = AI controlls the Object
    
    private Rigidbody rb;
    private NavMeshObstacle obstacle;
    private NavMeshAgent agent;
    private F_AI_SimplePain __script_AI;

    private F_CharacterController __CharCont;
    private F_UserControlBoard __UserCont;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
        agent = GetComponent<NavMeshAgent>();
        __script_AI = GetComponent<F_AI_SimplePain>();
        __CharCont = GetComponent<F_CharacterController>();
        __UserCont = GetComponent<F_UserControlBoard>();


        if (b_Switch_to_Player_Controll == false)
        {
            __CharCont.enabled = false;
            __UserCont.enabled = false;

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

            __script_AI.enabled = true;
            agent.enabled = true;
            obstacle.enabled = true;

            this.enabled = false;
            return;

        }


        if (b_Switch_to_Player_Controll)
        {
            __script_AI.enabled = false;
            agent.enabled = false;
            obstacle.enabled = false;

            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;


            __CharCont.enabled = true;
            __UserCont.enabled = true;

            this.enabled = false;
            return;
        }


    }//Start



    private void OnEnable()
    {
        Start();

    }//OnEnable


       
}//END

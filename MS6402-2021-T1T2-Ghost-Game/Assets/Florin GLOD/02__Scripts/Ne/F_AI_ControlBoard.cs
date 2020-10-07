using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region 
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾

//_______________________________________________________________________
#endregion

#region Require Components
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(F_CharacterController))]
[RequireComponent(typeof(Animator))]
//_______________________________________________________________________
#endregion
public class F_AI_ControlBoard : MonoBehaviour
{
    #region PUBLIC VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    #region Get Set Stuff
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public NavMeshAgent agent { get; private set; }
    public Animator anim { get; private set; }
    public NavMeshObstacle obstacle { get; private set; }
    public F_CharacterController __script_CharCont { get; private set; }
    //_______________________________________________________________________
    #endregion

    public Transform target;
    public List<string> target_Tags;
    [Space]
    [Space]
    public float speed;
    public float rotSpeed;
    public bool b_canRun = true;
    [Space]
    [Space]
    public bool b_canWander = false;//true = when no target the AI will randomly wander     false = when no target the AI will be static idle
    public float wanderRadius;
    public float wanderTimer;
    public float destinationProximity;
    [Space]
    [Space]
    public float Pursue_Distance;//target closer than this distance = run to target
    public float Attack_Distance;//target closer than this distance = attack target
    [Space]
    [Space]
    public float currentDistance;//current distance between target and this object
    public bool bool_ignoreLimits = false;//true = if got target, will pursue the target until either this object or target die         false = use Pursue_Distance
    [Space]
    [Space]
    public GameObject[] damageDealers_Melee;//the objects child to this object that have F_MeleeDamage.cs script
    public int meleeDamage = 3;//basic melee damage that will be used to randomise the damage per hit
    public float attackMelee_Cooldown = 2.5f;//how long takes 1 attack 
    [HideInInspector] public float nextAttkMelee_Cooldown = 0;//works with attackMelee_Cooldown in cooldown logic

    //_______________________________________________________________________
    #endregion


    #region PRIVATE VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private float cooldownDrop = 0.5f;//used in function Lifeline_Span()
    private float nextCooldownDrop = 0;//used in function Lifeline_Span()
    private float pouringContainer = 0;//used in function Lifeline_Span()

    private float cooldownCalcDist = 0.3f;
    private float next_CalcDist = 0;

    private float timer;//used in random wander
    private Vector3 newPos;//same as above
    //_______________________________________________________________________
    #endregion




    #region START
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private void Start()
    {
        #region Get components
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        agent = GetComponentInChildren<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        obstacle = GetComponentInChildren<NavMeshObstacle>();
        __script_CharCont = GetComponent<F_CharacterController>();
        //_______________________________________________________________________
        #endregion

        #region Modify parameters
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        agent.updateRotation = false;
        agent.updatePosition = true;

        agent.angularSpeed = rotSpeed;
        agent.speed = speed;
        //_______________________________________________________________________
        #endregion

    }//Start
     //_______________________________________________________________________
    #endregion




    #region UPDATE
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private void Update()
    {
        //if (target != null)
        //    agent.SetDestination(target.position);

        //if (agent.remainingDistance > agent.stoppingDistance)
        //    __script_CharCont.Behaviour_Movement(agent.desiredVelocity);
        //else
        //    __script_CharCont.Behaviour_Movement(Vector3.zero);
        AI_Brain();

    }//Update
     //_______________________________________________________________________
    #endregion





    #region My Functions
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void AI_Brain()
    {
        #region AI with not target
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (target == null)
        {
            if (b_canWander)
            {
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    timer = 0;
                    newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                }

                if (agent.remainingDistance > destinationProximity)
                {
                    __script_CharCont.Behaviour_Movement(agent.desiredVelocity * 0.5f);
                }
                else __script_CharCont.Behaviour_Movement(Vector3.zero);

            }

        }
        //_______________________________________________________________________
        #endregion



        #region AI with target
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (target)
        {

        }
        //_______________________________________________________________________
        #endregion




    }//AI_Brain



    public void Lifeline_Span()//things like attacking that have long animations will add their duration here to prevent Idle animation taking over until the timer is 0 again
    {
        if (pouringContainer == 0) return;
        if (pouringContainer < 0) pouringContainer = 0;

        if (Time.time > nextCooldownDrop)
        {
            nextCooldownDrop = Time.time + cooldownDrop;
            pouringContainer -= cooldownDrop;
        }

        if (pouringContainer <= 0)
        {
            anim.SetInteger("Pain", 0);
        }

    }//Lifeline_Span



    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;

    }//RandomNavSphere



    public void PickTarget(Transform target)
    {
        this.target = target;

    }//PickTarget
    //_______________________________________________________________________
    #endregion


}//END


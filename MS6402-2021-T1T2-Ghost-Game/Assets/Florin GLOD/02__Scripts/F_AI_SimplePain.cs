using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class F_AI_SimplePain : MonoBehaviour
{
    public GameObject target; //or other target
    public List<string> target_Tags;
    [Space]
    [Space]
    public bool b_NPC_Coward = false;
    [Space]
    [Space]
    public float speed;
    public float rotSpeed;
    public bool b_canRun = true;
    [Space]
    public float Pursue_Distance;
    public float Attack_Distance;
    [Space]
    public float currentDistance;
    public bool bool_ignoreLimits = false;
    [Space]
    public GameObject[] damageDealers_Melee;
    public int meleeDamage = 3;
    public float attackMelee_Cooldown = 2.5f;
    [HideInInspector] public float nextAttkMelee_Cooldown = 0;


    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private Animator anim;
    private float cooldownCalcDist = 0.3f;
    private float next_CalcDist = 0;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = false;
        agent.enabled = true;
        agent.speed = speed;
        agent.angularSpeed = rotSpeed;
        obstacle.enabled = false;

       // target_Tags.Remove("Faction Zombie");

        meleeDamage = Random.Range(meleeDamage - meleeDamage / 2, meleeDamage + meleeDamage * 2);

        foreach (GameObject g in damageDealers_Melee) g.GetComponent<F_MeleeDamage>().damage = meleeDamage;
        foreach (GameObject g in damageDealers_Melee) g.GetComponent<Collider>().enabled = false;
    }


    void Update()
    {
        if (!target)
        {
            AI_Idling();
            return;
        }

        if (target)
        {
            AI_Brain();
        }


    }//Update()

    //0 = idle      1 = walk        2 = run         3 = attack

    void AI_Brain()
    {
        if (b_NPC_Coward == true)
        {
            if (Time.time > nextAttkMelee_Cooldown * 2)
            {
                agent.SetDestination(RandomNavmeshLocation(40));
            }

            return;//if the NPC is coward, the function will stop here

        }

        #region Calculate distance between target and this object
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (Time.time > next_CalcDist)
        {
            next_CalcDist = Time.time + cooldownCalcDist;
            currentDistance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        }

        if (currentDistance <= Attack_Distance)
        {
            if(agent.enabled == true) agent.enabled = false;
            if(obstacle.enabled == false) obstacle.enabled = true;
        }

        if (currentDistance > Attack_Distance)
        {
            if (obstacle.enabled == true) obstacle.enabled = false;
            if (agent.enabled == false) agent.enabled = true;
        }
        //_______________________________________________________
        #endregion


        #region Target is far away
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (currentDistance > Pursue_Distance && bool_ignoreLimits == false)
        {
            if (anim.GetInteger("Pain") != 0) anim.SetInteger("Pain", 0);//idle
            return;
        }

        if (currentDistance > Pursue_Distance && bool_ignoreLimits == true)
        {
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                if (anim.GetInteger("Pain") != 0) anim.SetInteger("Pain", 0);//idle
                if (agent.destination != gameObject.transform.position) agent.SetDestination(gameObject.transform.position);
            }

            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                if (anim.GetInteger("Pain") != 2) anim.SetInteger("Pain", 2);//run
                if (agent.destination != target.transform.position) agent.SetDestination(target.transform.position);
            }
        }
        //________________________
        #endregion


        #region Target within pursue distance && Not enough to attack
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (currentDistance <= Pursue_Distance && currentDistance > Attack_Distance)
        {
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                if (anim.GetInteger("Pain") != 0) anim.SetInteger("Pain", 0);//idle
                if (agent.destination != gameObject.transform.position) agent.SetDestination(gameObject.transform.position);
            }

            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                if (anim.GetInteger("Pain") != 2) anim.SetInteger("Pain", 2);//run
                if (agent.destination != target.transform.position) agent.SetDestination(target.transform.position);
            }

        }
        //___________________________________________________________
        #endregion


        #region Target within attack range
        //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
        if (currentDistance <= Attack_Distance)
        {
            Vector3 relativePos = target.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5 * Time.deltaTime);

            if (anim.GetInteger("Pain") != 3) anim.SetInteger("Pain", 3);//attack

            if (Time.time > nextAttkMelee_Cooldown)
            {
                nextAttkMelee_Cooldown = Time.time + attackMelee_Cooldown;
                foreach (GameObject g in damageDealers_Melee) g.GetComponent<Collider>().enabled = true;
            }
        }
        //________________________________
        #endregion

    }//AI_Brain


    void AI_Idling()
    {
        if (agent.enabled) agent.enabled = false;
        if (obstacle.enabled == false) obstacle.enabled = true;
        if (anim.GetInteger("Pain") != 0) anim.SetInteger("Pain", 0);
        //if (agent.destination != gameObject.transform.position) agent.SetDestination(gameObject.transform.position);

    }//AI_Idling


    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }


}//END


#region 
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾

//_______________________________________________________________________
#endregion
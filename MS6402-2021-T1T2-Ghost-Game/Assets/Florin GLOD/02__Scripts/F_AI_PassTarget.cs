using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_AI_PassTarget : MonoBehaviour
{
    
    public List<string> enemyTags;


    private void OnTriggerStay(Collider other)
    {
        if (gameObject.GetComponentInParent<F_AI_ControlBoard>().enabled == false) return;

        enemyTags = GetComponentInParent<F_AI_SimplePain>().target_Tags;

        foreach (string s in enemyTags)
        {
            if(other.gameObject.transform.root.gameObject.tag == s)
            {
                if (GetComponentInParent<F_AI_SimplePain>().target == null)
                {
                    GetComponentInParent<F_AI_SimplePain>().target = other.gameObject.transform.root.gameObject;
                }


                if (Vector3.Distance(gameObject.transform.root.transform.position, other.gameObject.transform.root.gameObject.transform.position) < Vector3.Distance(gameObject.transform.root.transform.position, GetComponentInParent<F_AI_SimplePain>().target.transform.position))
                {
                    GetComponentInParent<F_AI_SimplePain>().target = other.gameObject.transform.root.gameObject;
                }


            }



        }


    }//OnTriggerStay


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.root.gameObject == GetComponentInParent<F_AI_SimplePain>().target && GetComponentInParent<F_AI_SimplePain>().bool_ignoreLimits == false)
        {
            GetComponentInParent<F_AI_SimplePain>().target = null;

        }
    }//OnTriggerExit


}//END
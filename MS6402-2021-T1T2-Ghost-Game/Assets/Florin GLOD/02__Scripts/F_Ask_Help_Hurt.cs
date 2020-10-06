using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Ask_Help_Hurt : MonoBehaviour
{
    public float inflatePerTurn = 0.01f;
    public float maxDistance = 10;
    public bool b_Begin = false;
    public GameObject aggresor;

    void Update()
    {
        if (b_Begin)
        {
            if (gameObject.transform.parent != null) gameObject.transform.parent = null;
            gameObject.transform.localScale += new Vector3(inflatePerTurn, inflatePerTurn, inflatePerTurn);
        }

        if (gameObject.transform.localScale.x > maxDistance) Destroy(gameObject);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<F_AI_SimplePain>())
        {
            if(other.gameObject.GetComponent<F_AI_SimplePain>().target == null)
            {
                if(aggresor != other.gameObject)
                {
                    other.gameObject.GetComponent<F_AI_SimplePain>().target = aggresor;
                }
            }
        }
    }

}

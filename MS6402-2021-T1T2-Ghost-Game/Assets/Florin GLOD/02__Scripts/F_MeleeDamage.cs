using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_MeleeDamage : MonoBehaviour
{
    public int damage = 2;
    public bool b_ignoreArmor = false;
    public bool b_infiniteHits = false;//true = the collider never get's deactivated
    [Space]
    [Space]
    public bool b_needHelper = true;
    public GameObject helperRadius;
    [Space]
    public string[] noHelper_IgnoreTags;//if no helper is used but still don't want to damage everything with hp

    private int containerAvailable_Damage = 0;//this is the variable that will be manipulated by script.
    private int containerPartial_Damage = 0;//when the armor cannot take all damage, this value will be used to consume all available armor, and the rest of the damage will go to health


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<F_Ask_Help_Hurt>() && other.gameObject.GetComponentInChildren<F_Ask_Help_Hurt>().b_Begin == false &&
            other.gameObject.GetComponent<F_AI_SimplePain>().enabled == true)
        {
            other.gameObject.GetComponentInChildren<F_Ask_Help_Hurt>().aggresor = gameObject.transform.root.transform.gameObject;
            other.gameObject.GetComponentInChildren<F_Ask_Help_Hurt>().b_Begin = true;
        }


        if (b_needHelper)
        {
            foreach (string s in helperRadius.GetComponent<F_AI_PassTarget>().enemyTags)
            {
                if (other.gameObject.transform.root.gameObject.tag == s && other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>())
                {
                    //Debug.Log("Hit Mechanic from   " + gameObject.name);
                    containerAvailable_Damage = damage;
                    if (b_ignoreArmor == false)//if damage can be sent to armor first
                    {
                        if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM > 0)//if there is armor available
                        {

                            if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM >= containerAvailable_Damage)//can take all damage using armor
                            {
                                other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM -= containerAvailable_Damage;//damage sent to armor
                                containerAvailable_Damage = 0;//damage to be received is zero
                            }


                            if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM < containerAvailable_Damage)//not all damage can be received by armor
                            {
                                containerPartial_Damage = other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM;//how much armor is available is how much damage is substracted from total
                                containerAvailable_Damage -= containerPartial_Damage;//how much damage is left to be sent to health
                                other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM = 0;//all armor used up
                            }


                        }//armor value > 0


                    }//ignore armor?


                    other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_HP -= containerAvailable_Damage;//all damage available in this variable is sent to health


                    if (other.gameObject.transform.root.gameObject.GetComponent<F_AI_SimplePain>().target == null)
                    {
                        other.gameObject.transform.root.gameObject.GetComponent<F_AI_SimplePain>().target = gameObject.transform.root.gameObject;
                        other.gameObject.transform.root.gameObject.GetComponent<F_AI_SimplePain>().target_Tags.Add(gameObject.transform.root.gameObject.tag);
                    }

                    if(b_infiniteHits == false) gameObject.GetComponent<Collider>().enabled = false;//prevent giving damage multiple time with one hit
                }
            }
        }//need helper


        if(b_needHelper == false)
        {
            foreach(string s in noHelper_IgnoreTags)
            {
                if (other.gameObject.transform.root.gameObject.tag == s) return;
            }


            if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>())
            {
                containerAvailable_Damage = damage;

                if (b_ignoreArmor == false)//if damage can be sent to armor first
                {
                    if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM > 0)//if there is armor available
                    {

                        if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM >= containerAvailable_Damage)//can take all damage using armor
                        {
                            other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM -= containerAvailable_Damage;//damage sent to armor
                            containerAvailable_Damage = 0;//damage to be received is zero
                        }


                        if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM < containerAvailable_Damage)//not all damage can be received by armor
                        {
                            containerPartial_Damage = other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM;//how much armor is available is how much damage is substracted from total
                            containerAvailable_Damage -= containerPartial_Damage;//how much damage is left to be sent to health
                            other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM = 0;//all armor used up
                        }


                    }//armor value > 0


                }//ignore armor?


                other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_HP -= containerAvailable_Damage;//all damage available in this variable is sent to health
                if (b_infiniteHits == false) gameObject.GetComponent<Collider>().enabled = false;//prevent giving damage multiple time with one hit


            }

        }//damage anything with hp
        

    }//OnTriggerEnter

}//END



//if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>() && other.gameObject.transform.root.gameObject.tag == helperRadius.GetComponent<F_AI_PassTarget>().EnemyTag1
//    || other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>() && other.gameObject.transform.root.gameObject.tag == helperRadius.GetComponent<F_AI_PassTarget>().EnemyTag2)
//{
//    containerAvailable_Damage = damage;

//    if(b_ignoreArmor == false)//if damage can be sent to armor first
//    {
//        if (other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM > 0)//if there is armor available
//        {

//            if(other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM >= containerAvailable_Damage)//can take all damage using armor
//            {
//                other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM -= containerAvailable_Damage;//damage sent to armor
//                containerAvailable_Damage = 0;//damage to be received is zero
//            }


//            if(other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM < containerAvailable_Damage)//not all damage can be received by armor
//            {
//                containerPartial_Damage = other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM;//how much armor is available is how much damage is substracted from total
//                containerAvailable_Damage -= containerPartial_Damage;//how much damage is left to be sent to health
//                other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_ARM = 0;//all armor used up
//            }


//        }//armor value > 0


//    }//ignore armor?


//    other.gameObject.transform.root.gameObject.GetComponent<F_HEALTH>().curr_valor_HP -= containerAvailable_Damage;//all damage available in this variable is sent to health

//    gameObject.GetComponent<Collider>().enabled = false;//prevent giving damage multiple time with one hit




//}//got a health script
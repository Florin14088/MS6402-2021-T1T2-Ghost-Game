using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_HEALTH : MonoBehaviour
{
    public int max_valor_HP = 5;//max health
    public int max_valor_ARM = 5;//max armor
    [HideInInspector] public bool b_needRegen_maxHP = false;
    [HideInInspector] public bool b_needRegen_maxARM = false;
    [Space]
    [Space]
    public int curr_valor_HP = 5;//current health
    public int curr_valor_ARM = 5;//current armor
    [Space]
    [Space]
    public bool b_canRegen_HP = false;//master bool, if this is false, no HP regen ever
    [HideInInspector] public bool b_needRegen_HP = false;//true if current HP is lower than max HP
    public float cooldownRegen_HP = 10;
    [HideInInspector] public float nextReg_HP = 0;
    public int regenAmount_HP = 1;
    [Space]
    [Space]
    public bool b_canRegen_ARM = false;//master bool, if this is false, no ARM regen ever
    [HideInInspector] public bool b_needRegen_ARM = false;//true if current ARM is lower than max ARM
    public float cooldownRegen_ARM = 10;
    [HideInInspector] public float nextReg_ARM = 0;
    public int regenAmount_ARM = 1;




    private int temp_cur_HP = 0;
    private int temp_cur_ARM = 0;
    private int temp_max_HP = 0;
    private int temp_max_ARM = 0;




    void Start()
    {
        curr_valor_HP = max_valor_HP;
        curr_valor_ARM = max_valor_ARM;

    }//Start




    void Update()
    {
        UnitManagement_HP();
        Regen_HP();

        UnitManagement_ARM();
        Regen_ARM();

    }//Update



    void UnitManagement_HP()
    {
        if(temp_cur_HP != curr_valor_HP)
        {
            temp_cur_HP = curr_valor_HP;

            if(curr_valor_HP < 0)
            {
                curr_valor_HP = 0;
                temp_cur_HP = curr_valor_HP;
            }


            if(curr_valor_HP == 0)
            {
                Death();
                return;
            }


            if(curr_valor_HP < max_valor_HP)
            {
                b_needRegen_HP = true;
            }


            if (curr_valor_HP > max_valor_HP)
            {
                b_needRegen_HP = false;
                curr_valor_HP = max_valor_HP;
                temp_cur_HP = curr_valor_HP;
            }

        }


        if(temp_max_HP != max_valor_HP)
        {
            temp_max_HP = max_valor_HP;
            temp_cur_HP--;//to trigger the "what happens when saved value of hp is different of the actual value of hp"
        }

    }//UnitManagement_HP



    void Regen_HP()
    {
        if (b_canRegen_HP == false) return;

        if (b_needRegen_HP == false) return;

        if (curr_valor_HP < max_valor_HP)
        {
            if (Time.time > nextReg_HP)
            {
                nextReg_HP = Time.time + cooldownRegen_HP;
                curr_valor_HP += regenAmount_HP;
            }
        }

    }//Regen_HP



    void UnitManagement_ARM()
    {
        if(temp_cur_ARM != curr_valor_ARM)
        {
            temp_cur_ARM = curr_valor_ARM;


            if(curr_valor_ARM < 0)
            {
                curr_valor_ARM = 0;
                temp_cur_ARM = curr_valor_ARM;
            }


            if(curr_valor_ARM == 0)
            {
                Death();
                return;
            }


            if(curr_valor_ARM < max_valor_ARM)
            {
                b_needRegen_ARM = true;
            }


            if (curr_valor_ARM > max_valor_ARM)
            {
                b_needRegen_ARM = false;
                curr_valor_ARM = max_valor_ARM;
                temp_cur_ARM = curr_valor_ARM;
            }



        }


        if (temp_max_ARM != max_valor_ARM)
        {
            temp_max_ARM = max_valor_ARM;
            temp_cur_ARM--;//to trigger the "what happens when saved value of ARM is different of the actual value of ARM"
        }



    }//UnitManagement_ARM



    void Regen_ARM()
    {
        if (b_canRegen_ARM == false) return;

        if (b_needRegen_ARM == false) return;

        if (curr_valor_ARM < max_valor_ARM)
        {
            if(Time.time > nextReg_ARM)
            {
                nextReg_ARM = Time.time + cooldownRegen_ARM;
                curr_valor_ARM += regenAmount_ARM;
            }
        }

    }//Regen_ARM



    void Death()
    {
        if (curr_valor_HP == 0)
        {
            if(gameObject.GetComponent<Player_Controller>()) gameObject.GetComponent<Player_Controller>().enabled = false;
            gameObject.GetComponent<Ragdoll_Switch>().enabled = true;
        }

    }//Death




}//END

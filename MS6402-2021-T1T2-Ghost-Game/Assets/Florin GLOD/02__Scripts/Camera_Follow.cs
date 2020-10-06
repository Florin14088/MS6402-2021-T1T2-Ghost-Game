using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    void Start()
    {
        offset = transform.position - player.position;
    }



    void FixedUpdate()  
    {
        if(player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            return;
        }
        // Follow Transform
        //transform.position = player.position + offset;

        // Move Towards
        //float speed = 3.0f;
        //transform.position = Vector3.MoveTowards(transform.position, player.position + offset, Time.deltaTime * speed);

        // Linear Interpolation
        //float speed = 1.0f;
        //transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * speed);

        //Smooth Damp
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, 0.3f);
        //print(velocity);
    }
}

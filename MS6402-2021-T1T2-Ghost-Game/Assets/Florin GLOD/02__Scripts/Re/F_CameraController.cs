using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_CameraController : MonoBehaviour
{
    //in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you

    #region PUBLIC VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public float sens_X = 50;//sensitivity
    public float sens_Y = 50;
    public GameObject pivot;//do now put player here. Your camera should have a parent. Put that parent here so camera can orbit around it
    [Space]
    [Space]
    public float delay_MoveClip = 0.05f;//This is clip avoidance time, the smaller the value, the faster it goes. 
    public float delay_returnTime = 0.4f;//This is the time it takes to move back to original position. Have to be a bigger value than delay_MoveClip
    public float radius_castCheck = 0.1f;//SphereCast for objects between target and camera
    public float minDist = 0.5f;//min distance between target and camera
    [Space]
    [Space]
    public Transform mainCamera;
    public Transform cam_Pivot;//camera pivots around this
    public float init_dist;//original camera distance. The script will change this
    public bool b_protection {get; private set;}//check if an there is something between camera and player
    public string tag_ignoreClip = "Player";//put here Player/target tag
                                            //_______________________________________________________
    #endregion


    #region PRIVATE VARIABLES
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private float mouse_H;//mouse horizontal
    private float mouse_V;//mouse vertical
    private float camSpeed;//camera speed

    private float curr_camDist;//distance from camera to target/player
    private float targetDist;

    private bool b_initialIntersect = false;
    private bool b_hitSomething = false;
    private float nearest;


    private Ray __ray__ = new Ray();//used in LateUpdate()
    private RaycastHit[] __hits__;
    private RayHitComparer ray_hit_Dist;
    //_______________________________________________________________________
    #endregion


    


    #region START()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        init_dist = mainCamera.localPosition.magnitude;
        curr_camDist = init_dist;

        ray_hit_Dist = new RayHitComparer();

    }//Start
     //_______________________________________________________________________
    #endregion


    #region UPDATE()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    void Update()
    {
        mouse_V = Input.GetAxis("Mouse Y") * -sens_X;
        mouse_H = Input.GetAxis("Mouse X") * sens_Y;

        pivot.transform.eulerAngles += new Vector3(mouse_V * Time.deltaTime, mouse_H * Time.deltaTime, 0);

    }//Update
     //_______________________________________________________________________
    #endregion


    #region LATE UPDATE()
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    private void LateUpdate()
    {
        targetDist = init_dist;

        __ray__.origin = cam_Pivot.position + cam_Pivot.forward * radius_castCheck;
        __ray__.direction = -cam_Pivot.forward;


        //quick check
        Collider[] allCollisions = Physics.OverlapSphere(__ray__.origin, radius_castCheck);


        b_initialIntersect = false;
        b_hitSomething = false;


        //check collisions
        for (int i = 0; i < allCollisions.Length; i++)
        {
            if ((!allCollisions[i].isTrigger) && !(allCollisions[i].attachedRigidbody != null && allCollisions[i].attachedRigidbody.CompareTag(tag_ignoreClip)))
            {
                b_initialIntersect = true;
                break;
            }
        }


        //if collision found
        if (b_initialIntersect)
        {
            __ray__.origin += cam_Pivot.forward * radius_castCheck;            
            __hits__ = Physics.RaycastAll(__ray__, init_dist - radius_castCheck);
        }
        else __hits__ = Physics.SphereCastAll(__ray__, radius_castCheck, init_dist + radius_castCheck);


        //collisions get sorted by distance
        Array.Sort(__hits__, ray_hit_Dist);        
        nearest = Mathf.Infinity;


        // loop all collisions
        for (int i = 0; i < __hits__.Length; i++)
        {
            //manage collision i if it is closer than collision i-1. Also, no trigger and not the objects with the ignore tag
            if (__hits__[i].distance < nearest && (!__hits__[i].collider.isTrigger) && !(__hits__[i].collider.attachedRigidbody != null && __hits__[i].collider.attachedRigidbody.CompareTag(tag_ignoreClip)))
            {
                //nearest collision is moved away in the order
                nearest = __hits__[i].distance;
                targetDist = -cam_Pivot.InverseTransformPoint(__hits__[i].point).z;
                b_hitSomething = true;
            }
        }


        //camera needs to be moved and protected from obstacles
        b_protection = b_hitSomething;
        curr_camDist = Mathf.SmoothDamp(curr_camDist, targetDist, ref camSpeed, curr_camDist > targetDist ? delay_MoveClip : delay_returnTime);
        curr_camDist = Mathf.Clamp(curr_camDist, minDist, init_dist);
        mainCamera.localPosition = -Vector3.forward * curr_camDist;

    }//LateUpdate
     //_______________________________________________________________________
    #endregion


    #region MY FUNCTIONS
    //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }

    }//RayHitComparer
     //_______________________________________________________________________
    #endregion



}//END


//in doubts how to use this script? get in touch with me at https://www.instagram.com/florinpain_official/ and I can help you
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_Switch : MonoBehaviour
{
    //Public Variables\\
    [Header("Spawn Point for the Prefab")]
    public Transform Spawnpoint;
    [Space]
    [Header("Prefab to be spawned")]
    public GameObject Prefab;
    [Space]
    [Header("Original")]
    public GameObject Original;




    // Update is called once per frame
    void Start()
    {
        CopyTransformsRecurse(Spawnpoint, Prefab);
        Instantiate(Prefab, Spawnpoint.position, Spawnpoint.rotation);
        Destroy(Original);


    }//Start


    public void CopyTransformsRecurse(Transform src, GameObject dst)
    {
        dst.transform.position = src.position;
        dst.transform.rotation = src.rotation;

        //if (dst.GetComponent<Rigidbody>())
        //dst.GetComponent<Rigidbody>().AddForce(hitDirection * force, ForceMode.VelocityChange);

        foreach (Transform child in dst.transform)
        {
            Transform curSrc = src.Find(child.name);
            if (curSrc)
            {
                CopyTransformsRecurse(curSrc, child.gameObject);

            }
        }

    }//CopyTransformsRecurse


    //void CopyVelocity(Rigidbody from, Rigidbody to)
    //{
    //    Vector3 vFrom = from.velocity;
    //    Vector3 vTo = to.velocity;

    //    // Move the values you want for each exis
    //    vTo.x = vFrom.x;
    //    // vTo.y = vFrom.y; // Leaving y-axis as is
    //    vTo.z = vFrom.z;

    //    to.velocity = vTo;
    //}
}

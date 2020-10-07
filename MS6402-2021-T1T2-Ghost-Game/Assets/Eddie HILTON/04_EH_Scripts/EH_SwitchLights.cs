using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EH_SwitchLights : MonoBehaviour
{
    public GameObject lightFixture; // The game object that is getting switched off.
    public GameObject playerLocator;
    public GameObject switchLightsPopup; // The text object on the canvas that is tied to this event.

    public float activeRange = 3; //range at which behaviour becomes active.

    public GameObject lightExplode; // 

    public bool active = false;

    private bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject taggedObject = GameObject.FindWithTag("player");
        
        if (taggedObject != null)
        {
            playerLocator = taggedObject;
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (active == true)
        {
            if (Input.GetKeyDown(KeyCode.E) == true)
            {
                Instantiate(lightExplode, lightFixture.transform.position, Quaternion.Euler(new Vector3(-270, 0, 0)));
                Destroy(lightFixture);
                completed = true;
                switchLightsPopup.SetActive(false);
            }
        }

        if (Vector3.Distance(playerLocator.transform.position, this.transform.position) <= activeRange && !completed)
        {
            active = true;
            switchLightsPopup.SetActive(true);
        }
        else
        {
            active = false;
            switchLightsPopup.SetActive(false);
        }
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PowerupDisplay : MonoBehaviour
{

    List<GameObject> activePowerupList = new List<GameObject>();
    public GameObject heal, juggernaut, teleport, freez, barrier;
    bool isHealActive, isJuggernautActive, isTeleportActive, isFreezActive, isBarrierActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check for each powerup (with a boolean variable)
            //if true and active => show image in active powerups
            // if true and deactive => show image in deactive powerup
            //if false => hide image
       
        //How to show on canvas?
            //Have a list 
            //add the active powerup into the list.
                //Find its position in the list=> if the first one, it appears in the first position in the UI and ..
            //Remove deactive powerups from the list 
        if (isHealActive)
        {
            activePowerupList.Add(heal);
            int index = activePowerupList.IndexOf(heal);
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class SetMode : MonoBehaviour
{
    DataBase link_input;



    void Start()
    {
        print("156156");
        link_input = GameObject.Find("Manerger").GetComponent<DataBase>();
        Lbs_mode("https://smartcuratorapi.azurewebsites.net/Api/Factory/GetLbsAr/31806736-C18E-4764-BF6E-B976E1235F38?longitude=120.949374&latitude=23.868287&distance=600");
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ARTag_mode()
    {
        // GameObject.Find("Camera").GetComponent<GyroControl>().enabled = false;
        // GameObject.Find("World").SetActive(false);
        // GameObject.Find("Canvas").SetActive(true);
        // GameObject.Find("ManagerAR").SetActive(true);
        // GameObject.Find("B bear_butterfly").SetActive(true);
        // GameObject.Find("P bear_jump").SetActive(true);
        // GameObject.Find("wanwan跳舞").SetActive(true);
        // GameObject.Find("owl2").SetActive(true);
    }

    public void Lbs_mode(string link_in)
    {
        
        // GameObject.Find("Canvas").SetActive(false);
        // GameObject.Find("ManagerAR").SetActive(false);
        // GameObject.Find("B bear_butterfly").SetActive(false);
        // GameObject.Find("P bear_jump").SetActive(false);
        // GameObject.Find("wanwan跳舞").SetActive(false);
        // GameObject.Find("owl2").SetActive(false);
        // GameObject.Find("World").SetActive(true);
        // GameObject.Find("Camera").GetComponent<GyroControl>().enabled = true;
        link_input.linkname(link_in);
    }

}

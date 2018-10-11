using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GPS : MonoBehaviour
{



    //https://maps.googleapis.com/maps/api/geocode/json?latlng=
    public static GPS Instance { set; get; }
    public static bool finish = false;

    public float latitude;
    public float longitude;
    private bool change=true;
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void Update()
    {
        if (change == true)
        {
            StartCoroutine(StarLocationService());
        }
        
    }

    private IEnumerator StarLocationService()
    {
        change = false;
        if (!Input.location.isEnabledByUser)
        {
            print("Error");
            yield break;
        }
        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait <= 0)
        {
            print("time out");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to ");
            yield break;
        }
        finish = true;
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        change = true;
        yield break;
    }


}

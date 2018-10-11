using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class DataBase : MonoBehaviour {
    public jsonState loadData = new jsonState();
    public string link="";
    List<String> position = new List<String>();//name
    List<String> distance = new List<String>();//poid
    List<String> longitude = new List<String>();
    List<String> latitude = new List<String>();
    public bool set_data_ok = false;
    public class jsonState
    {
        public detail[] factory;
        public detail[] poi;
    }
    [Serializable]
    public class detail
    {
        public string id;
        public string name;
        public string description;
        public double longitude;
        public double latitude;
        public string poiType;
        public double angle;
        public double distance;
    }

    // Use this for initialization
  
	// Update is called once per frame
	void Update () {
     
    }
    IEnumerator Create_obj()
    {
       // "https://smartcuratorapi.azurewebsites.net/Api/Factory/GetLbsAr/31806736-C18E-4764-BF6E-B976E1235F38?distance=500&position=20"
        WWW linkname = new WWW(link);
        yield return linkname;

        if (!string.IsNullOrEmpty(linkname.error))
        {
            print("Error downloading: " + linkname.error);
        }
        else
        {
            loadData = JsonUtility.FromJson<jsonState>(linkname.text);
        }
        set_data_ok = true;
    }

    public void linkname(string link_in)
    {

        link = link_in;
        position_distance(link_in);
        
    }
    public static List<String> matches(String text, String pattern, int groupId)
    {
        List<String> rzList = new List<String>();


        Match match = Regex.Match(text, pattern);
        for (int i = 0; match.Success; i++)
        {
            rzList.Add(match.Groups[groupId].Value);
            match = match.NextMatch();
        }
        return rzList;
    }
    void position_distance(String link_in)
    {
        // "https://smartcuratorapi.azurewebsites.net/Api/Factory/GetLbsAr/31806736-C18E-4764-BF6E-B976E1235F38?distance=500&position=560"
        
        position = matches(@link_in, @"position=[0-9]{1,8}", 0);
        distance = matches(@link_in, @"distance=[0-9]{1,8}.[0-9]{1,8}", 0);
        longitude = matches(@link_in, @"longitude=[0-9]{1,8}.[0-9]{1,8}", 0);
        latitude = matches(@link_in, @"latitude=[0-9]{1,8}.[0-9]{1,8}", 0);
        if (position.Count != 0)
        {
            gameObject.GetComponent<Main_control>().size = Convert.ToInt16(position[0].Replace("position=", ""));
        }

        if (distance.Count != 0)
        {
            gameObject.GetComponent<Main_control>().far = Convert.ToSingle(distance[0].Replace("distance=", ""));
        }

        if (longitude.Count !=0 && latitude.Count !=0)
        {
            gameObject.GetComponent<Main_control>().FakeGPS = true;
            gameObject.GetComponent<Main_control>().longitude = Convert.ToSingle(longitude[0].Replace("longitude=", ""));
            gameObject.GetComponent<Main_control>().latitude = Convert.ToSingle(latitude[0].Replace("latitude=", ""));
        }
        StartCoroutine(Create_obj());
    }
}

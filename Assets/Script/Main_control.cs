using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_control : MonoBehaviour {
    //3d物件的button(點擊)
    public GameObject cude;

    [Header("劇場")]
    public GameObject copy1GameObject;//劇場
    [Header("服務")]
    public GameObject copy2GameObject;//服務
    [Header("樂園")]
    public GameObject copy3GameObject;//樂園
    [Header("部落")]
    public GameObject copy4GameObject;//部落
    [Header("餐廳")]
    public GameObject copy5GameObject;//餐廳
    //取用DataBase裡的data
    DataBase json_data;

    //要被放置在哪個物件底下
    public GameObject superGameObject;
    public GameObject copyGameObject;
    //被複製出來的物件
    public GameObject[] childGameObject_poi;
    public GameObject[] childGameObject_factory;

    //測試用位置24.172344,120.6608473
    public double longitude;
    public double latitude ;
    public bool FakeGPS=false;

    //1公尺約0.00000900900901度
    //範圍倍率  tranform_distance*n=範圍
    float tranform_distance = 0.0009f;
    public float far = 500;
    int near = 10;
    //物件上限數量
    public int size = 20;

    //物件總量
    int poi_Object_size=0;
    int factory_Object_size = 0;
    //物件放大縮小
    int Object_rect = 250;
    // Use this for initialization

    // android init
    public AndroidJavaObject androidContext()
    {
        return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
    }


    void Start () {
        json_data = gameObject.GetComponent<DataBase>();
      
    }
	
    void check_factory_size()
    {
        
        /*執行所有物件，並計算是否在範圍內，如果在範圍內則動態產生物件*/
        for (int i = 0; i < json_data.loadData.factory.Length; i++)
        {
            if (json_data.loadData.factory[i].longitude != 0 && json_data.loadData.factory[i].latitude != 0 && size > 0)
            {

                if ((longitude + far * tranform_distance) >= Convert.ToDouble(json_data.loadData.factory[i].longitude) && (longitude - far * tranform_distance) <= Convert.ToDouble(json_data.loadData.factory[i].longitude))
                {
                    if ((latitude + far * tranform_distance) >= Convert.ToDouble(json_data.loadData.factory[i].latitude) && (latitude - far * tranform_distance) <= Convert.ToDouble(json_data.loadData.factory[i].latitude))
                    {
                        if ((childGameObject_factory[i] == null|| childGameObject_factory[i].name != json_data.loadData.factory[i].name) && json_data.loadData.factory[i].distance*1000 > near && json_data.loadData.factory[i].distance*1000 <far)
                        {
                            size--;
                            childGameObject_factory[i] = Instantiate(copyGameObject);
                            //複製copyGameObject物件(連同該物件身上的腳本一起複製)
                            childGameObject_factory[i].transform.parent = superGameObject.transform;//放到superGameObject物件內
                            childGameObject_factory[i].name = i.ToString();
                            cude = GameObject.Find("Cube");
                            cude.name = "factory";
                        }
                    }
                }
            }
        }

        //物件位置，依比例越後面則放大高度的倍率越大  2:0.05:x:distance 
        for (int i = 0; i < json_data.loadData.factory.Length; i++)
        {
            double scale;
            scale = 2 * json_data.loadData.factory[i].distance / 0.05;
            if (childGameObject_factory[i] == null)
            {

            }
            else
            {
                childGameObject_factory[i].GetComponent<Transform>().localPosition = new Vector3(250 * (float)json_data.loadData.factory[i].distance * Mathf.Sin((float)json_data.loadData.factory[i].angle * Mathf.PI / 180.0f), 0f, 250 * (float)json_data.loadData.factory[i].distance * Mathf.Cos((float)json_data.loadData.factory[i].angle * Mathf.PI / 180.0f));
                //重疊之問題
                int same_degree = (int)(childGameObject_factory[i].GetComponent<Transform>().localPosition.y / scale);
                for (int j = 0; j < i; j++)
                {
                    if (json_data.loadData.factory[i].angle + 20 > json_data.loadData.factory[i].angle && json_data.loadData.factory[i].angle - 20 < json_data.loadData.factory[i].angle && childGameObject_factory[j] != null)
                    {
                        if ((childGameObject_factory[j].GetComponent<Transform>().localPosition.x + 15 > childGameObject_factory[i].GetComponent<Transform>().localPosition.x && childGameObject_factory[j].GetComponent<Transform>().localPosition.x - 15 < childGameObject_factory[i].GetComponent<Transform>().localPosition.x) || (childGameObject_factory[j].GetComponent<Transform>().localPosition.y + 15 > childGameObject_factory[i].GetComponent<Transform>().localPosition.y && childGameObject_factory[j].GetComponent<Transform>().localPosition.y - 15 < childGameObject_factory[i].GetComponent<Transform>().localPosition.y))
                        {
                            same_degree++;
                        }

                    }

                }
                //過近時，隱藏物件
                if (json_data.loadData.factory[i].distance*1000 < near || json_data.loadData.factory[i].distance*1000 > far)
                {
                    childGameObject_factory[i].gameObject.SetActive(false);
                    childGameObject_factory[i].GetComponent<Transform>().localPosition = new Vector3(1000000, 1000000, 1000000);

                }
                else
                {
                    childGameObject_factory[i].GetComponent<Transform>().rotation = Quaternion.Euler(0 + 90, (float)json_data.loadData.factory[i].angle + 180, 0);
                    childGameObject_factory[i].GetComponent<Transform>().localPosition = new Vector3(Object_rect * (float)json_data.loadData.factory[i].distance * Mathf.Sin((float)json_data.loadData.factory[i].angle * Mathf.PI / 180.0f), (float)scale * (same_degree - 1), Object_rect * (float)json_data.loadData.factory[i].distance * Mathf.Cos((float)json_data.loadData.factory[i].angle * Mathf.PI / 180.0f));
                    childGameObject_factory[i].GetComponentInChildren<TextMeshPro>().text = json_data.loadData.factory[i].name;
                    childGameObject_factory[i].GetComponentInChildren<TextMeshPro>().name = json_data.loadData.factory[i].name;
                    //childGameObject_factory[i].GetComponentInChildren<TextMesh>().text = json_data.loadData.factory[i].name;
                    //childGameObject_factory[i].GetComponentInChildren<TextMesh>().name = json_data.loadData.factory[i].name;
                    
                }
            }
        }
    }

    void check_poi_size()
    {
        /*執行所有物件，並計算是否在範圍內，如果在範圍內則動態產生物件*/
        for (int i = 0; i < json_data.loadData.poi.Length; i++)
        {
            if (json_data.loadData.poi[i].longitude != 0 && json_data.loadData.poi[i].latitude != 0 && size > 0)
            {
                if ((longitude + far * tranform_distance) >= Convert.ToDouble(json_data.loadData.poi[i].longitude) && (longitude - far * tranform_distance) <= Convert.ToDouble(json_data.loadData.poi[i].longitude))
                {
                    if ((latitude + far * tranform_distance) >= Convert.ToDouble(json_data.loadData.poi[i].latitude) && (latitude - far * tranform_distance) <= Convert.ToDouble(json_data.loadData.poi[i].latitude))
                    {
                        if ((childGameObject_poi[i] == null || childGameObject_poi[i].name != json_data.loadData.poi[i].name) && json_data.loadData.poi[i].distance*1000 >  near && json_data.loadData.poi[i].distance*1000 < far)
                        {
                            if(childGameObject_poi[i] != null)
                            {
                                Destroy(childGameObject_poi[i]);
                            }
                            size--;
                            if (json_data.loadData.poi[i].poiType == "部落")
                            {
                                childGameObject_poi[i] = Instantiate(copy1GameObject);
                            }
                            else if (json_data.loadData.poi[i].poiType == "服務")
                            {
                                childGameObject_poi[i] = Instantiate(copy2GameObject);
                            }
                            else if (json_data.loadData.poi[i].poiType == "樂園")
                            {
                                childGameObject_poi[i] = Instantiate(copy3GameObject);
                            }
                            else if (json_data.loadData.poi[i].poiType == "劇場")
                            {
                                childGameObject_poi[i] = Instantiate(copy4GameObject);
                            }
                            else if (json_data.loadData.poi[i].poiType == "餐飲")
                            {
                                childGameObject_poi[i] = Instantiate(copy5GameObject);
                            }
                         //   childGameObject_poi[i] = Instantiate(copyGameObject);
                            //複製copyGameObject物件(連同該物件身上的腳本一起複製)
                            childGameObject_poi[i].transform.parent = superGameObject.transform;//放到superGameObject物件內
                            childGameObject_poi[i].name = i.ToString();
                            cude = GameObject.Find("Cube");
                            cude.name = "poi";

                        }
            }
        }
    }
        }

        //物件位置，依比例越後面則放大高度的倍率越大  2:0.05:x:distance 
        for (int i = 0; i < json_data.loadData.poi.Length; i++)
        {

            double scale;
            scale = 2 * json_data.loadData.poi[i].distance / 0.05;
            if (childGameObject_poi[i] == null)
            {

            }
            else
            {
                childGameObject_poi[i].GetComponent<Transform>().localPosition = new Vector3(250 * (float)json_data.loadData.poi[i].distance * Mathf.Sin((float)json_data.loadData.poi[i].angle * Mathf.PI / 180.0f), 0f, 250 * (float)json_data.loadData.poi[i].distance * Mathf.Cos((float)json_data.loadData.poi[i].angle * Mathf.PI / 180.0f));
                //重疊之問題
                //   int same_degree = (int)(childGameObject_poi[i].GetComponent<Transform>().localPosition.y / scale);
                int same_degree = 0;
                for (int j = 0; j < i; j++)
                {
                    if (json_data.loadData.poi[i].angle + 20 > json_data.loadData.poi[i].angle && json_data.loadData.poi[i].angle - 20 < json_data.loadData.poi[i].angle && childGameObject_poi[j] != null)
                    {
                        if ((childGameObject_poi[j].GetComponent<Transform>().localPosition.x + 15 > childGameObject_poi[i].GetComponent<Transform>().localPosition.x && childGameObject_poi[j].GetComponent<Transform>().localPosition.x - 15 < childGameObject_poi[i].GetComponent<Transform>().localPosition.x) || (childGameObject_poi[j].GetComponent<Transform>().localPosition.y + 15 > childGameObject_poi[i].GetComponent<Transform>().localPosition.y && childGameObject_poi[j].GetComponent<Transform>().localPosition.y - 15 < childGameObject_poi[i].GetComponent<Transform>().localPosition.y))
                        {
                            same_degree++;
                        }

                    }

                }
                //過近時，隱藏物件
               
                if (json_data.loadData.poi[i].distance*1000 <near|| json_data.loadData.poi[i].distance*1000 > far)
                {
                    childGameObject_poi[i].gameObject.SetActive(false);
                    childGameObject_poi[i].GetComponent<Transform>().localPosition = new Vector3(1000000, 1000000, 1000000);
                }
                else
                {
                    childGameObject_poi[i].GetComponent<Transform>().rotation = Quaternion.Euler(0 + 90, (float)json_data.loadData.poi[i].angle + 180, 0);
                    childGameObject_poi[i].GetComponent<Transform>().localPosition = new Vector3(Object_rect * (float)json_data.loadData.poi[i].distance * Mathf.Sin((float)json_data.loadData.poi[i].angle * Mathf.PI / 180.0f), (float)scale * (same_degree - 1), Object_rect * (float)json_data.loadData.poi[i].distance * Mathf.Cos((float)json_data.loadData.poi[i].angle * Mathf.PI / 180.0f));
                    childGameObject_poi[i].GetComponentInChildren<TextMeshPro>().text = json_data.loadData.poi[i].name;
                    childGameObject_poi[i].GetComponentInChildren<TextMeshPro>().name = json_data.loadData.poi[i].name+" "+ json_data.loadData.poi[i].distance;
                    //childGameObject_poi[i].GetComponentInChildren<TextMesh>().text = json_data.loadData.poi[i].name;
                    //childGameObject_poi[i].GetComponentInChildren<TextMesh>().name = json_data.loadData.poi[i].name;
                }
            }
        }
    }
    //void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 28;
    //    GUI.Label(new Rect(20, 20, 600, 48), longitude.ToString());
    //    GUI.Label(new Rect(20, 50, 600, 48), latitude.ToString());
    //}
    void data()
    {
        for (int i = 0; i < json_data.loadData.poi.Length; i++)
        {
            print(json_data.loadData.poi[i].name);
            print(json_data.loadData.poi[i].distance);
            print(json_data.loadData.poi[i].angle);
        }
    }
    // Update is called once per frame
    void Update () {
        if (FakeGPS == true)
        {
            
        }else
        {
            longitude = GPS.Instance.longitude;
            latitude = GPS.Instance.latitude;
        }

        
        if (json_data.set_data_ok == true)
        {
            for (int i = 0; i < json_data.loadData.poi.Length; i++)
            {
                Calculate_degree_distance(longitude, latitude, i, 1);
                //  print(i + " " + json_data.loadData.poi[i].distance);
            }
            for (int i = 0; i < json_data.loadData.factory.Length; i++)
            {
                Calculate_degree_distance(longitude, latitude, i, 0);
                //  print(i + " " + json_data.loadData.poi[i].distance);
            }
            if (json_data.loadData.poi.Length > 0 && poi_Object_size != json_data.loadData.poi.Length)
            {
                childGameObject_poi = new GameObject[json_data.loadData.poi.Length];
                poi_Object_size = json_data.loadData.poi.Length;
            }
            if (json_data.loadData.factory.Length > 0 && factory_Object_size != json_data.loadData.factory.Length)
            {
                childGameObject_factory = new GameObject[json_data.loadData.factory.Length];
                factory_Object_size = json_data.loadData.factory.Length;
            }
            Array.Sort(json_data.loadData.poi, delegate (DataBase.detail x, DataBase.detail y) { return x.distance.CompareTo(y.distance); });

            check_poi_size();
            check_factory_size();
        }
       
    }

    private void Calculate_degree_distance(double longitude, double latitude, int i, int mode)
    {
        double degree;//角度
        double distance_object;
        double lon2;
        double lat2;
        if (mode == 0)
        {
            lon2 = Convert.ToDouble(json_data.loadData.factory[i].longitude);
            lat2 = Convert.ToDouble(json_data.loadData.factory[i].latitude);
        }
        else
        {
            lon2 = Convert.ToDouble(json_data.loadData.poi[i].longitude);
            lat2 = Convert.ToDouble(json_data.loadData.poi[i].latitude);
        }

        degree = gpsd((float)longitude, (float)latitude, (float)lon2, (float)lat2);

        double a = longitude * Mathf.PI / 180.0 - lon2 * Mathf.PI / 180.0;
        double radLat1 = latitude * Mathf.PI / 180.0;
        double radLat2 = lat2 * Mathf.PI / 180.0;
        double b = radLat1 - radLat2;

        double s = 2 * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin((float)(b / 2)), 2) +
        Mathf.Cos((float)radLat1) * Mathf.Cos((float)radLat2) * Mathf.Pow(Mathf.Sin((float)(a / 2)), 2)));
        s = s * 6378.137; // 地球半径，单位千米
        distance_object = Mathf.Round((float)(s * 10000)) / 10000;//公里

        if (mode == 0)
        {
            json_data.loadData.factory[i].angle = degree;
            json_data.loadData.factory[i].distance = distance_object;
        }
        else
        {
            json_data.loadData.poi[i].angle = degree;
            json_data.loadData.poi[i].distance = distance_object;
        }

    }
    /*計算角度*/
    private float gpsd(float lng_a, float lat_a, float lng_b, float lat_b)
    {
        float m;
        float x, y;
        x = lng_b - lng_a;
        y = lat_b - lat_a;
        m = (lng_b - lng_a) / (lat_b - lat_a);
        m = Mathf.Atan(m) * 180 / Mathf.PI;
        if (x > 0 && y < 0)
        {
            m = 180 - Mathf.Abs(m);
        }
        else if (x <= 0 && y <= 0)
        {
            m = 180 + Mathf.Abs(m);
        }
        else if (m < 0)
        {
            m = 360 + m;
        }
        return m;
    }


}

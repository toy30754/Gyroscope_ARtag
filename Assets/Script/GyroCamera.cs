using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroCamera : MonoBehaviour
{
    public Gyroscope gyro;
    private bool gyroSupported;
    private Quaternion rotfix;
    public GameObject world;


    // private Transform[] worldObj;
    [SerializeField]
    private GameObject[] worldObj;

    private float startY;
    int i = 0;
    // Use this for initialization
    void Start()
    {
        worldObj = new GameObject[70];
     

        gyroSupported = SystemInfo.supportsGyroscope;

        GameObject canParent = new GameObject("canParent");
        canParent.transform.position = transform.position;
      //  transform.parent = canParent.transform;
        transform.SetParent(canParent.transform);


        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            canParent.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
            rotfix = new Quaternion(0, 0, 1, 0);
        }
    }


    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < world.transform.childCount; i++)
        {
            worldObj[i] = world.transform.GetChild(i).gameObject;
        }

        if (gyroSupported && startY == 0)
        {
            ResetGyroROtation();
        }
        transform.localRotation = gyro.attitude * rotfix;


    }

    void ResetGyroROtation()
    {
        startY = transform.eulerAngles.y;
        worldObj[i].transform.rotation = Quaternion.Euler(0f, startY, 0f);
    }
}

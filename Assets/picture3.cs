using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picture3 : MonoBehaviour {

    public GameObject planeGameObject;
      public Texture2D texture;
    // Use this for initialization
    void Start()
    {
        
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        planeGameObject.GetComponent<Renderer>().material = material;
    }
    // Update is called once per frame
    void Update () {
		
	}
}

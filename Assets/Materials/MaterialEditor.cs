using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEditor : MonoBehaviour {
    public Texture[] textures;
    public int currentTexture;
    Material newMat;
    Material oldMat;
    public float newTimeFlat = 0;
    public float oldTimeFlat = 1;
    void Start()
    {
        newMat = Resources.Load("BlueMask", typeof(Material)) as Material;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {

            //gameObject.renderer.material = newMat;
            oldMat = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material = newMat;
            newMat = oldMat;

            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            //currentTexture++;
            //currentTexture %= textures.Length;
            GetComponent<Renderer>().material.mainTexture = textures[currentTexture];
        }
    }
}

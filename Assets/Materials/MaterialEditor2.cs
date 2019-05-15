using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEditor2 : MonoBehaviour {
    public Texture[] textures;
    public int currentTexture;
    Material newMat;
    Material oldMat;
    public float newTimeFlat = 0;
    public float oldTimeFlat = 1;
    void Start()
    {
        newMat = Resources.Load("RedMask", typeof(Material)) as Material;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {

            //gameObject.renderer.material = newMat;
            oldMat = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material = newMat;
            newMat = oldMat;

            //currentTexture++;
            //currentTexture %= textures.Length;
            GetComponent<Renderer>().material.mainTexture = textures[currentTexture];
        }
    }
}

using UnityEngine;
using System.Collections;

public class Resize : MonoBehaviour
{
    // Use this for initialization
  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.02F, 0.01F, 0);
    }
}

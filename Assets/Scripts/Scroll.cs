using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    private float _speed = 0.007f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * _speed, 0);

        MeshRenderer mr = GetComponent<MeshRenderer>();

        mr.material.mainTextureOffset = offset;
    }
}

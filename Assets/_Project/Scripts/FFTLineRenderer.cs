using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLineRenderer : MonoBehaviour
{
    LineRenderer _Line;

    // Start is called before the first frame update
    void Start()
    {
        _Line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

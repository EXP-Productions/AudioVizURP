using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ApplyFFTTexture : MonoBehaviour
{
    public FFTToTexture _FFTTexture;
    MeshRenderer _MeshRenderer;

    public string _TextureName = "_BaseMap";

    // Start is called before the first frame update
    void Start()
    {
        print("Applying texture");
        _MeshRenderer = GetComponent<MeshRenderer>();
        _MeshRenderer.material.SetTexture(_TextureName, _FFTTexture._FFTTexture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

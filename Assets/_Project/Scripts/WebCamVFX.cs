using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
/// <summary>
/// Basic example of feeding webcam data into a vfx graph and manipulating with mouse input
/// </summary>
public class WebCamVFX : MonoBehaviour
{
    public VisualEffect _VFX;
    public Camera _Camera;
    WebCamTexture _WebCamTexture;
    Texture2D _Output;

    void Start()
    {
        _WebCamTexture = new WebCamTexture(WebCamTexture.devices[1].name);
        _WebCamTexture.Play();
        _Output = new Texture2D(_WebCamTexture.width, _WebCamTexture.height, TextureFormat.ARGB32, false);
    }

    void Update()
    {
        // Copy webcam texture into VFX graph
        Graphics.CopyTexture(_WebCamTexture, _Output);
        _VFX.SetTexture("WebCamTex", _Output);

        // Add attraction / repulsion to mouse on click
        Vector3 target = _Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f));
        _VFX.SetVector3("_TargetPos", target);

        float attractionScalar;

        if (Input.GetMouseButton(0))
        {
            attractionScalar = 3f;
        }
        else if (Input.GetMouseButton(1))
        {
            attractionScalar = -3f;
        }
        else
        {
            attractionScalar = 0f;
        }

        _VFX.SetFloat("_AttractionScalar", attractionScalar);
    }

}

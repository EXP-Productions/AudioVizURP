using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTScale : MonoBehaviour
{   
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    public int _FrequencyBandIndex = 0;

    Vector3 _BaseScale;
    public Vector3 _ScaleStrength;

    private void Awake()
    {
        _BaseScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = _BaseScale + (_ScaleStrength * _FFT.GetBandValue(_FrequencyBandIndex, _FreqBands));        
    }
}

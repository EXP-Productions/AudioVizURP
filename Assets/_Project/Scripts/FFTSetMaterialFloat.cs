using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTSetMaterialFloat : MonoBehaviour
{   
    public FFT _FFT;
    public FFT.Bands _FreqBands = FFT.Bands.Eight;
    public int _FrequencyBandIndex = 0;

    public string _FloatName = "_EmissionStrength";
    public float _StrengthScalar = 1;

    public MeshRenderer _MeshRenderer;

    void Update()
    {
        if (_FFT != null)
        {
            float strength = _FFT.GetBandValue(_FrequencyBandIndex, _FreqBands) * _StrengthScalar;
            _MeshRenderer.material.SetFloat(_FloatName, strength);
        }
    }
}

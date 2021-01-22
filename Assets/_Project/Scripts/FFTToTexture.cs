using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTToTexture : MonoBehaviour
{
    public FrequencyBandAnalyser.Bands _Bands = FrequencyBandAnalyser.Bands.Eight;
    public FrequencyBandAnalyser _FFT;
    public Texture2D _FFTTexture;
    public FilterMode _FilterMode = FilterMode.Point;
    public Color _Col = Color.white;
    public float _Strength = 10;
    Color[] _Colours;

    void Awake()
    {
        _Colours = new Color[(int)_Bands];      

        _FFTTexture = new Texture2D(_Colours.Length, 1);
        _FFTTexture.filterMode = _FilterMode;
        _FFTTexture.wrapMode = TextureWrapMode.Clamp;
    }


    void Update()
    {
        for (int i = 0; i < _Colours.Length; i++)
        {
            if (_Bands == FrequencyBandAnalyser.Bands.Eight)
                _Colours[i] = _FFT._FreqBands8[i] * _Col * _Strength;
            else
                _Colours[i] = _FFT._FreqBands64[i] * _Col * _Strength;
        }

        _FFTTexture.SetPixels(_Colours);
        _FFTTexture.Apply();
    }
}

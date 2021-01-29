using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTToTexture : MonoBehaviour
{   
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _Bands = FrequencyBandAnalyser.Bands.Eight;
    public float _Strength = 10;

    public Texture2D _FFTTexture;
    Color[] _Colours;
    public Color _Col = Color.white;

    public Material _Material;
    public string _TextureName = "_BaseMap";

    void Awake()
    {
        _Colours = new Color[(int)_Bands];      

        _FFTTexture = new Texture2D(_Colours.Length, 1);
        _FFTTexture.filterMode = FilterMode.Point;
        _FFTTexture.wrapMode = TextureWrapMode.Clamp;

        _Material.SetTexture(_TextureName, _FFTTexture);
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

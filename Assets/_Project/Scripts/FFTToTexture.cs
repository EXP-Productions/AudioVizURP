using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTToTexture : MonoBehaviour
{
    public enum Bands
    {
        Eight = 8,
        SixtyFour = 64,
    }

    public Bands _Bands = Bands.Eight;
    public FFT _FFT;
    public Texture2D _FFTTexture;
    public FilterMode _FilterMode = FilterMode.Point;
    public Color _Col = Color.white;
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
            if (_Bands == Bands.Eight)
                _Colours[i] = _FFT._FreqBands8[i] * _Col;
            else
                _Colours[i] = _FFT._FreqBands64[i] * _Col;
        }

        _FFTTexture.SetPixels(_Colours);
        _FFTTexture.Apply();
    }
}

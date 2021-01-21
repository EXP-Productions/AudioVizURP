using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTToTexture : MonoBehaviour
{
    public FFT _FFT;
    public Texture2D _FFTTexture;

    public FilterMode _FilterMode = FilterMode.Point;

    public Color _Col = Color.white;

    Color[] _Colours;

    // Start is called before the first frame update
    void Awake()
    {
        _FFTTexture = new Texture2D(_FFT._SampleCount, 1);
        _FFTTexture.filterMode = _FilterMode;

        _Colours = new Color[_FFT._SampleCount];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _FFTTexture.width; i++)
        {
            float fftSample = _FFT._SamplesNormalized[i];

            _Colours[i] = _Col * fftSample;
        }

        _FFTTexture.SetPixels(_Colours);
        _FFTTexture.Apply();
    }
}

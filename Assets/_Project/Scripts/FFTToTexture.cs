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
        _Colours = new Color[_FFT._NumBands];

        _FFTTexture = new Texture2D(_Colours.Length, 1);
        _FFTTexture.filterMode = _FilterMode;
        _FFTTexture.wrapMode = TextureWrapMode.Clamp;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _FFTTexture.width; i++)
        {
            float fftSample = _FFT._FreqBands[i]; //_FFT.GetValueAtScaledIndex(i);// _FFT.LinearIndexToLogSpacedValue(i); //_FFT.GetValueAtScaledIndex(i);// _FFT._SamplesNormalized[i];

            _Colours[i] = _Col * fftSample;
        }

        _FFTTexture.SetPixels(_Colours);
        _FFTTexture.Apply();
    }
}

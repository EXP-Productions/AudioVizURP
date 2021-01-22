using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqBandData : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    public int _FrequencyBandIndex = 0;

    public float GetFFTBandValue()
    {
        return _FFT.GetBandValue(_FrequencyBandIndex, _FreqBands);
    }

    public float GetFFTBandValue(int index)
    {
        return _FFT.GetBandValue(index, _FreqBands);
    }
}

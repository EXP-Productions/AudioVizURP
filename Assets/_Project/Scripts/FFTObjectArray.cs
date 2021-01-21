using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectArray : MonoBehaviour
{
    public FFT _FFT;
    public FFT.Bands _FreqBands = FFT.Bands.Eight;
    FFTScale[] _FFTObjects;

    public FFTScale _ObjectToSpawn;

    public float _Spacing = 1;

    public Vector3 _ScaleStrength = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        _FFTObjects = new FFTScale[(int)_FreqBands];
        for (int i = 0; i < (int)_FreqBands; i++)
        {
            FFTScale newFFTObject = Instantiate(_ObjectToSpawn).GetComponent<FFTScale>();
            newFFTObject.transform.SetParent(transform);
            newFFTObject.transform.localPosition = new Vector3(_Spacing * i, 0, 0);

            newFFTObject._FFT = _FFT;
            newFFTObject._FreqBands = _FreqBands;
            newFFTObject._FrequencyBandIndex = i;
            newFFTObject._ScaleStrength = _ScaleStrength;

            _FFTObjects[i] = newFFTObject;
        }
    }
}

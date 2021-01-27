using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectArray : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    FFTScale[] _FFTObjects;

    public FFTScale _ObjectToSpawn;

    public float _Spacing = 1;

    public Vector3 _ScaleStrength = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        _FFTObjects = new FFTScale[(int)_FreqBands];

        ////---   LINEAR
        //for (int i = 0; i < (int)_FreqBands; i++)
        //{
        //    FFTScale newFFTObject = Instantiate(_ObjectToSpawn).GetComponent<FFTScale>();
        //    newFFTObject.transform.SetParent(transform);
        //    newFFTObject.transform.localPosition = new Vector3(_Spacing * i, 0, 0);

        //    newFFTObject._FFT = _FFT;
        //    newFFTObject._FreqBands = _FreqBands;
        //    newFFTObject._FrequencyBandIndex = i;
        //    newFFTObject._ScaleStrength = _ScaleStrength;

        //    _FFTObjects[i] = newFFTObject;
        //}


        float angleSpacing = (2f * Mathf.PI) / _FFTObjects.Length;
        //---   CIRCULAR
        for (int i = 0; i < _FFTObjects.Length; i++)
        {
            float angle = i * angleSpacing;
            float x = Mathf.Sin(angle) * _Spacing;
            float y = Mathf.Cos(angle) * _Spacing;


            FFTScale newFFTObject = Instantiate(_ObjectToSpawn).GetComponent<FFTScale>();
            newFFTObject.transform.SetParent(transform);
            newFFTObject.transform.localPosition = new Vector3(x, y, 0);

            FFTSetMaterialFloat setMaterialFloat = newFFTObject.gameObject.GetComponentInChildren<FFTSetMaterialFloat>();
            if(setMaterialFloat != null)
            {
                setMaterialFloat._FFT = _FFT;
                setMaterialFloat._FreqBands = _FreqBands;
                setMaterialFloat._FrequencyBandIndex = i;
            }

            //---   ROTATION
            newFFTObject.transform.LookAt(transform.position);
            newFFTObject.transform.localRotation *= Quaternion.Euler(-90,0,0);

            newFFTObject._FFT = _FFT;
            newFFTObject._FreqBands = _FreqBands;
            newFFTObject._FrequencyBandIndex = i;
            newFFTObject._ScaleStrength = _ScaleStrength;

            _FFTObjects[i] = newFFTObject;
        }
    }
}

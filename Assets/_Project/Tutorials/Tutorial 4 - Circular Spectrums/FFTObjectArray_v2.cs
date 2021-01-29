using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectArray_v2 : MonoBehaviour
{
    public enum Shape
    {
        Line,
        Circle,
    }

    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    public GameObject[] _FFTGameObjects;

    public GameObject _ObjectToSpawn;

    public Shape _Shape = Shape.Line;

    public float _SpacingRadius = 1;

    public Vector3 _ScaleStrength = Vector3.up;
    Vector3 _BaseScale;

    // Start is called before the first frame update
    void Start()
    {
        _FFTGameObjects = new GameObject[(int)_FreqBands];
        _BaseScale = _ObjectToSpawn.transform.localScale;

        if (_Shape == Shape.Line)
        {
            //---   LINEAR
            for (int i = 0; i < _FFTGameObjects.Length; i++)
            {
                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(_SpacingRadius * i, 0, 0);
                _FFTGameObjects[i] = newFFTObject;
            }
        }
        else if(_Shape == Shape.Circle)
        {
            float angleSpacing = (2f * Mathf.PI) / (int)_FreqBands;
            //---   CIRCULAR
            for (int i = 0; i < _FFTGameObjects.Length; i++)
            {
                float angle = i * angleSpacing;
                float x = Mathf.Sin(angle) * _SpacingRadius;
                float y = Mathf.Cos(angle) * _SpacingRadius;

                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(x, y, 0);


                //---   ROTATION
                newFFTObject.transform.LookAt(transform.position);
                newFFTObject.transform.localRotation *= Quaternion.Euler(-90, 0, 0);

                _FFTGameObjects[i] = newFFTObject;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            _FFTGameObjects[i].transform.localScale = _BaseScale + (_ScaleStrength * _FFT.GetBandValue(i, _FreqBands));
        }
    }
}

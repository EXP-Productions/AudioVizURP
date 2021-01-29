using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLineRenderer_v1 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBandCount;

    LineRenderer _Line;
    public float _LineLength = 2;
    float _Spacing = .2f;
    public float _Strength = 1;

    // Start is called before the first frame update
    void Start()
    {
        _Line = GetComponent<LineRenderer>();

        if(_FreqBandCount == FrequencyBandAnalyser.Bands.Eight)
        {
            _Line.positionCount = 8;
        }
        else
        {
            _Line.positionCount = 64;
        }

        _Spacing = _LineLength / _Line.positionCount;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _Line.positionCount; i++)
        {
            float xPos = i * _Spacing;
            float yPos = _FFT.GetBandValue(i, _FreqBandCount) * _Strength;

            Vector3 pos = new Vector3(xPos, yPos, 0);

            _Line.SetPosition(i, pos);
        }
    }
}

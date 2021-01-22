using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FreqBandData))]
[RequireComponent(typeof(LineRenderer))]
public class FFTLineRenderer : MonoBehaviour
{
    public FreqBandData _FreqBand;

    LineRenderer _Line;
    public float _Spacing = .2f;
    public float _Strength = 1;

    // Start is called before the first frame update
    void Start()
    {
        _Line = GetComponent<LineRenderer>();
        if(_FreqBand._FreqBands == FrequencyBandAnalyser.Bands.Eight)
        {
            _Line.positionCount = 8;
        }
        else
        {
            _Line.positionCount = 64;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _Line.positionCount; i++)
        {
            float normPosition = i / (float)_Line.positionCount;
            float xPos = i * _Spacing;
            float yPos = _FreqBand.GetFFTBandValue(i) * _Strength;

            Vector3 pos = new Vector3(xPos, yPos, 0);

            _Line.SetPosition(i, pos);
        }
    }
}

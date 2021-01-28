using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLineRenderer_v2 : MonoBehaviour
{
    public enum Shape
    {
        Line,
        Circle,
    }

    public Shape _Shape = Shape.Line;

    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.SixtyFour;

    LineRenderer _Line;
    public float _LineLengthRadius = 2;
    float _Spacing = .2f;
    public float _Strength = 1;

    // Start is called before the first frame update
    void Start()
    {
        _Line = GetComponent<LineRenderer>();

        if(_FreqBands == FrequencyBandAnalyser.Bands.Eight)
        {
            _Line.positionCount = 8;
        }
        else
        {
            _Line.positionCount = 64;
        }

        _Spacing = _LineLengthRadius / _Line.positionCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Shape == Shape.Line)
        {
            for (int i = 0; i < _Line.positionCount; i++)
            {
                float normPosition = i / (float)_Line.positionCount;
                float xPos = i * _Spacing;
                float yPos = _FFT.GetBandValue(i, _FreqBands) * _Strength;

                Vector3 pos = new Vector3(xPos, yPos, 0);

                _Line.SetPosition(i, pos);
            }
        }
        else if (_Shape == Shape.Circle)
        {
            float angleSpacing = (2f * Mathf.PI) / (int)_FreqBands;
            //---   CIRCULAR
            for (int i = 0; i < _Line.positionCount; i++)
            {
                float frequencyStrength = _FFT.GetBandValue(i, _FreqBands) * _Strength;
                float radius = _LineLengthRadius + frequencyStrength;

                float angle = i * angleSpacing;
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;

                Vector3 pos = new Vector3(x, y, 0);

                _Line.SetPosition(i, pos);
            }
        }
    }
}

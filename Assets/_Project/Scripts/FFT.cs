using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class FFT : MonoBehaviour
{
    public enum Bands
    {
        Eight = 8,
        SixtyFour = 64,
    }


    AudioSource _AudioSource;
    public int _FrequencyBins = 512;

    public float[] _Samples;
    public float[] _SampleBuffer;

    public float _SmoothDownRate = 0;

    public bool _DrawGizmos = false;

    public float[] _FreqBands8;
    public float[] _FreqBands64;


    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();

        _FreqBands8 = new float[8];
        _FreqBands64 = new float[64];
        _Samples = new float[_FrequencyBins];
        _SampleBuffer = new float[_FrequencyBins];
    }


    void UpdateFreqBands8()
    {
        // 22050 / 512 = 43hz per sample
        // 10 - 60 hz
        // 60 - 250
        // 250 - 500
        // 500 - 2000
        // 2000 - 4000
        // 4000 - 6000
        // 6000 - 20000

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += _Samples[count] * (count + 1);
                count++;
            }

            average /= count;
            _FreqBands8[i] = average;
        }
    }

    void UpdateFreqBands64()
    {
        // 22050 / 512 = 43hz per sample
        // 10 - 60 hz
        // 60 - 250
        // 250 - 500
        // 500 - 2000
        // 2000 - 4000
        // 4000 - 6000
        // 6000 - 20000

        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;

            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3)
                    sampleCount -= 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += _Samples[count] * (count + 1);
                count++;
            }

            average /= count;
            _FreqBands64[i] = average;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //---   POPULATE SAMPLES
        _AudioSource.GetSpectrumData(_SampleBuffer, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < _Samples.Length; i++)
        {
            if (_SampleBuffer[i] > _Samples[i])
                _Samples[i] = _SampleBuffer[i];
            else
                _Samples[i] = Mathf.Lerp(_Samples[i], _SampleBuffer[i], Time.deltaTime * _SmoothDownRate);
        }

        UpdateFreqBands8();
        UpdateFreqBands64();
    }

    //private void OnDrawGizmos()
    //{
    //    if (_DrawGizmos && Application.isPlaying)
    //    {
    //        float linearXPos0;
    //        float linearXPos1;
    //        float logXPos0;
    //        float logXPos1;

    //        float rawSample0;
    //        float rawSample1;
    //        float logSample0;
    //        float logSample1;
    //        for (int i = 1; i < _Samples.Length - 1; i++)
    //        {
    //            linearXPos0 = (float)(i - 1) / _FrequencyBins;
    //            linearXPos1 = (float)(i) / _FrequencyBins;
    //            logXPos0 = Mathf.Log(i - 1);
    //            logXPos1 = Mathf.Log(i);

    //            rawSample0 = _Samples[i-1];
    //            rawSample1 = _Samples[i];

    //            logSample0 = Mathf.Log(rawSample0);
    //            logSample1 = Mathf.Log(rawSample1);

    //            Gizmos.color = Color.white;
    //            //// X - Linear X spacing   Y - Raw sample
    //            //Gizmos.DrawLine(new Vector3(linearXPos0, rawSample0 + 10, 0), new Vector3(linearXPos1, rawSample1 + 10, 0));

    //            //// X - Linear X spacing   Y - Natural log sample
    //            //Gizmos.DrawLine(new Vector3(linearXPos0, logSample0 + 10, 2), new Vector3(linearXPos1, logSample1 + 10, 2));

    //            //// X - Natural log spacing   Y - Raw sample
    //            //Gizmos.DrawLine(new Vector3(logXPos0, rawSample0 * _Scalar - 10, 1), new Vector3(logXPos1, rawSample1 * _Scalar - 10, 1));

    //            //// X - Natural log spacing   Y - Natural Log sample
    //            //Gizmos.DrawLine(new Vector3(logXPos0, logSample0, 3), new Vector3(logXPos1, logSample1, 3));

    //            // X - Natural log spacing   Y - Raw sample
    //            Gizmos.DrawLine(new Vector3(logXPos0, _SamplesNormalized[i - 1] * _Scalar - 20, 0), new Vector3(logXPos1, _SamplesNormalized[i] * _Scalar - 20, 0));

    //            Gizmos.color = Color.blue;
    //            // X - Linear spacing   Y - Raw sample
    //            Gizmos.DrawLine(new Vector3(linearXPos0, _SamplesNormalized[i - 1] * _Scalar - 22, 0), new Vector3(linearXPos1, _SamplesNormalized[i] * _Scalar - 22, 0));
    //        }

    //        Gizmos.DrawLine(new Vector3(0, 0 * _Scalar - 20, 0), new Vector3(0, 1 * _Scalar - 20, 0));
    //    }
    //}
}

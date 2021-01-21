using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFT : MonoBehaviour
{
    AudioSource _AudioSource;
    public int _SampleCount = 512;

    public float[] _Samples;
    public float[] _SamplesNormalized;
    public float[] _LogSamplesNormalized;

    public float _SmoothDownRate = 0;

    public float _Scalar = 1;
 

    public float _MinSample = 999;
    public float _MaxSample = 0;


    public float _MaxLogSample = 0;
    public float _MinLogSample = 999;

    public bool _DrawGizmos = false;

    float _MaxLog = 0;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();

        //---   INITIALIZE ARRAYS
        _Samples = new float[_SampleCount];
        _SamplesNormalized = new float[_SampleCount];
        _LogSamplesNormalized = new float[_SampleCount];

        for (int i = 0; i < _SampleCount; i++)
        {
            float logx = Mathf.Log(i);
            if (logx > _MaxLog)
                _MaxLog = logx;

            //print(logx);
        }

        for (int i = 0; i < _SampleCount; i++)
        {
            print(LinearToLogIndex(i));
        }
    }



    // Update is called once per frame
    void Update()
    {
        //_MaxSample = 0;
        //_MaxLogSample = 0;

        //---   POPULATE SAMPLES
        _AudioSource.GetSpectrumData(_Samples, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < _Samples.Length; i++)
        {
            _MinLogSample = Mathf.Min(_LogSamplesNormalized[i], _MinLogSample);
            _MinSample = Mathf.Min(_Samples[i], _MinSample);

            _MaxLogSample = Mathf.Max(_LogSamplesNormalized[i], _MaxLogSample);
            _MaxSample = Mathf.Max(_Samples[i], _MaxSample);

            _LogSamplesNormalized[i] = Mathf.InverseLerp(_MinLogSample, _MaxLogSample, Mathf.Log(_Samples[i]));
            _SamplesNormalized[i] = Mathf.InverseLerp(_MinSample, _MaxSample, _Samples[i]);
        }
    }

    int LinearToLogIndex(int index)
    {       
        float logPos = Mathf.Log(index);
        return (int)(Mathf.InverseLerp(0, _MaxLog, logPos) * _SampleCount);
    }

    private void OnDrawGizmos()
    {
        if (_DrawGizmos && Application.isPlaying)
        {
            float linearXPos0;
            float linearXPos1;
            float logXPos0;
            float logXPos1;

            float rawSample0;
            float rawSample1;
            float logSample0;
            float logSample1;
            for (int i = 1; i < _Samples.Length - 1; i++)
            {
                linearXPos0 = (float)(i - 1) / _SampleCount;
                linearXPos1 = (float)(i) / _SampleCount;
                logXPos0 = Mathf.Log(i - 1);
                logXPos1 = Mathf.Log(i);

                rawSample0 = _Samples[i-1];
                rawSample1 = _Samples[i];

                logSample0 = Mathf.Log(rawSample0);
                logSample1 = Mathf.Log(rawSample1);

                Gizmos.color = Color.white;
                //// X - Linear X spacing   Y - Raw sample
                //Gizmos.DrawLine(new Vector3(linearXPos0, rawSample0 + 10, 0), new Vector3(linearXPos1, rawSample1 + 10, 0));

                //// X - Linear X spacing   Y - Natural log sample
                //Gizmos.DrawLine(new Vector3(linearXPos0, logSample0 + 10, 2), new Vector3(linearXPos1, logSample1 + 10, 2));

                //// X - Natural log spacing   Y - Raw sample
                //Gizmos.DrawLine(new Vector3(logXPos0, rawSample0 * _Scalar - 10, 1), new Vector3(logXPos1, rawSample1 * _Scalar - 10, 1));

                //// X - Natural log spacing   Y - Natural Log sample
                //Gizmos.DrawLine(new Vector3(logXPos0, logSample0, 3), new Vector3(logXPos1, logSample1, 3));

                // X - Natural log spacing   Y - Raw sample
                Gizmos.DrawLine(new Vector3(logXPos0, _SamplesNormalized[i - 1] * _Scalar - 20, 0), new Vector3(logXPos1, _SamplesNormalized[i] * _Scalar - 20, 0));

                Gizmos.color = Color.blue;
                // X - Linear spacing   Y - Raw sample
                Gizmos.DrawLine(new Vector3(linearXPos0, _SamplesNormalized[i - 1] * _Scalar - 22, 0), new Vector3(linearXPos1, _SamplesNormalized[i] * _Scalar - 22, 0));
            }

            Gizmos.DrawLine(new Vector3(0, 0 * _Scalar - 20, 0), new Vector3(0, 1 * _Scalar - 20, 0));
        }
    }
}

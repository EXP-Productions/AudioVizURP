using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class FFT : MonoBehaviour
{
    AudioSource _AudioSource;
    public int _FrequencyBins = 512;

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

    public float[] _FreqBands;

    float _MaxLog = 0;
    float naturalLog = 2.718281828459f;

    public AnimationCurve _Curve;

    public int _NumBands = 8;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();

        _FreqBands = new float[_NumBands];

        //---   INITIALIZE ARRAYS
        _Samples = new float[_FrequencyBins];
        _SamplesNormalized = new float[_FrequencyBins];
        _LogSamplesNormalized = new float[_FrequencyBins];

        for (int i = 0; i < _FrequencyBins; i++)
        {
            float logx = Mathf.Log(i);
            if (logx > _MaxLog)
                _MaxLog = logx;

            //print(logx);
        }


        for (int i = 0; i < _FrequencyBins; i++)
        {
            float norm = (float)i / (_FrequencyBins - 1f);
            float val = Mathf.Log(i) / _MaxLog;
            //print(val);

            float keyVal = norm;
            keyVal = 1 - keyVal;
            _Curve.AddKey(new Keyframe(keyVal, 1-val));

            //float normalizedLog = Mathf.Log(i) / _MaxLog;
            //float inverse = Mathf.Pow(naturalLog, normalizedLog);
            //print(inverse);
        }

        for (int i = 0; i < _FrequencyBins; i++)
        {
            float norm = (float)i / (_FrequencyBins - 1f);
            int index = (int)(_Curve.Evaluate(norm) * _FrequencyBins);
            print(index);
        }






      
    }


    void MakeFreqBands()
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
        float scalar = 4;

        for (int i = 0; i < _NumBands; i++)
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
            _FreqBands[i] = average * scalar;
        }
    }

    public float LinearIndexToLogSpacedValue(int index)
    {
        float freqSpacing = AudioSettings.outputSampleRate / 2 / _FrequencyBins;
        float freq = index * freqSpacing;
        float currentBark = 13 * Mathf.Atan(freq / 1315.8f) + 3.5f * Mathf.Atan(freq / 7518);
        int currentBin = (int)(currentBark / 24 * (_FrequencyBins - 1));

        currentBin = Mathf.Clamp(index, 0, _FrequencyBins - 1);

        return _SamplesNormalized[currentBin];
    }


    public float GetValueAtScaledIndex(int i)
    {
        float norm = (float)i / (_FrequencyBins - 1f);
        int index = (int)(_Curve.Evaluate(norm) * _FrequencyBins);
        //print(index);

        index = Mathf.Clamp(index, 0, _FrequencyBins-1);

        return _Samples[index];
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

        MakeFreqBands();
    }

    int LinearToLogIndex(int index)
    {       
        float logPos = Mathf.Log(index);
        return (int)(Mathf.InverseLerp(0, _MaxLog, logPos) * _FrequencyBins);
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
                linearXPos0 = (float)(i - 1) / _FrequencyBins;
                linearXPos1 = (float)(i) / _FrequencyBins;
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

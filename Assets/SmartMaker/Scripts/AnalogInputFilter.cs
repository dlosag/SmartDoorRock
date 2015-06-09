using UnityEngine;
using System.Collections;
using System;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Utility/AnalogInputFilter")]
	public class AnalogInputFilter : AppActionUtil
	{
		public AnalogInput analogInput;
		[Range(0f, 1f)]
		public float minValue = 0f;
		[Range(0f, 1f)]
		public float maxValue = 1f;
		public bool noiseFilter = false;
		[Range(0f, 1f)]
		public float sensitivity = 0.5f;

		private float _sensitivity;
		private int _sampleNum = 100;
		private ArrayList _originValues = new ArrayList();
		private ArrayList _values = new ArrayList();
		private ArrayList _momentums = new ArrayList();
		// Kalman filter's parameter
		private float _Q;
		private float _R;
		private float _P;
		private float _X;
		private float _K;

		// Use this for initialization
		void Start ()
		{
			Reset();

			if(analogInput != null)
			{
				analogInput.OnStarted.AddListener(OnAnalogPinStarted);
				analogInput.OnStopped.AddListener(OnAnalogPinStopped);
				analogInput.OnExcuted.AddListener(OnAnalogPinExcuted);
			}
		}
		
		// Update is called once per frame
		void Update ()
		{
		}
		
		void OnAnalogPinStarted()
		{
			Reset();
		}
		
		void OnAnalogPinStopped()
		{
		}
		
		void OnAnalogPinExcuted()
		{
			if(this.enabled == true)
			{
				if(_originValues.Count >= _sampleNum)
					_originValues.RemoveAt(0);
				if(_values.Count >= _sampleNum)
					_values.RemoveAt(0);
				if(_momentums.Count >= _sampleNum)
					_momentums.RemoveAt(0);

				float originValue = analogInput.Value;
				float filterValue = Mathf.Clamp(originValue, minValue, maxValue);
				filterValue = (filterValue - minValue) / (maxValue - minValue);

				if(_sensitivity != sensitivity)
					FilterReset();
				if(noiseFilter == true)
				{
					_K = (_P + _Q) / (_P + _Q + _R);
					_P = _R * (_P + _Q) / (_R + _P + _Q);
					filterValue = _X + (filterValue - _X) * _K;
					_X = filterValue;
				}
				else
					FilterReset();
				
				_originValues.Add(originValue);
				_values.Add(filterValue);
				float momentum = filterValue - (float)_values[_values.Count - 2];
				_momentums.Add(momentum);
			}
		}

		public void Reset()
		{
			FilterReset();

			_originValues.Clear();

			_values.Clear();
			_values.Add(0f);
			_values.Add(0f);
			_values.Add(0f);

			_momentums.Clear();
			_momentums.Add(0f);
			_momentums.Add(0f);
			_momentums.Add(0f);
		}

		private void FilterReset()
		{
			_sensitivity = sensitivity;
			_Q = 0.00001f + (0.001f * sensitivity);
			_R = 0.01f;
			_P = 1f;
			_X = 0f;
			_K = 0f;
		}

		public float Value
		{
			get
			{
				return (float)_values[_values.Count - 1];
			}
		}

		public float Momentum
		{
			get
			{
				return (float)_momentums[_momentums.Count - 1];
			}
		}

		public float[] OriginValues
		{
			get
			{
				return (float[])_originValues.ToArray(typeof(float));
			}
		}

		public float[] Values
		{
			get
			{
				return (float[])_values.ToArray(typeof(float));
			}
		}

		public float[] Momentums
		{
			get
			{
				return (float[])_momentums.ToArray(typeof(float));
			}
		}
	}
}

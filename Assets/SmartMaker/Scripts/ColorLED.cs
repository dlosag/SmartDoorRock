using UnityEngine;
using System.Collections;
using System;

namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Utility/ColorLED")]
	public class ColorLED : AppActionUtil
	{
		public AnalogOutput analogRed;
		public AnalogOutput analogGreen;
		public AnalogOutput analogBlue;

		public Color color;

		[SerializeField]
		private float _calibrationRed = 0f;
		[SerializeField]
		private float _calibrationGreen = 0f;
		[SerializeField]
		private float _calibrationBlue = 0f;

		private Color _color;

		void Awake()
		{
			if(analogRed != null)
				analogRed.OnStarted.AddListener(OnStartedRed);
			if(analogGreen != null)
				analogGreen.OnStarted.AddListener(OnStartedGreen);
			if(analogBlue != null)
				analogBlue.OnStarted.AddListener(OnStartedBlue);
		}

		// Use this for initialization
		void Start ()
		{
		
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(color != _color)
			{
				_color = color;
				DoLEDControl();
			}
		}

		public float calibrationRed
		{
			get
			{
				return _calibrationRed;
			}
			set
			{
				float newCalibration = Mathf.Clamp(value, -1f, 1f);
				if(_calibrationRed != newCalibration)
				{
					_calibrationRed = newCalibration;
					DoLEDControl();
				}
			}
		}

		public float calibrationGreen
		{
			get
			{
				return _calibrationGreen;
			}
			set
			{
				float newCalibration = Mathf.Clamp(value, -1f, 1f);
				if(_calibrationGreen != newCalibration)
				{
					_calibrationGreen = newCalibration;
					DoLEDControl();
				}
			}
		}

		public float calibrationBlue
		{
			get
			{
				return _calibrationBlue;
			}
			set
			{
				float newCalibration = Mathf.Clamp(value, -1f, 1f);
				if(_calibrationBlue != newCalibration)
				{
					_calibrationBlue = newCalibration;
					DoLEDControl();
				}
			}
		}

		private void DoLEDControl()
		{
			if(analogRed != null)
			{
				if(analogRed.Started == true)
					analogRed.value = _color.r + calibrationRed;
			}
			if(analogGreen != null)
			{
				if(analogGreen.Started == true)
					analogGreen.value = _color.g + calibrationGreen;
			}
			if(analogBlue != null)
			{
				if(analogBlue.Started == true)
					analogBlue.value = _color.b + calibrationBlue;
			}
		}

		private void OnStartedRed()
		{
			DoLEDControl();
		}

		private void OnStartedGreen()
		{
			DoLEDControl();
		}

		private void OnStartedBlue()
		{
			DoLEDControl();
		}
	}
}
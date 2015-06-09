using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Utility/AnalogInputDrag")]
	public class AnalogInputDrag : AppActionUtil
	{
		public AnalogInput analogInput;
		[Range(0f, 1f)]
		public float dragMinRatio = 0.1f;
		[Range(0f, 1f)]
		public float dragMaxRatio = 0.9f;
		public float dragForceScaler = 1f;

		public UnityEvent OnDragStart;
		public UnityEvent OnDragMove;
		public UnityEvent OnDragEnd;

		private bool _drag;
		private float _value;
		private float _preValue;
		private float _deltaTime;
		private float _preTime;

		// Use this for initialization
		void Start ()
		{
			_value = 0f;
			_preValue = _value;

			if(analogInput != null)
			{
				analogInput.OnStarted.AddListener(OnStarted);
				analogInput.OnExcuted.AddListener(OnExcuted);
				analogInput.OnStopped.AddListener(OnStopped);
			}
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(analogInput != null)
			{
				if(analogInput.Started == true)
				{

				}
				else
				{
					if(_drag == true)
					{
						OnDragEnd.Invoke();
					}
				}
			}		
		}

		private void OnStarted()
		{
			_drag = false;
			_value = 0f;
			_preValue = _value;
		}

		private void OnExcuted()
		{
			float curValue = analogInput.Value;
			if(_drag == true)
			{
				if(curValue > dragMinRatio && curValue < dragMaxRatio)
				{
					float time = Time.time;
					_deltaTime = time - _preTime;
					_preTime = time;
					_preValue = _value;
					_value = curValue;
					if(_value != _preValue)
						OnDragMove.Invoke();
				}
				else
				{
					_drag = false;
					OnDragEnd.Invoke();
				}
			}
			else
			{
				if(curValue > dragMinRatio && curValue < dragMaxRatio)
				{
					_drag = true;
					_value = curValue;
					_preValue = _value;
					_preTime = Time.time;

					OnDragStart.Invoke();
				}
			}
		}

		private void OnStopped()
		{
			_drag = false;
		}

		public bool isDragging
		{
			get
			{
				return _drag;
			}
		}

		public float Value
		{
			get
			{
				return _value;
			}
		}

		public float DragForce
		{
			get
			{
				float diff = _value - _preValue;
				float force = 0f;
				if(diff != 0f)
					force = diff / _deltaTime;
				return force * dragForceScaler;
			}
		}
	}
}

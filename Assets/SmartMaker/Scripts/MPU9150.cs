using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/Add-on/MPU9150")]
	public class MPU9150 : AppAction
	{
		public int pin;

		public Transform target;
		public Vector3 offsetAngles = Vector3.zero;

		public UnityEvent OnCalibrated;

		private Quaternion _rotation;
		private Quaternion _calibRotation;
		private Quaternion _curRotation;
		private Quaternion _fromRotation;
		private Quaternion _toRotation;
		private float _time;
		private float _refTime;
		private bool _calibrated = false;

		protected short _qX;
		protected short _qY;
		protected short _qZ;
		protected short _qW;
		protected ushort _intervalTime;

		void Awake()
		{
			_time = 1f;
		}
		
		// Use this for initialization
		void Start ()
		{
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(_time < 1f)
			{
				_time += (Time.deltaTime / _refTime);
				_curRotation = Quaternion.Lerp(_fromRotation, _toRotation, _time);
			}
			else
				_curRotation = _toRotation;
			
			if(target != null)
				target.localRotation = _curRotation;
		}

		public override string SketchDeclaration()
		{
			return string.Format("{0} {1}({2:d});", this.GetType().Name, SketchVarName, id);
		}
		
		public override string SketchVarName
		{
			get
			{
				return string.Format("imu{0:d}", id);
			}
		}
		
		protected override void OnActionStart ()
		{
			_calibRotation = Quaternion.identity;
			_curRotation = Quaternion.identity;
			_toRotation = Quaternion.identity;
			_calibrated = true;
			_time = 1f;
		}
		
		protected override void OnActionStop ()
		{
			_toRotation =  Quaternion.identity;
			_toRotation.eulerAngles += offsetAngles;
			_curRotation = _toRotation;
		}
		
		protected override void OnActionExcute ()
		{
			_fromRotation = _toRotation;
			_toRotation =  _rotation;
			if(_calibrated == true)
			{
				_calibRotation = _rotation;
				Vector3 euler = _calibRotation.eulerAngles;
				euler.x = 0f;
				euler.z = 0f;
				_calibRotation.eulerAngles = euler;
				_calibrated = false;

				OnCalibrated.Invoke();
			}
			_toRotation.eulerAngles += (offsetAngles - _calibRotation.eulerAngles);
			
			_time = 0f;
		}

		protected override void OnPop ()
		{
			Pop(ref _qX);
			Pop(ref _qY);
			Pop(ref _qZ);
			Pop(ref _qW);
			Pop(ref _intervalTime);

			_refTime = (float)_intervalTime / 1000f;
			_rotation = new Quaternion((float)_qX * -0.0001f
			                        ,(float)_qZ * -0.0001f
			                        ,(float)_qY * -0.0001f
			                        ,(float)_qW * 0.0001f);
		}
		
		protected override void OnPush ()
		{
		}
		
		public Quaternion Rotation
		{
			get
			{
				return _curRotation;
			}
		}

		public float IntervalTime
		{
			get
			{
				return (float)_intervalTime * 0.001f;
			}
		}

		public void Calibration()
		{
			_calibrated = true;
		}
	}
}

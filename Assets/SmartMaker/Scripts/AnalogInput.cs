using UnityEngine;
using System.Collections;
using System;

namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/AnalogInput")]
	public class AnalogInput : AppAction
	{
		public int pin;
		public int resolution = 1024;

		protected ushort _newValue;
		protected ushort _value;

		void Awake()
		{
		}
		
		// Use this for initialization
		void Start ()
		{
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}

		public float Value
		{
			get
			{
				return (float)_value / (float)(resolution - 1);
			}
		}
		
		public override string SketchDeclaration()
		{
			return string.Format("{0} {1}({2:d}, A{3:d});", this.GetType().Name, SketchVarName, id, pin);
		}

		public override string SketchVarName
		{
			get
			{
				return string.Format("aInput{0:d}", id);
			}
		}

		protected override void OnActionStart ()
		{
			_newValue = _value;
		}
		
		protected override void OnActionStop ()
		{
			
		}
		
		protected override void OnActionExcute ()
		{
			if(_newValue != _value)
			{
				_value = _newValue;
			}
		}
		
		protected override void OnPop ()
		{
			Pop(ref _newValue);
		}
		
		protected override void OnPush ()
		{
		}
	}
}

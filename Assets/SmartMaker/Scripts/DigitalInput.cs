using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/DigitalInput")]
	public class DigitalInput : AppAction
	{
		public int pin;
		public bool pullup;

		public UnityEvent OnChangedValue;

		private bool _first;
		private byte _newValue;
		private byte _value;
		
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

		public bool Value
		{
			get
			{
				if(_value == 0)
					return false;
				else
					return true;
			}
		}

		public override string SketchDeclaration()
		{
			string code = string.Format("{0} {1}({2:d}, {3:d}, ", this.GetType().Name, SketchVarName, id, pin);
			if(pullup == true)
				code += "true);";
			else
				code += "false);";

			return code;
		}

		public override string SketchVarName
		{
			get
			{
				return string.Format("dInput{0:d}", id);
			}
		}
		
		protected override void OnActionSetup ()
		{
		}
		
		protected override void OnActionStart ()
		{
			_newValue = _value;
			_first = true;
		}
		
		protected override void OnActionExcute ()
		{
			if(_newValue != _value || _first == true)
			{
				_value = _newValue;
				_first = false;
				OnChangedValue.Invoke();
			}
		}
		
		protected override void OnActionStop ()
		{
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

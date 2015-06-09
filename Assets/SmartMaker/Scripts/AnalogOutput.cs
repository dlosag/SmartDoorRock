using UnityEngine;
using System.Collections;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/AnalogOutput")]
	public class AnalogOutput : AppAction
	{
		public int pin;

		public float value;

		private float _value;
		private byte _bValue;


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
			if(value != _value)
			{
				_value = value;
				int iValue = (int)Mathf.Clamp(_value * 255f, 0f, 255f);
				_bValue = (byte)iValue;
				SetDirty();
			}
		}

		public override string SketchDeclaration()
		{
			return string.Format("{0} {1}({2:d}, {3:d});", this.GetType().Name, SketchVarName, id, pin);
		}

		public override string SketchVarName
		{
			get
			{
				return string.Format("aOutput{0:d}", id);
			}
		}
		
		protected override void OnActionSetup ()
		{
		}
		
		protected override void OnActionStart ()
		{
			autoUpdate = false;
		}
		
		protected override void OnActionExcute ()
		{
		}
		
		protected override void OnActionStop ()
		{
		}
		
		protected override void OnPop ()
		{
		}
		
		protected override void OnPush ()
		{
			Push(_bValue);
		}
	}
}

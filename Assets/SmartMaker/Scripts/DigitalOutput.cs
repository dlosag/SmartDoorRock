using UnityEngine;
using System.Collections;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/DigitalOutput")]
	public class DigitalOutput : AppAction
	{
		public int pin;

		public bool value;

		private bool _value;
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
				_bValue = 0;
				if(_value == true)
					_bValue = 1;

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
				return string.Format("dOutput{0:d}", id);
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

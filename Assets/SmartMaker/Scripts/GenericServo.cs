using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/AppActions/GenericServo")]
	public class GenericServo : AppAction
	{
		public int pin;
		public float offsetAngle = 0f;
		public float angle;

		private float _angle;
		private byte _bAngle;
		
		void Awake()
		{
		}
		
		// Use this for initialization
		void Start ()
		{
			_angle = -180f;
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(angle != _angle)
			{
				_angle = angle;
				int iAngle = (int)Mathf.Clamp(_angle + 90f, 0f, 180f);
				_bAngle = (byte)iAngle;
				SetDirty();
			}
		}

		public override string[] SketchIncludes()
		{
			List<string> includes = new List<string>();
			includes.Add("#include <Servo.h>");
			return includes.ToArray();
		}
		
		public override string SketchDeclaration()
		{
			return string.Format("{0} {1}({2:d}, {3:d});", this.GetType().Name, SketchVarName, id, pin);
		}

		public override string SketchVarName
		{
			get
			{
				return string.Format("servo{0:d}", id);
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
			int angle = (int)Mathf.Clamp((float)_bAngle + offsetAngle, 0f, 180f);
			Push((byte)angle);
		}
	}
}

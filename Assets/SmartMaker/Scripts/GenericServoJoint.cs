using UnityEngine;
using System.Collections;

namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Utility/GenericServoJoint")]
	public class GenericServoJoint : AppActionUtil
	{
		public GenericServo genericServo;
		public Vector3 forward;
		public Vector3 up;
		public float drawScale = 1f;

		private Quaternion _initRot;
		private float _angle;
		private Vector3 _preForward;
		private Vector3 _forward;
		private Vector3 _up;
		private Vector3 _right;

		// Use this for initialization
		void Start ()
		{
			_initRot = transform.localRotation;
			_forward = forward;
			_up = up;
			_right = Vector3.Cross(up, forward);

			Reset();
		}
		
		// Update is called once per frame
		void Update ()
		{
			Quaternion rot = transform.localRotation * Quaternion.Inverse(_initRot);
			Vector3 vec = rot * _forward;
			vec = Vector3.Project(vec, _forward) + Vector3.Project(vec, _right);
			vec.Normalize();
			float angle = Vector3.Angle(_preForward, vec);
			float dir = Vector3.Dot(Vector3.Cross(_preForward, vec), _up);		
			if (dir < 0f)
				angle = -angle;		
			_preForward = vec;
			_angle += angle;

			if(genericServo != null)
				genericServo.angle = _angle;
		}

		void OnDrawGizmos()
		{
			if(Application.isPlaying == false)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * up) * drawScale);

				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * forward) * drawScale);
			}
			else
			{
				Quaternion rot = Quaternion.identity;
				if(transform.parent != null)
					rot = transform.parent.rotation * _initRot;
				else
					rot = _initRot;

				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, transform.position + (rot * _up) * drawScale);
				
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, transform.position + (rot * _forward) * drawScale);

				if(_angle != 0f)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(transform.position, transform.position + (rot * _preForward) * drawScale);
				}
			}
		}

		public void Reset()
		{
			transform.localRotation = _initRot;
			_preForward = _forward;
			_angle = 0f;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Internal/AppAction")]
	public class AppAction : MonoBehaviour
	{
		public int id;

		public UnityEvent OnStarted;
		public UnityEvent OnExcuted;
		public UnityEvent OnStopped;

		private List<byte> _dataBytes = new List<byte>();
		private const int _maxNumBytes = 116;
		[SerializeField] private byte _autoUpdate = 1;
		private bool _updated;
		private bool _started;
		private bool _dirty;

		public bool autoUpdate
		{
			get
			{
				if(_autoUpdate == 0)
					return false;
				else
					return true;
			}
			set
			{
				byte newValue = 0;
				if(value == true)
					newValue = 1;

				if(_autoUpdate == newValue)
					return;

				_autoUpdate = newValue;
				SetDirty();
			}
		}		

		public byte[] dataBytes
		{
			get
			{
				if(_dirty == false)
					return null;
				
				_dirty = false;
				_dataBytes.Clear();
				OnPush();
				Push(_autoUpdate);
				
				if(_dataBytes.Count == 0)
					return null;
				else
					return _dataBytes.ToArray();
			}
			set
			{
				_dataBytes.Clear();
				_dataBytes.AddRange(value);
				OnPop();
				_updated = true;
			}
		}
		
		public bool Started
		{
			get
			{
				return _started;
			}
		}

		public void ActionSetup()
		{
			OnActionSetup();
		}
		
		public void ActionStart()
		{
			SetDirty();
			_updated = false;
			_started = true;
			
			OnActionStart();
			OnStarted.Invoke();
		}
		
		public void ActionExcute()
		{
			if(_updated == true)
			{
				OnActionExcute();
				_updated = false;
				
				OnExcuted.Invoke();
			}
		}
		
		public void ActionStop()
		{
			_started = false;
			
			OnActionStop();
			OnStopped.Invoke();
		}
		
		protected bool Push(byte value)
		{
			if((_maxNumBytes - _dataBytes.Count) < 1)
				return false;
			
			_dataBytes.Add(value);
			return true;
		}
		
		protected bool Push(ushort value)
		{
			if((_maxNumBytes - _dataBytes.Count) < 2)
				return false;
			
			_dataBytes.Add((byte)(value & 0xFF));
			_dataBytes.Add((byte)((value >> 8) & 0xFF));
			return true;
		}
		
		protected bool Push(short value)
		{
			ushort binary = 0;
			if(value < 0)
			{
				value *= -1;
				binary = (ushort)value;
				binary |= (ushort)0x8000;
			}
			else
				binary = (ushort)value;
			
			return Push(binary);
		}
		
		protected bool Push(byte[] value)
		{
			if((_maxNumBytes - _dataBytes.Count) < value.Length)
				return false;
			
			_dataBytes.AddRange(value);
			return true;
		}
		
		protected bool Pop(ref byte value)
		{
			if(_dataBytes.Count < 1)
				return false;
			value = _dataBytes[0];
			_dataBytes.RemoveAt(0);
			return true;
		}
		
		protected bool Pop(ref ushort value)
		{
			if(_dataBytes.Count < 2)
				return false;
			
			value = (ushort)(((_dataBytes[1] << 8) & 0xFF00) | (_dataBytes[0] & 0xFF));
			_dataBytes.RemoveRange(0, 2);
			return true;
		}
		
		protected bool Pop(ref short value)
		{
			ushort binary = 0;
			if(Pop(ref binary) == false)
				return false;
			
			value = (short)(binary & 0x7FFF);
			if((binary & 0x8000) == 0x8000)
				value *= -1;
			return true;
		}
		
		protected bool Pop(ref byte[] value, int count)
		{
			if(_dataBytes.Count < count)
				return false;
			
			value = _dataBytes.GetRange(0, count).ToArray();
			_dataBytes.RemoveRange(0, count);
			return true;
		}

		protected void SetDirty()
		{
			_dirty = true;
		}

		public virtual string[] SketchIncludes()
		{
			return null;
		}

		public virtual string SketchDeclaration()
		{
			return "";
		}

		public virtual string SketchVarName
		{
			get
			{
				return string.Format("noName{0:d}", id);
			}
		}

		protected virtual void OnActionSetup()
		{
		}
		
		protected virtual void OnActionStart()
		{
		}
		
		protected virtual void OnActionStop()
		{
		}
		
		protected virtual void OnActionExcute()
		{
		}
		
		protected virtual void OnPush()
		{
		}
		
		protected virtual void OnPop()
		{
		}
	}
}
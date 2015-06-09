using UnityEngine;
using System.Collections;
using System;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Internal/CommObject")]
	public class CommObject : MonoBehaviour
	{
		public EventHandler OnOpened;
		public EventHandler OnOpenFailed;
		public EventHandler OnErrorClosed;

		public virtual void Open()
		{
		}

		public virtual void Close()
		{
		}

		public virtual void Write(byte[] bytes)
		{
		}

		public virtual byte[] Read()
		{
			return null;
		}

		public virtual bool IsOpen
		{
			get
			{
				return false;
			}
		}
	}
}


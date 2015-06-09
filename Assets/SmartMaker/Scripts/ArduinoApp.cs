using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/ArduinoApp")]
	public class ArduinoApp : MonoBehaviour
	{
		public CommObject commObject;
		public float timeoutSec = 5f;
		public int uartNum = 0;
		public int uartBaudrate = 115200;

		public UnityEvent OnConnected;
		public UnityEvent OnConnectionFailed;
		public UnityEvent OnDisconnected;

		private enum CMD
		{
			Start = 0x80, //128
			Exit = 0x81,  //129
			Update = 0x82, //130
			Action = 0x83, //131
			Ready = 0x84, //132
			Ping = 0x85 //133
		}

		private AppAction[] _actions;
		private bool _opened = false;
		private bool _connected = false;
		private float _time = 0f;
		private float _timeout = 0;
		private bool _processProtocolTx = false;
		private int _processUpdate = 0;
		private byte _id;
		private byte _numData;
		private List<byte> _rxDataBytes = new List<byte>();

		void Awake()
		{
			if(commObject != null)
			{
				commObject.OnOpened += CommOpenEventHandler;
				commObject.OnOpenFailed += CommOpenFailEventHandler;
				commObject.OnErrorClosed += CommErrorCloseEventHandler;

				CommSerial serial = (CommSerial)commObject;
				if(serial != null)
					serial.baudrate = uartBaudrate;
			}
		}

		// Use this for initialization
		void Start ()
		{
			_actions = appActions;
			foreach(AppAction action in _actions)
				action.ActionSetup();
		}
			
		// Update is called once per frame
		void Update ()
		{
			if(_opened == true)
			{
				// Process RX
				byte[] readBytes = commObject.Read();
				bool update = false;
				if(readBytes != null)
				{
					for(int i=0; i<readBytes.Length; i++)
					{
						if(_connected == false)
						{
							if(readBytes[i] == (byte)CMD.Ping)
							{
								commObject.Write(new byte[] { (byte)CMD.Start, (byte)CMD.Ready });
								foreach(AppAction action in _actions)
									action.ActionStart();
								
								TimeoutReset();
								_connected = true;
								_processProtocolTx = true;
								OnConnected.Invoke();
							}
						}
						else
						{
							if(readBytes[i] == (byte)CMD.Ping)
							{
								TimeoutReset();
								_processUpdate = 0;
							}
							else if(readBytes[i] == (byte)CMD.Ready)
							{
								TimeoutReset();
								_processUpdate = 0;
								_processProtocolTx = true;
							}
							else if(readBytes[i] == (byte)CMD.Update)
							{
								TimeoutReset();
								_processUpdate = 1;
							}
							else if(readBytes[i] == (byte)CMD.Action)
							{
								TimeoutReset();
								if(_processUpdate > 0)
								{
									foreach(AppAction action in _actions)
										action.ActionExcute();
									
									update = true;
								}
								_processUpdate = 0;
							}
							else if(_processUpdate > 0 && readBytes[i] < 0x80)
							{
								TimeoutReset();
								if(_processUpdate == 1)
								{
									_id = readBytes[i];
									_processUpdate = 2;
								}
								else if(_processUpdate == 2)
								{
									_numData = readBytes[i];
									_processUpdate = 3;
									_rxDataBytes.Clear();
								}
								else if(_processUpdate == 3)
								{
									if(_rxDataBytes.Count < _numData)
										_rxDataBytes.Add(readBytes[i]);
									
									if(_rxDataBytes.Count >= _numData)
									{
										// Decoding 7bit bytes
										byte bit = 1;
										for(int j=0; j<_rxDataBytes.Count; j++)
										{
											if(bit == 1)
											{
												_rxDataBytes[j] = (byte)(_rxDataBytes[j] << bit);
												bit++;
											}
											else if(bit == 8)
											{
												_rxDataBytes[j - 1] |= _rxDataBytes[j];
												_rxDataBytes.RemoveAt(j);
												j--;
												bit = 1;
											}
											else
											{
												_rxDataBytes[j - 1] |= (byte)(_rxDataBytes[j] >> (7 - bit + 1));
												if(j == (_rxDataBytes.Count - 1))
													_rxDataBytes.RemoveAt(j);
												else
													_rxDataBytes[j] = (byte)(_rxDataBytes[j] << bit);
												bit++;
											}
										}
										foreach(AppAction action in _actions)
										{
											if(action.id == _id)
												action.dataBytes = _rxDataBytes.ToArray();
										}
										_processUpdate = 1;
									}
								}
							}
							else
								_processUpdate = 0;
						}
					}
					
					if(update == true)
					{
						commObject.Write(new byte[] { (byte)CMD.Ready });
					}
				}
				
				// Process TX
				if(_connected == true)
				{
					if(_processProtocolTx == true)
					{
						List<byte> writeBytes = new List<byte>();
						foreach(AppAction action in _actions)
						{
							byte[] dataBytes = action.dataBytes;
							if(dataBytes != null)
							{
								writeBytes.Add((byte)(action.id & 0x7F));
								
								// Encoding 7bit bytes
								List<byte> data7bitBytes = new List<byte>();
								byte bit = 1;
								byte temp = 0;
								for(int i=0; i<dataBytes.Length; i++)
								{
									data7bitBytes.Add((byte)((temp | (dataBytes[i] >> bit)) & 0x7F));
									if(bit == 7)
									{
										data7bitBytes.Add((byte)(dataBytes[i] & 0x7F));
										bit = 1;
										temp = 0;
									}
									else
									{
										temp = (byte)(dataBytes[i] << (7 - bit));
										if(i == (dataBytes.Length - 1))
											data7bitBytes.Add((byte)(temp & 0x7F));
										bit++;
									}
								}
								
								writeBytes.Add((byte)data7bitBytes.Count); // num bytes
								writeBytes.AddRange(data7bitBytes.ToArray());
							}
						}
						
						if(writeBytes.Count > 0)
						{
							writeBytes.Insert(0, (byte)CMD.Update); // Update
							writeBytes.Add((byte)CMD.Action); // Action
							commObject.Write (writeBytes.ToArray());
						}
						else
							commObject.Write(new byte[] { (byte)CMD.Update, (byte)CMD.Action });
						
						_processProtocolTx = false;
					}
				}
				
				// try reconnection
				if(_time > 0.5f) // per time
				{
					_time = 0;
					if(_connected == false)
						commObject.Write(new byte[] { (byte)CMD.Ping });
					else
						commObject.Write(new byte[] { (byte)CMD.Ready, (byte)CMD.Update, (byte)CMD.Action });
				}
				else
					_time += Time.deltaTime;
				
				// Check timeout
				if(_timeout > timeoutSec) // wait until timeout seconds
					ErrorDisconnect();
				else
					_timeout += Time.deltaTime;
			}
		}

		public AppAction[] appActions
		{
			get
			{
				List<AppAction> listActions = new List<AppAction>(GameObject.FindObjectsOfType<AppAction>());
				for(int i=0; i<listActions.Count; i++)
				{
					if(listActions[i].enabled == false)
					{
						listActions.RemoveAt(i);
						i--;
					}
				}

				return listActions.ToArray();
			}
		}

		public bool Connected
		{
			get
			{
				return _connected;
			}
		}

		public void Connect()
		{
			if(commObject == null)
				return;

			commObject.Open();
		}

		private void ErrorDisconnect()
		{
			bool state = _opened;
			_connected = false;
			_opened = false;
			
			commObject.Close();

			foreach(AppAction action in _actions)
				action.ActionStop();

			if(state == false)
			{
				Debug.Log("Failed to open CommObject!");
				OnConnectionFailed.Invoke();
			}
			else
			{
				Debug.Log("Lost connection!");
				OnDisconnected.Invoke();
			}
		}

		public void Disconnect()
		{
			if(commObject == null)
				return;

			if(_connected == true)
				commObject.Write(new byte[] { (byte)CMD.Exit });

			_connected = false;
			_opened = false;

			commObject.Close();

			foreach(AppAction action in _actions)
				action.ActionStop();
			
			OnDisconnected.Invoke();
		}

		private void TimeoutReset()
		{
			_time = 0;
			_timeout = 0;
		}

		private void CommOpenEventHandler(object sender, EventArgs e)
		{
			_opened = true;
			TimeoutReset();
			commObject.Write(new byte[] { (byte)CMD.Ping });
		}

		private void CommOpenFailEventHandler(object sender, EventArgs e)
		{
			ErrorDisconnect();
		}

		private void CommErrorCloseEventHandler(object sender, EventArgs e)
		{
			ErrorDisconnect();
		}
	}
}

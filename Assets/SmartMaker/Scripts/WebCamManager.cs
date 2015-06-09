using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Unity3D/WebCamManager")]
	public class WebCamManager : MonoBehaviour
	{
		[SerializeField]
		public List<string> deviceNames = new List<string>();
		public string deviceName;
		public int capWidth = 320;
		public int capHeight = 240;
		public int capFPS = 30;

		public Material material;
		public RawImage uiImage;

		public Text uiText;
		public RectTransform uiPanel;
		public GameObject uiItem;

		private WebCamTexture _webcam = null;

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

		public void DeviceSearch()
		{
			deviceNames.Clear();

			WebCamDevice[] devices = WebCamTexture.devices;
			foreach(WebCamDevice device in devices)
				deviceNames.Add(device.name);

			if(uiPanel != null && uiItem != null)
			{
				List<GameObject> items = new List<GameObject>();
				foreach(RectTransform rect in uiPanel)
				{
					if(rect.gameObject.Equals(uiItem) == false)
						items.Add(rect.gameObject);						
				}
				
				foreach(GameObject go in items)
					GameObject.DestroyImmediate(go);
				
				Text t = uiItem.GetComponent<Text>();
				if(t == null)
					t = uiItem.GetComponentInChildren<Text>();
				
				if(deviceNames.Count == 0)
				{
					if(t != null)
						t.text = "";
				}
				else
				{
					if(t != null)
						t.text = deviceNames[0];
					
					for(int i=1; i<deviceNames.Count; i++)
					{
						GameObject item = GameObject.Instantiate(uiItem);
						item.transform.SetParent(uiPanel.transform);
						t = item.GetComponent<Text>();
						if(t == null)
							t = item.GetComponentInChildren<Text>();
						if(t != null)
							t.text = deviceNames[i];
					}
				}
			}
		}

		public void Play()
		{
			if(_webcam == null)
				_webcam = new WebCamTexture();

			if(_webcam.isPlaying == true)
				_webcam.Stop();

			_webcam.deviceName = deviceName;
			_webcam.requestedWidth = capWidth;
			_webcam.requestedHeight = capHeight;
			_webcam.requestedFPS = capFPS;
			_webcam.Play();

			if(material != null)
				material.mainTexture = _webcam;

			if(uiImage != null)
				uiImage.texture = _webcam;
		}

		public void Pause()
		{
			if(_webcam == null)
				_webcam = new WebCamTexture();

			_webcam.Pause();
		}

		public void Stop()
		{
			if(_webcam == null)
				_webcam = new WebCamTexture();
			
			_webcam.Stop();
		}

		public bool isPlaying
		{
			get
			{
				if(_webcam == null)
					return false;

				return _webcam.isPlaying;
			}
		}

		public int currentWidth
		{
			get
			{
				return _webcam.width;
			}
		}

		public int currentHeight
		{
			get
			{
				return _webcam.height;
			}
		}

		public void SelectDeviceName(Text text)
		{
			deviceName = text.text;
			if(uiText != null)
				uiText.text = deviceName;
		}
	}
}

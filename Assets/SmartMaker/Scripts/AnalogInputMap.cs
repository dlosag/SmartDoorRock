using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SmartMaker
{
	[AddComponentMenu("SmartMaker/Utility/AnalogInputMap")]
	public class AnalogInputMap : AppActionUtil
	{
		[Serializable]
		public class MapSmaple
		{
			public float analogValue;
			public float mapValue;
		}

		public AnalogInput analogInput;
		public float scaler = 1f;
		public MapSmaple[] mapSamples;

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
				if(analogInput == null)
					return 0f;

				float sampleValue = analogInput.Value * scaler;
				float analog_a = 0;
				float analog_b = 0;
				float map_a = 0;
				float map_b = 0;
				for(int i=0; i<mapSamples.Length; i++)
				{
					if(i == 0 && sampleValue <= mapSamples[i].analogValue)
					{
						analog_a = mapSamples[i].analogValue;
						analog_b = mapSamples[i].analogValue;
						map_a = mapSamples[i].mapValue;
						map_b = mapSamples[i].mapValue;
						break;
					}
					else if(i == mapSamples.Length - 1 && sampleValue >= mapSamples[i].analogValue)
					{
						analog_a = mapSamples[i].analogValue;
						analog_b = mapSamples[i].analogValue;
						map_a = mapSamples[i].mapValue;
						map_b = mapSamples[i].mapValue;
						break;
					}
					else
					{
						if(sampleValue >= mapSamples[i].analogValue && sampleValue < mapSamples[i+1].analogValue)
						{
							analog_a = mapSamples[i].analogValue;
							analog_b = mapSamples[i+1].analogValue;
							map_a = mapSamples[i].mapValue;
							map_b = mapSamples[i+1].mapValue;
							break;
						}
					}
				}
				
				float mapValue = 0;
				if(map_a == map_b)
					mapValue = map_a;
				else
				{
					float a = (map_a - map_b) / (analog_a - analog_b);
					mapValue = a * (sampleValue - analog_b) + map_b;
				}
				
				return mapValue;
			}
		}
	}
}

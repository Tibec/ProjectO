using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Calculation of FPS (Frame per Second) average, Rendered triangules and vertices, and time from data transit between cluster nodes.
/// </summary>
public class Statistics : MonoBehaviour {
		


		public float fpsMeasurePeriod = 1.0f;
		private int m_FpsAccumulator = 0;
		private float m_FpsNextPeriod = 0;
		private int m_CurrentFps;
		const string display = "{0} FPS";


		
		private double transitTime;
		
		void Start () {
			
			m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
			transitTime = 0;
		}
		
		void Update () {


			m_FpsAccumulator++;
			if (Time.realtimeSinceStartup > m_FpsNextPeriod)
			{
				m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
				m_FpsAccumulator = 0;
				m_FpsNextPeriod += fpsMeasurePeriod;
				print(string.Format(display, m_CurrentFps)+ "#TransitTime#" +  Math.Round(transitTime) + "#" );
			}

		}

		void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
			float ping = 1.0F;
			
			if (stream.isWriting) {
				stream.Serialize(ref ping);
			} else {
				transitTime = (Network.time - info.timestamp)*1000;
				stream.Serialize(ref ping);
			}
		}
		
	}

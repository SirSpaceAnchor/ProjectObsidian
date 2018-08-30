using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PO
{
	public class VerticalPlatform : MonoBehaviour
	{
		private PlatformEffector2D effector;
		public float waitTime;

		void Start()
		{
			effector = GetComponent<PlatformEffector2D>();
		}

		void Update()
		{
			if(Input.GetAxisRaw("Vertical") >= 0)
			{
				waitTime = 0.5f;
			}

			if(Input.GetAxisRaw("Vertical") <= 0)
			{
				if(waitTime <= 0)
				{
					effector.rotationalOffset = 180f;
					waitTime = 0.5f;
				}
				else
				{
					waitTime -= Time.deltaTime;
				}
			}

			if(Input.GetAxisRaw("Vertical") == 0)
			{
				effector.rotationalOffset = 0;
			}
		}
	}
}
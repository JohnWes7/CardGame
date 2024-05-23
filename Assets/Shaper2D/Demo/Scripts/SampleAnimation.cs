using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shaper2D
{
	public class SampleAnimation : MonoBehaviour
	{

		private Shaper2D _shaper2d;

		void Start()
		{
			//Just sets the window size of the windows application
			Screen.SetResolution(300, 300, false);
			//Getting the Shaper2D component
			_shaper2d = GetComponent<Shaper2D>();
			//Start the first animation
			//After it finishes it starts the next one and so on
			StartCoroutine(Appear());
		}

		//Expand the arc and rotate
		private IEnumerator Appear()
		{
			//Setting initial parameters
			_shaper2d.innerColor = new Color(0.9f, 1f, 0.5f);
			_shaper2d.outerColor = new Color(1f, 0.5f, 0.5f);
			_shaper2d.sectorCount = 35;
			_shaper2d.arcDegrees = 1;
			_shaper2d.rotation = 180;
			_shaper2d.outerRadius = 5;
			_shaper2d.innerRadius = 3;
			_shaper2d.starrines = 0;
			//Start animation
			while (_shaper2d.arcDegrees < 360)
			{
				//Increase arcDegrees and decrease rotation each frame
				_shaper2d.arcDegrees += 2;
				_shaper2d.rotation -= 1f;
				yield return new WaitForEndOfFrame();
			}

			_shaper2d.arcDegrees = 360;
			//Start the next animation
			StartCoroutine(RadiusChange());
		}

		//Animate inner radius
		private IEnumerator RadiusChange()
		{
			while (_shaper2d.innerRadius > 0)
			{
				//Decrease inner radius each frame
				_shaper2d.innerRadius -= 0.05f;
				yield return new WaitForEndOfFrame();
			}

			_shaper2d.innerRadius = 0f;
			//Start the next animation
			StartCoroutine(ColorChange());
		}

		//Animate color and starriness parameters
		private IEnumerator ColorChange()
		{
			Color inner = _shaper2d.innerColor;
			Color outer = _shaper2d.outerColor;
			for (float i = 0f; i <= 1; i += 0.02f)
			{
				_shaper2d.innerColor = Color.Lerp(inner, new Color(0f, 0.5f, 1f), i);
				_shaper2d.outerColor = Color.Lerp(outer, new Color(0.5f, 0.4f, 0.6f), i);
				if (i <= 0.5f) _shaper2d.starrines += 0.005f;
				if (i > 0.5f) _shaper2d.starrines -= 0.005f;
				yield return new WaitForEndOfFrame();
			}

			//Start the next animation
			StartCoroutine(AngleChange());
		}

		//Animate sector count, rotation and size
		private IEnumerator AngleChange()
		{
			for (int i = 0; i < 360; i++)
			{
				_shaper2d.rotation++;
				//Decrease sector count each 10th
				if (i % 10 == 0 && _shaper2d.sectorCount > 3)
				{
					_shaper2d.sectorCount--;
				}

				yield return new WaitForEndOfFrame();
			}

			//Start the next animation
			StartCoroutine(Disappear());
		}

		//Disappear like a pacman
		private IEnumerator Disappear()
		{
			for (int i = 360; i > 0; i--)
			{
				if (i % 10 == 0 && _shaper2d.sectorCount < 20) _shaper2d.sectorCount++;
				_shaper2d.arcDegrees -= 2;
				_shaper2d.rotation += 1f;
				//Stop when ar degrees reach zero
				if (_shaper2d.arcDegrees < 1) break;
				yield return new WaitForEndOfFrame();
			}

			//Start the first animation again
			StartCoroutine(Appear());
		}
	}
}

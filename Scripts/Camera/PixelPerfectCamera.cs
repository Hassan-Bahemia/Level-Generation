// Duo Chroma version 0.3.0
// Pixel Perfect Camera Script.
// 17/07/2018 19:45

using UnityEngine;

namespace Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class PixelPerfectCamera : MonoBehaviour
	{
		[SerializeField] private int _resolutionX;
		[SerializeField] private int _resolutionY;

		[SerializeField] private int _pixelWidth;

		private UnityEngine.Camera _targetCamera;

		private void Awake()
		{
			_targetCamera = GetComponent<UnityEngine.Camera>();
		}

		private void LateUpdate()
		{
			// Work out the largest possible dimensions that are multiples of the resolution.
			float largestWidth = Mathf.FloorToInt(Screen.width / (_resolutionX * _pixelWidth));
			float largestHeight = Mathf.FloorToInt(Screen.height / (_resolutionY * _pixelWidth));

			// Picks the smaller size to make sure it fits the resolution.
			if (largestHeight < largestWidth)
			{
				largestWidth = largestHeight;
			}
			else
			{
				largestHeight = largestWidth;
			}
			
			largestHeight *= _resolutionY * _pixelWidth;
			largestWidth *= _resolutionX * _pixelWidth;

			_targetCamera.orthographicSize = ((largestHeight / 2 / 100) / _pixelWidth);
        
			// Creates a new camera rectangle and modifies it to fit the desired resolution.
			var rectangle = _targetCamera.rect;
			rectangle.width = largestWidth / Screen.width;
			rectangle.height = largestHeight / Screen.height;
			rectangle.x = Mathf.Round((( 1 - rectangle.width) / 2) * 1000000) / 1000000;
			rectangle.y = Mathf.Round((( 1 - rectangle.height) / 2) * 1000000) / 1000000;
			_targetCamera.rect = rectangle;
		}
	}
}
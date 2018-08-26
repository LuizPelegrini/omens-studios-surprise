using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtil {

	/// <summary>
	/// Instead of calculating these value every time I need them, I rather chose to make a static class to calculate them once and for all
	/// </summary>
	private static float _halfScreenWidthInWorldUnits;
	private static float _halfScreenHeightInWorldUnits;

	private static float xmin; //(0,y) on screen
	private static float xmax; //(1,y) on screen
	private static float ymin; //(x,0) on screen
	private static float ymax; //(x,1) on screen

	public static float halfScreenWidthInWorldUnits
	{
		get
		{ 
			if (_halfScreenWidthInWorldUnits == 0f)
				_halfScreenWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize;

			return _halfScreenWidthInWorldUnits;
		}
	}

	public static float halfScreenHeightInWorldUnits
	{
		get
		{
			if (_halfScreenHeightInWorldUnits == 0f)
				_halfScreenHeightInWorldUnits = Camera.main.orthographicSize;

			return _halfScreenHeightInWorldUnits;
		}
	}

	public static float Xmin{
		get{
			if(xmin==0){
				xmin = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0)).x; 
			}
			return xmin;
		}
	}
	public static float Xmax{
		get{
			if(xmax==0){
				xmax = Camera.main.ViewportToWorldPoint (new Vector2 (1, 0)).x; 
			}
			return xmax;
		}
	}
		public static float Ymin{ 
		get{
			if(ymin==0){
				ymin = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0)).y; 
			}
			return ymin;
		}
	}
		public static float Ymax{ 
		get{
			if(ymax==0){
				ymax = Camera.main.ViewportToWorldPoint (new Vector2 (0, 1)).y; 
			}
			return ymax;
		}
	}
}

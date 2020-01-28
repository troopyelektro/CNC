/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 11.12.2019
 * Time: 13:00
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CNC.CADElements
{
	/// <summary>
	/// 2D Point
	/// </summary>
	public class Point2D : Element
	{
		/// <summary>
		/// Location X of point
		/// </summary>		
		private decimal x;
		
		/// <summary>
		/// Location X of point 
		/// </summary>
		public decimal X {
			get {
				return this.x;
			}
			set {
				this.x = value;
			}
		}
				
		/// <summary>
		/// Location Y of point
		/// </summary>
		private decimal y;

		/// <summary>
		/// Location Y of point
		/// </summary>
		public decimal Y {
			get {
				return this.y;
			}
			set {
				this.y = value;
			}
		}
				
		/// <summary>
		/// Instance of CAD 2D Drawing Point
		/// </summary>
		/// <param name="locX">Location X of point</param>
		/// <param name="locY">Location Y of point</param>
		public Point2D(decimal locX, decimal locY)
		{
			this.X = locX;
			this.Y = locY;
		}
		
		public override bool isMouseOnElement(CAD2DDrawingPanel p, MouseState ms)
		{
			Vector2 loc = p.pointToLocation(this);			
			if(loc.X >= (ms.X-this.mouseRange) &&
			   loc.X <= (ms.X+this.mouseRange) &&
			   loc.Y >= (ms.Y-this.mouseRange) &&
			   loc.Y <= (ms.Y+this.mouseRange)
			) {
				return true;
			} else {
				return false;
			}
		}				
	}
}

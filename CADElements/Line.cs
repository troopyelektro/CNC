using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CNC.CADElements
{
	public class Line : Element
	{
		public Point2D Point1;
		public Point2D Point2;
		private int mouseCut = 5;
		
		public Line(Point2D p1, Point2D p2)
		{
			this.Point1 = p1;
			this.Point2 = p2;
			this.mouseRange = 3;
		}
		
		public override bool isMouseOnElement(CAD2DDrawingPanel p, MouseState ms)
		{
			Vector2 loc1 = p.pointToLocation(this.Point1);
			Vector2 loc2 = p.pointToLocation(this.Point2);
			double X1 = loc1.X;
			double Y1 = loc1.Y;
			double X2 = loc2.X;
			double Y2 = loc2.Y;						


			// Get point distance
			double diffX = Math.Abs(X2 - X1);
			double diffY = Math.Abs(Y2 - Y1);
			int diff = (int)Math.Sqrt(Math.Pow(diffX,2) + Math.Pow(diffY,2));
		
			// Get offset
			int oX = (int)(X2 - X1);
			int oY = (int)(Y2 - Y1);
			
			// Scan Line
			for(int i=2*this.mouseRange; i<=diff-2*this.mouseRange; i++) {
				int x = (int)X1 + (int)((double)i/(double)diff*(double)oX);
				int y = (int)Y1 + (int)((double)i/(double)diff*(double)oY);
				if(x >= (ms.X-this.mouseRange) &&
				   x <= (ms.X+this.mouseRange) &&
				   y >= (ms.Y-this.mouseRange) &&
				   y <= (ms.Y+this.mouseRange)
				) {
					return true;
				}				
			}				
			return false;
		}
	}
}

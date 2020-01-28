using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CNC;

namespace CNC.CADElements
{
	public class Shape : Element
	{
		public List<Point2D> Points;
		
		public List<Line> Lines {
			get {
				List<Line> ll = new List<Line>();
				for(int l=0; l<this.Points.Count; l++) {
					Point2D p1 = this.Points[l];
					Point2D p2 = this.Points[((l+1)==this.Points.Count) ? 0 : l+1];
					ll.Add(new Line(p1, p2));
				}
				return ll;
			}
		}
		
		public Shape(List<Point2D> pointSequence)
		{
			this.Points = pointSequence;
			this.mouseRange = 5;
		}
		
		public override bool isMouseOnElement(CAD2DDrawingPanel p, MouseState ms)
		{
			foreach(Line l in this.Lines) {
				if(l.isMouseOnElement(p,ms)) {
					return true;
				}
			}
			return false;
		}		
	}
}

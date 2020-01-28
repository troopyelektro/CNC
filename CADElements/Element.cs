using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace CNC.CADElements
{
	public class Element
	{
		/// <summary>
		/// Determines, how many pixels from point center is point marked by mouse
		/// </summary>
		protected int mouseRange = 10;

		public Element()
		{
		}		

		public virtual bool isMouseOnElement(CAD2DDrawingPanel p, MouseState ms)
		{
			return false;
		}
		
		public bool isPushed(CAD2DDrawingPanel p)
		{
			if(p.isCanvasPushed() &&
			   this.isMouseOnElement(p, p.Mouse.CurrentState)
			) {
				return true;
			} else {
				return false;
			}
		}

		public bool isHovered(CAD2DDrawingPanel p)
		{
			if(p.isCanvasHovered() &&
			   this.isMouseOnElement(p, p.Mouse.CurrentState)
			) {
				return true;
			} else {
				return false;
			}
		}

		public bool isClicked(CAD2DDrawingPanel p)
		{
			if(p.isCanvasHovered() &&
			   this.isMouseOnElement(p, p.Mouse.LeftButtonPressed) &&
			   this.isMouseOnElement(p, p.Mouse.LeftButtonReleased) 
			) {
				return true;
			} else {
				return false;
			}
		}		
		
	}
}

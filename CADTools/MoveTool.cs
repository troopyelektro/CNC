using System;
using CNC.CADElements;
using System.Collections.Generic;

namespace CNC.CADTools
{
	public class MoveTool : Tool
	{
		private List<decimal> elementsInitX;
		private List<decimal> elementsInitY;
		
		public decimal InitX {
			get;
			private set;
		}
		public decimal InitY {
			get;
			private set;
		}
			
		private Element eLock;
		public bool Locked {
			get {
				if(this.eLock == null) {
					return false;
				} else {
					return true;
				}
			}
		}
		
		public MoveTool(Toolbox tb) : base(tb)
		{
			this.buttonTexture = tb.TextureToolMove;
			this.elementsInitX = new List<decimal>();
			this.elementsInitY = new List<decimal>();
		}
		
		public void lockElement(Element e)
		{
			this.eLock = e;
			if(this.eLock.GetType() == typeof(Point2D)) {
				Point2D ep = (Point2D)this.eLock;
				this.InitX = ep.X;
				this.InitY = ep.Y;
			}
			foreach(Element selEl in this.Toolbox.SelectTool.Elements) {
				if(selEl.GetType() == typeof(Point2D)) {
					Point2D selElPoint = (Point2D)selEl;
					this.elementsInitX.Add(selElPoint.X);
					this.elementsInitY.Add(selElPoint.Y);
				}
			}
		}
		
		public void move(decimal X, decimal Y)
		{
			if(this.Locked) {
				decimal diffX = X - this.InitX;
				decimal diffY = Y - this.InitY;
				for(int i=0; i<this.Toolbox.SelectTool.Elements.Count; i++) {
					Element selEl = this.Toolbox.SelectTool.Elements[i];
					if(selEl.GetType() == typeof(Point2D)) {
						Point2D selElPoint = (Point2D)selEl;
						selElPoint.X = elementsInitX[i] + diffX;
						selElPoint.Y = elementsInitY[i] + diffY;						
					}				
				}
			}
		}
		
		public void unlockElement()
		{
			this.eLock = null;
			this.elementsInitX.Clear();
			this.elementsInitY.Clear();
		}
	}
}

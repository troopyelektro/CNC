using System;
using System.Collections.Generic;
using CNC.CADElements;
using CNC;

namespace CNC.CADTools
{
	public class SelectTool : Tool
	{
		private List<Element> elements;
		public List<Element> Elements {
			get {
				return this.elements;
			}
		}
		
		public SelectTool(Toolbox tb) : base(tb)
		{
			this.elements = new List<Element>();
			this.buttonTexture = tb.TextureToolSelect;
		}
		
		public void toggleSelection(Element e)
		{
			int index = this.Toolbox.SelectTool.Elements.IndexOf(e);
			if(index == -1) {
				// Add to selection
				this.Elements.Add(e);
			} else {
				// Remove from selection
				this.Elements.RemoveAt(index);
			}
		}
	}
}

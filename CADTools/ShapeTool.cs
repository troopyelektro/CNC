using System;
using CNC;

namespace CNC.CADTools
{
	public class ShapeTool : Tool
	{
		public ShapeTool(Toolbox tb) : base(tb)
		{
			this.buttonTexture = tb.TextureToolShape;
		}
	}
}
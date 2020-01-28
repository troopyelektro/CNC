using System;

namespace CNC.CADTools
{
	public class PointTool : Tool
	{
		public PointTool(Toolbox tb) : base(tb)
		{
			this.buttonTexture = tb.TextureToolPoint;
		}
	}
}
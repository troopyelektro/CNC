using System;

namespace CNC.CADTools
{
	public class DeleteTool : Tool
	{
		public DeleteTool(Toolbox tb) : base(tb)
		{
			this.buttonTexture = tb.TextureToolDelete;
		}
	}
}
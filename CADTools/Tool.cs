/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 15.12.2019
 * Time: 14:06
 */
using System;
using Microsoft.Xna.Framework.Graphics;
using CNC;

namespace CNC.CADTools
{
	abstract public class Tool
	{
		protected Toolbox toolbox;
		public Toolbox Toolbox {
			get {
				return this.toolbox;
			}
		}
		
		public bool Available = true;
		
		public Texture2D buttonTexture;
		
		public Tool(Toolbox tb)
		{
			this.toolbox = tb;
		}
	}
}

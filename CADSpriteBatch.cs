/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 08.12.2019
 * Time: 9:40
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Troopy;
using Troopy.Xna.Controls;

namespace CNC
{
	/// <summary>
	/// Sprite Batch for CAD
	/// </summary>
	public class CADSpriteBatch : Troopy.Xna.SpriteBatch
	{
		private CADContentFactory CADContent {
			get {
				return (CADContentFactory)this.content;
			}
		}
		
		/// <summary>
		/// Instance of SpriteBatch optimised for CAD capability
		/// </summary>
		/// <param name="graphics"></param>
		public CADSpriteBatch(GraphicsDevice graphics, CADContentFactory cadContent) : base(graphics, cadContent)
		{
		}

		/// <summary>
		/// Draws animated crosshair cursor on mouse location
		/// </summary>
		public void drawCrosshair()
		{
			this.Draw(
            	this.CADContent.imgCursorCrosshair,
            	new Vector2(mouse.X, mouse.Y),
            	null,
            	null,
            	new Vector2(7,7),
            	0,
            	null,
            	Color.White,
            	SpriteEffects.None,
            	this.LayerDepth
            );		
		}
	
	}
}

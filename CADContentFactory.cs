/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 08.12.2019
 * Time: 9:25
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Troopy;

namespace CNC
{
	/// <summary>
	/// Storage for CAD Content
	/// </summary>
	public class CADContentFactory : Troopy.Xna.ContentFactory
	{	
		// Cursors
		public Texture2D imgCursorCrosshair;
		
		// Buttons
		public Texture2D imgButtonArrowOn;
		public Texture2D imgButtonArrowOff;
		public Texture2D imgButtonArrowHover;
		public Texture2D imgButtonArrowDisabled;
		
		// ScrollBars
		public Texture2D scrollArrowOn;
		public Texture2D scrollArrowOff;
		public Texture2D scrollArrowHover;
		public Texture2D scrollArrowDisabled;
		public Texture2D scrollRangeOn;
		public Texture2D scrollRangeDisabled;
		public Texture2D scrollBarEdgeOn;
		public Texture2D scrollBarEdgeOff;
		public Texture2D scrollBarEdgeHover;
		public Texture2D scrollBarCenterOn;
		public Texture2D scrollBarCenterOff;
		public Texture2D scrollBarCenterHover;	
	
		// Machine
		public Texture2D machineTool;
		public Texture2D machineController;
		
		// Drawing
		public Texture2D imgToolsSelect;
		public Texture2D imgToolsMove;
		public Texture2D imgToolsDelete;
		public Texture2D imgToolsPoint;
		public Texture2D imgToolsShape;
		
		public Texture2D imgPoint;
			
		// Fonts
		public SpriteFont fontConsolas12;
				
		/// <summary>
		/// Instance of Content Factory
		/// </summary>
		/// <param name="cm"></param>
		public CADContentFactory(ContentManager cm) : base(cm)
		{		
			// Load Images
            imgCursorCrosshair = this.contentManager.Load<Texture2D>(@"Images\Cursor");
 
            imgButtonArrowOn = this.contentManager.Load<Texture2D>(@"Images\Arrow_ON");
            imgButtonArrowOff = this.contentManager.Load<Texture2D>(@"Images\Arrow_OFF");
            imgButtonArrowHover = this.contentManager.Load<Texture2D>(@"Images\Arrow_HOVER");
            imgButtonArrowDisabled = this.contentManager.Load<Texture2D>(@"Images\Arrow_DISABLED");
            
            scrollArrowOn = this.contentManager.Load<Texture2D>(@"Images\scrollArrowOn");
			scrollArrowOff = this.contentManager.Load<Texture2D>(@"Images\scrollArrowOff");
			scrollArrowHover = this.contentManager.Load<Texture2D>(@"Images\scrollArrowHover");
			scrollArrowDisabled = this.contentManager.Load<Texture2D>(@"Images\scrollArrowDisabled");
			scrollRangeOn = this.contentManager.Load<Texture2D>(@"Images\scrollRangeOn");
			scrollRangeDisabled = this.contentManager.Load<Texture2D>(@"Images\scrollRangeDisabled");
			scrollBarEdgeOn = this.contentManager.Load<Texture2D>(@"Images\scrollBarEdgeOn");
			scrollBarEdgeOff = this.contentManager.Load<Texture2D>(@"Images\scrollBarEdgeOff");
			scrollBarEdgeHover = this.contentManager.Load<Texture2D>(@"Images\scrollBarEdgeHover");
			scrollBarCenterOn = this.contentManager.Load<Texture2D>(@"Images\scrollBarCenterOn");
			scrollBarCenterOff = this.contentManager.Load<Texture2D>(@"Images\scrollBarCenterOff");
			scrollBarCenterHover = this.contentManager.Load<Texture2D>(@"Images\scrollBarCenterHover");
			
			imgPoint = this.contentManager.Load<Texture2D>(@"Images\Point");
			
			imgToolsDelete = this.contentManager.Load<Texture2D>(@"Images\btnToolsDelete");
			imgToolsMove = this.contentManager.Load<Texture2D>(@"Images\btnToolsMove");
			imgToolsPoint = this.contentManager.Load<Texture2D>(@"Images\btnToolsPoint");
			imgToolsSelect = this.contentManager.Load<Texture2D>(@"Images\btnToolsSelect");
			imgToolsShape = this.contentManager.Load<Texture2D>(@"Images\btnToolsShape");
			
			
			machineTool = this.contentManager.Load<Texture2D>(@"Images\MachineTool");
			machineController = this.contentManager.Load<Texture2D>(@"Images\TextureMachineController");
			
            // Load Font
            fontConsolas12 = this.contentManager.Load<SpriteFont>(@"Fonts\Consolas");
		}
		
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>		
		public void unload()
		{
			// TODO: Unload any non ContentManager content here
		}
	}
}

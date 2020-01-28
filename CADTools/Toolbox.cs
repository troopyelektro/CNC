using System;
using Troopy.Xna.Controls;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CNC.CADTools
{
	public class Toolbox : Control
	{
		private SelectTool toolSelect;
		private MoveTool toolMove;
		private DeleteTool toolDelete;
		private PointTool toolPoint;
		private ShapeTool toolShape;

		public Texture2D TextureToolSelect;
		public Texture2D TextureToolMove;
		public Texture2D TextureToolDelete;
		public Texture2D TextureToolPoint;
		public Texture2D TextureToolShape;
		
		public int ButtonBorder = 3;
		public Color BorderColorSelected = Color.Green;
		public Color BorderColorHover = Color.LightBlue;
		public Color BorderColorDisabled = Color.Gray;
		public Color BorderColorAvailable = Color.Black;
		
		private float LdButtonBorder {get{return this.LayerBase + 0.002f;}}
		private float LdButtonButton {get{return this.LayerBase + 0.002f;}}
		
		private List<Tool> tools;
		public List<Tool> Tools {
			get {
				return this.tools;
			}
		}
		
		private Tool selectedTool;
		public Tool SelectedTool {
			get {
				return this.selectedTool;
			}
			set {
				if (value.Available) {
					this.selectedTool = value;									
				} else {
					this.selectedTool = this.SelectTool;
				}
			}
		}
		
		public SelectTool SelectTool
		{
			get {
				return (SelectTool)this.getTool(typeof(SelectTool));
			}
		}
		
		public MoveTool MoveTool
		{
			get {
				return (MoveTool)this.getTool(typeof(MoveTool));
			}
		}		
		
		public DeleteTool DeleteTool
		{
			get {
				return (DeleteTool)this.getTool(typeof(DeleteTool));
			}
		}				
		
		public PointTool PointTool
		{
			get {
				return (PointTool)this.getTool(typeof(PointTool));
			}
		}						
		
		public ShapeTool ShapeTool
		{
			get {		
				return (ShapeTool)this.getTool(typeof(ShapeTool));
			}
		}			
		
		public Toolbox()
		{
			this.tools = new List<Tool>();
		}
		
		public void createTools() {
			this.tools.Add(new SelectTool(this));
			this.tools.Add(new MoveTool(this));
			this.tools.Add(new DeleteTool(this));
			this.tools.Add(new PointTool(this));
			this.tools.Add(new ShapeTool(this));
			this.selectedTool = this.tools[0];
			this.Width =  30 + 2*this.ButtonBorder;
			this.Height = this.Width * this.tools.Count;;
		}
		
		public void update()
		{
			for(int i = 0; i<this.Tools.Count; i++) {
				Tool t = this.Tools[i];
				if(t.Available && this.isToolClicked(i)) {
					this.SelectedTool = t;
				}
			}			
		}
		
		public void draw(Troopy.Xna.SpriteBatch spriteBatch)
		{
			for(int i = 0; i<this.Tools.Count; i++) {
				Tool t = this.Tools[i];
				Color bc;
				if(this.selectedTool != null && this.selectedTool.GetType() == t.GetType()) {
					bc = this.BorderColorSelected;
				} else {
					if(!t.Available) {
						bc = this.BorderColorDisabled;
					} else if(this.isToolHovered(i)) {
						bc = this.BorderColorHover;
					} else {
						bc = this.BorderColorAvailable;
					}
				}
				
				// Draw button border
				spriteBatch.LayerDepth = this.LdButtonBorder;
				spriteBatch.drawRectangle(
					this.Left,
					this.Top + (i * this.Width),
					this.Width,
					this.Width,
					bc
				);
				
				// Draw Button
				spriteBatch.LayerDepth = this.LdButtonButton;
				spriteBatch.Draw(
					t.buttonTexture,
					new Vector2(this.Left + this.ButtonBorder, this.Top + (i * this.Width) + this.ButtonBorder),
					null ,null ,null ,0f , null, Color.White, SpriteEffects.None, spriteBatch.LayerDepth
				);
					
			}
		}
		
		private bool isToolHovered(int index)
		{
			if(this.isHovered() &&
			   this.isMouseOnTool(this.Mouse.CurrentState, index)
			) {
				return true;
			} else {
				return false;
			}
		}
		
		private bool isToolClicked(int index)
		{
			if(this.isClicked() &&
			   this.isMouseOnTool(this.Mouse.LeftButtonPressed, index) &&
			   this.isMouseOnTool(this.Mouse.LeftButtonReleased, index)
			) {
				return true;
			} else {
				return false;
			}
		}

		private bool isMouseOnTool(MouseState ms, int index)
		{
			int yMin = this.Top + index * this.Width;
			int yMax = yMin + this.Width - 1;
			if(ms.Y >= yMin && ms.Y <= yMax) {
				return true;
			} else {
				return false;
			}			
		}
		
		public void selectTool(Tool tool)
		{
			foreach(Tool t in this.tools) {
				if(t.GetType() == tool.GetType()) {
					this.selectedTool = t;
				}
			}
		}
		
		private Tool getTool(Type tool)
		{
			foreach(Tool t in this.tools) {
				if(t.GetType() == tool) {
					return t;
				}
			}		
			return null;
		}
		
							
	}
}

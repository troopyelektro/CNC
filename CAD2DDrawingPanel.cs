/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 07.12.2019
 * Time: 11:45
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Troopy;
using Troopy.Xna.Controls;
using System.Collections.Generic;
using CNC.CADTools;
using CNC.CADFiles;
using CNC.CADElements;

namespace CNC
{
	/// <summary>
	/// Instance for 2D Drawing CAD Panel
	/// </summary>
	public class CAD2DDrawingPanel : CADPanel
	{			
		//
		// DOCUMENT WIDTH
		// 		
		
		/// <summary>
		/// Document size in Milimeters, width
		/// </summary>
		private decimal docSizeMmX;		
		/// <summary>
		/// Document size in Milimeters, width
		/// </summary>
		public decimal DocSizeMmX {
			get {
				return this.docSizeMmX;
			}
			set {
				this.docSizeMmX = value;
				this.resizeCanvas();
			}
		}

		//
		// DOCUMENT HEIGHT
		// 
		
		/// <summary>
		/// Document size in Milimeters, height
		/// </summary>
		private decimal docSizeMmY;
		/// <summary>
		/// Document size in Milimeters, height
		/// </summary>
		public decimal DocSizeMmY {
			get {
				return this.docSizeMmY;
			}
			set {
				this.docSizeMmY = value;
				this.resizeCanvas();
			}
		}
		
		//
		// DOCUMENT ZOOM
		//
		
		/// <summary>
		/// View Ratio Dots per Milimeter
		/// </summary>
		private decimal ratioDotsPerMm;				
		/// <summary>
		/// View Ratio Dots per Milimeter
		/// </summary>
		private decimal RatioDotsPerMm {
			get {
				return this.ratioDotsPerMm;
			}
			set {
				if(value>0) {
					this.ratioDotsPerMm = value;
					this.resizeCanvas();
				}
			}
		}

		/// <summary>
		/// Size of scrollbar (Arrows are squares of this size)
		/// </summary>
		private int scrollSize;
		public int ScrollSize {
			get {
				return this.scrollSize;
			}
			set {
				this.scrollSize = value;
				this.resizeCanvas();
			}
		}								
		
		//
		// PANEL DIMENSIONS
		//
		
		/// <summary>
		/// Width of CAD Panel
		/// </summary>
		public new int Width {
			get {
				return this.width;
			}
			set {
				this.width = value;
				this.resizeCanvas();
			}
		}
		
		/// <summary>
		/// Height of CAD Panel
		/// </summary>
		public new int Height {
			get {
				return this.height;
			}
			set {
				this.height = value;
				this.resizeCanvas();
			}
		}		
				
		//
		// INTERNAL DIMENSIONS
		//
		
		/// <summary>
		/// Determines, from which pixel number is top left corner View valid, Axis X
		/// </summary>
		public int DocViewXOffset {
			get {
				// Offsets
				int ofsetX = 0;			
				if(this.HorizontalScrollVisible) {
					ofsetX = new Conversion(((float)this.DocSizeDotsX - (float)this.CanvasAreaWidth) * this.scrollBarHorizontal.Value).getInt();
				}
				return ofsetX;
			}
		}
		
		/// <summary>
		/// Determines, from which pixel number is top left corner View valid, Axis X
		/// </summary>
		public int DocViewYOffset {
			get {
				int ofsetY = 0;
				if(this.VerticalScrollVisible) {
					ofsetY = new Conversion(((float)this.DocSizeDotsY - (float)this.CanvasAreaHeight) * this.scrollBarVertical.Value).getInt();
				}
				return ofsetY;
			}			
		}
		
		/// <summary>
		/// Size of document in dots, Axis X
		/// </summary>
		private int docSizeDotsX;
		/// <summary>
		/// Size of document in dots, Axis X
		/// </summary>
		public int DocSizeDotsX {
			get {
				return this.docSizeDotsX;
			}
		}

		/// <summary>
		/// Size of document in dots, Axis Y
		/// </summary>		
		private int docSizeDotsY;		
		/// <summary>
		/// Size of document in dots, Axis Y
		/// </summary>		
		public int DocSizeDotsY {
			get {
				return this.docSizeDotsY;
			}
		}
		
		//
		// Scroll Bar
		//		
		
		/// <summary>
		/// Indicates if horizontal scrollbar is visible
		/// </summary>
		private bool horizontalScrollVisible;
		/// <summary>
		/// Indicates if horizontal scrollbar is visible
		/// </summary>
		private bool HorizontalScrollVisible {
			get {
				return this.horizontalScrollVisible;
			}
			set {				
				if(!value) {					
					this.scrollBarHorizontal = null;
				}
				this.horizontalScrollVisible = value;
			}
		}

		/// <summary>
		/// Indicates if vertical scrollbar is visible 
		/// </summary>
		private bool verticalScrollVisible;
		/// <summary>
		/// Indicates if vertical scrollbar is visible 
		/// </summary>
		private bool VerticalScrollVisible {
			get {
				return this.verticalScrollVisible;
			}
			set {				
				if(!value) {
					this.scrollBarVertical = null;
				}				
				this.verticalScrollVisible = value;
			}			
		}
		
		/// <summary>
		/// Horizontal Scroll Bar
		/// </summary>
		private ScrollBar scrollBarHorizontal;		
		/// <summary>
		/// Vertical Scroll Bar
		/// </summary>
		private ScrollBar scrollBarVertical;
		
		// ScrollBar Textures
		public Texture2D TextureScrollArrowOn;
		public Texture2D TextureScrollArrowOff;
		public Texture2D TextureScrollArrowHover;
		public Texture2D TextureScrollArrowDisabled;
		public Texture2D TextureScrollRangeOn;
		public Texture2D TextureScrollRangeDisabled;
		public Texture2D TextureScrollBarEdgeOn;
		public Texture2D TextureScrollBarEdgeOff;
		public Texture2D TextureScrollBarEdgeHover;
		public Texture2D TextureScrollBarCenterOn;
		public Texture2D TextureScrollBarCenterOff;
		public Texture2D TextureScrollBarCenterHover;					

		// Fonts
		public SpriteFont FontCanvas;
		
		// Drawing elements
		public Texture2D TexturePoint;
		public Texture2D TextureMachineTool;
		
		//
		// Canvas dimension
		//		
		
		/// <summary>
		/// Panel width - ScrollBar if present
		/// </summary>		
		private int canvasAreaWidth;
		/// <summary>
		/// Panel width - ScrollBar if present
		/// </summary>
		public int CanvasAreaWidth {
			get {
				return this.canvasAreaWidth;
			}
		}

		/// <summary>
		/// Panel height - ScrollBar if present 
		/// </summary>
		private int canvasAreaHeight;
		/// <summary>
		/// Panel height - ScrollBar if present 
		/// </summary>
		public int CanvasAreaHeight {
			get {
				return this.canvasAreaHeight;
			}		
		}

		//
		// Grid ON
		//				
		
		/// <summary>
		/// Determines if Grid is on
		/// </summary>
		private bool gridOn;
		
		/// <summary>
		/// Determines if Grid is on
		/// </summary>
		public bool GridOn {
			get {
				return this.gridOn;
			}
			set {
				this.gridOn = value;
			}
		}
		
		/// <summary>
		/// How many Milimeters is between two neighbour lines
		/// </summary>
		private decimal gridStepMm = 10;

		/// <summary>
		/// How many Milimeters is between two neighbour lines
		/// </summary>
		private decimal GridStepMm {
			get {
				return this.gridStepMm;
			}
			set {
				if(value > 0) {
					this.gridStepMm = value;
				}				
			}
		}
		
		/// <summary>
		/// Zoom level
		/// </summary>
		private int zoom;

		/// <summary>
		/// Zoom level
		/// </summary>
		public int Zoom {
			get {
				return this.zoom;
			}
			set {
				this.zoom = value;
				if(value < 1) {
					this.zoom = 1;
				}
				if(value > 3) {
					this.zoom = 3;
				}
				
				switch(this.zoom) {
					case 1:
						this.RatioDotsPerMm = 1000;
						this.GridStepMm = 0.01m;
						break;
					case 2:
						this.RatioDotsPerMm = 100;
						this.GridStepMm = 0.1m;
						break;
					case 3:
						this.RatioDotsPerMm = 10;
						this.GridStepMm = 1m;
						break;
					case 4:
						this.RatioDotsPerMm = 1;
						this.GridStepMm = 0.01m;
						break;
					case 5:
						this.RatioDotsPerMm = 200;
						this.GridStepMm = 0.2m;
						break;
					case 6:
						this.RatioDotsPerMm = 100;
						this.GridStepMm = 0.1m;
						break;
					case 7:
						this.RatioDotsPerMm = 80;
						this.GridStepMm = 0.08m;
						break;
					case 8:
						this.RatioDotsPerMm = 60;
						this.GridStepMm = 0.06m;
						break;
					case 9:
						this.RatioDotsPerMm = 40;
						this.GridStepMm = 0.04m;
						break;
					case 10:
						this.RatioDotsPerMm = 20;
						this.GridStepMm = 0.02m;
						break;						
				}
				
			}
		}
		
		//
		// File
		//
		private Drawing2D file;
		public Drawing2D File {
			get {
				return this.file;
			}
			set {
				this.file = value;
			}
		}
		
		//
		// Machine
		//
		private Machine machine;
		public Machine Machine {
			get {
				return this.machine;
			}
			set {
				this.machine = value;
			}
		}
		
		//
		// Toolbox
		//
		private Toolbox toolbox;
		public Toolbox Toolbox {
			get {
				return this.toolbox;
			}
			set {
				this.toolbox = value;
			}
		}
		
		//
		// Layer Depth
		//
		private float ldBackRectangle {get {return this.LayerBase + 0.099f;}}
		private float ldGrid {get {return this.LayerBase + 0.098f;}}	
		private float ldContentShapes {get {return this.LayerBase + 0.097f;}}
		private float ldContentPoints {get {return this.LayerBase + 0.096f;}}
		private float ldMachinePosition {get {return this.LayerBase + 0.095f;}}
		private float ldCursor {get {return this.LayerBase + 0.094f;}}
		private float ldScrollBars {get {return this.LayerBase + 0.001f;}}
		
		/// <summary>
		/// Creates instance of CAD Panel for 2D Drawing
		/// </summary>
		/// <param name="panel"></param>
		public CAD2DDrawingPanel() : base()
		{						
			// Init File
			this.File = new Drawing2D();
			this.DocSizeMmX = this.File.DocSizeMmX;
			this.DocSizeMmY = this.File.DocSizeMmY;
			
			// Turn on Grid by default
			this.GridOn = true;
			this.GridStepMm = 1;
			
			// Init Panel Dimensions
			this.width = 100;
			this.height = 100;
			this.Left = 0;
			this.Top = 0;
			
			// Init Document Size
			this.Zoom = 5;
			
			// Force Panel Resize
			this.resizeCanvas();
		}
		
		/// <summary>
		/// Resize canvas according to document size and zoom
		/// </summary>
		private void resizeCanvas()
		{
			this.docSizeDotsX = (int)(this.docSizeMmX * this.ratioDotsPerMm) + 1;
			this.docSizeDotsY = (int)(this.docSizeMmY * this.ratioDotsPerMm) + 1;
			
			bool sbX = (this.DocSizeDotsX > this.Width);
			bool sbY = (this.DocSizeDotsY > this.Height);
			bool sbXs = (this.DocSizeDotsX > (this.Width - this.ScrollSize));
			bool sbYs = (this.DocSizeDotsY > (this.Height - this.ScrollSize));
			
			bool sbXe;
			bool sbYe;
			
			if(!sbX && !sbY) {
				sbXe = false;
				sbYe = false;				
			} else if(sbX && !sbYs) { // Only Horizontal needed, enough place for height
				sbXe = true;
				sbYe = false;
			} else if(sbY && !sbXs) { // Only Vertical needed, enough place for width
				sbXe = false;
				sbYe = true;
			} else {
				sbXe = true;
				sbYe = true;							
			}
			
			// Calculate Vertical ScrollBas
			if(sbYe) {
				this.VerticalScrollVisible = true;
				this.canvasAreaWidth = this.Width - this.ScrollSize;
			} else {
				this.VerticalScrollVisible = false;
				this.canvasAreaWidth = this.Width;				
			}
			
			// Calculate Horizontal ScrollBar
			if(sbXe) {
				this.HorizontalScrollVisible = true;
				this.canvasAreaHeight = this.Height - this.ScrollSize;
			} else {
				this.HorizontalScrollVisible = false;
				this.canvasAreaHeight = this.Height;
			}
		}		

		/// <summary>
		/// Provides Color for Grid
		/// </summary>
		/// <returns>Grid Color</returns>
		protected Color getGridColor()
		{
			return Color.DarkBlue;
		}
		
		/// <summary>
		/// Determines, if mouse cursor is on Canvas
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public bool isMouseOnCanvas(MouseState ms)
		{
			if((ms.X >= this.Left) &&
			   (ms.X <= (this.Left + this.CanvasAreaWidth - 1)) &&
			   (ms.Y >= this.Top) &&
			   (ms.Y <= (this.Top + this.CanvasAreaHeight - 1))
			) {
				return true;
			} else {
				return false;
			}		
		}

		/// <summary>
		/// Determines, if Canvas is hovered
		/// </summary>
		/// <returns></returns>
		public bool isCanvasHovered()
		{
			if(this.isHovered() &&
			   this.isMouseOnCanvas(this.Mouse.CurrentState)
			) {
				return true;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// Determines, if Canvas is pushed
		/// </summary>
		/// <returns></returns>
		public bool isCanvasPushed()
		{
			if(this.isPushed() &&
			   this.isMouseOnCanvas(this.Mouse.LeftButtonPressed)
			) {
				return true;
			} else {
				return false;
			}			
		}
		
		/// <summary>
		/// Determines, if Canvas is Clicked
		/// </summary>
		/// <returns></returns>		
		public bool isCanvasClicked()
		{
			if(isClicked() &&
			   this.isMouseOnCanvas(this.Mouse.LeftButtonPressed) &&
			   this.isMouseOnCanvas(this.Mouse.LeftButtonReleased)			
			) {
				return true;
			} else {
				return false;
			}
		}		
		
		/// <summary>
		/// Updates CAD 2D Drawing Panel
		/// </summary>
		public void update()
		{
			// Zoom
			int mouseWheelDiff = this.Mouse.CurrentState.ScrollWheelValue - this.Mouse.PreviousState.ScrollWheelValue;
			if(this.isHovered() && mouseWheelDiff != 0) {
				if(mouseWheelDiff > 0) {
					this.Zoom++;
				} else {
					this.Zoom--;
				}							
			}
			
			// Updates Scroll Bars
			if(this.VerticalScrollVisible) {
				// Create Scroll Bar if not exists
				if(this.scrollBarVertical == null) {
					this.scrollBarVertical = new ScrollBar();
					this.scrollBarVertical.Vertical = true;
					this.scrollBarVertical.Top = this.Top;
					this.scrollBarVertical.Left = this.Left + this.Width - this.ScrollSize;
					this.scrollBarVertical.Width = this.ScrollSize;
					this.scrollBarVertical.Height = this.Height - this.ScrollSize;					
					this.scrollBarVertical.Value = (float)this.DocViewYOffset / (float)(this.DocSizeDotsY - this.canvasAreaHeight);
					this.textureScrollBar(this.scrollBarVertical);
					this.scrollBarVertical.Enabled = true;
				}
				this.scrollBarVertical.Range = (float)this.CanvasAreaHeight / (float)this.DocSizeDotsY;
				this.scrollBarVertical.update();
			} else {
				this.scrollBarVertical = null;
			}
			
			// - Horizontal Scroll Bar
			if(this.HorizontalScrollVisible) {
				// Create Scroll Bar if not exists
				if(this.scrollBarHorizontal == null) {
					this.scrollBarHorizontal = new ScrollBar();
					this.scrollBarHorizontal.Horizontal = true;
					this.scrollBarHorizontal.Top = this.Top + this.Height - this.ScrollSize;
					this.scrollBarHorizontal.Left = this.Left;
					this.scrollBarHorizontal.Width = this.Width - this.ScrollSize;
					this.scrollBarHorizontal.Height = this.ScrollSize;					
					this.scrollBarHorizontal.Value = (float)this.DocViewXOffset / (float)(this.DocSizeDotsX - this.canvasAreaWidth);
					this.textureScrollBar(this.scrollBarHorizontal);
					this.scrollBarHorizontal.Enabled = true;
				}
				this.scrollBarHorizontal.Range = (float)this.CanvasAreaWidth / (float)this.DocSizeDotsX;
				this.scrollBarHorizontal.update();
			} else {
				scrollBarHorizontal = null;
			}
			
			//
			// Toolbox
			//
			
			// Precalculate
			this.Toolbox.update();
								
			
			
			// SelectTool
			if(this.Toolbox.SelectedTool == this.Toolbox.SelectTool) {
				// Init
				this.Toolbox.SelectTool.Available = true;
				this.Toolbox.MoveTool.Available = false;
				this.Toolbox.DeleteTool.Available = false;
				this.Toolbox.PointTool.Available = true;
				this.Toolbox.ShapeTool.Available = false;					
				int sc = this.Toolbox.SelectTool.Elements.Count;
				if(sc > 0) {
					this.Toolbox.MoveTool.Available = true;
					this.Toolbox.DeleteTool.Available = true;								
					if(sc > 2) {
						this.Toolbox.ShapeTool.Available = true;
					}
				}
				
				
				// Select/unselect point
				if(this.isCanvasClicked()) {
				   	Element eToggle = null;
				   	// Search clicked element
				   	foreach(Element e in this.File.Elements) {
				   		if(e.isClicked(this)) {
				   			eToggle = e;
				   			break;
				   		}
				   	}
				   	// Toggle
				   	if(eToggle != null) {
				   		this.Toolbox.SelectTool.toggleSelection(eToggle);
				   	}
				}				
			}
			
			// Point 
			if(this.Toolbox.SelectedTool == this.Toolbox.PointTool) {
				// Init
				this.Toolbox.SelectTool.Available = true;
				this.Toolbox.MoveTool.Available = false;
				this.Toolbox.DeleteTool.Available = false;
				this.Toolbox.PointTool.Available = true;
				this.Toolbox.ShapeTool.Available = false;	

				// Add point
				if(this.isCanvasClicked()) {
					Point2D p = this.mouseCursorToPoint(this.Mouse.CurrentState);
					Point2D sp = this.getNearestSnapPoint(p);
					this.File.Elements.Add(sp);
				}				
			}
			
			// Move Tool
			if(this.Toolbox.SelectedTool == this.Toolbox.MoveTool) {
				// Init
				this.Toolbox.SelectTool.Available = true;
				this.Toolbox.MoveTool.Available = false;
				this.Toolbox.DeleteTool.Available = false;
				this.Toolbox.PointTool.Available = true;
				this.Toolbox.ShapeTool.Available = false;	

				// Select/unselect point
				if(this.isCanvasPushed()) {
					if(!this.Toolbox.MoveTool.Locked) {
						// Try to find pushed element
						Element ePush = null;
					   	foreach(Element e in this.Toolbox.SelectTool.Elements) {
					   		if(e.isPushed(this)) {
					   			ePush = e;
					   			break;
					   		}
					   	}
					   	// Lock
					   	if(ePush != null) {
					   		this.Toolbox.MoveTool.lockElement(ePush);
					   	}
					} else {
						// Locked and pushed
						Point2D target = this.getNearestSnapPoint(this.mouseCursorToPoint(this.Mouse.CurrentState));
						this.Toolbox.MoveTool.move(target.X, target.Y);							
					}
				} else {
					this.Toolbox.MoveTool.unlockElement();
				}
			}			
			
			// Delete tool
			if(this.Toolbox.SelectedTool == this.Toolbox.DeleteTool) {
				foreach(Element e in this.Toolbox.SelectTool.Elements) {
					// Delete points
					if(e.GetType() == typeof(Shape)) {
						this.File.deleteShape((Shape)e);
					}
					if(e.GetType() == typeof(Point2D)) {
						this.File.deletePoint((Point2D)e);
					}										
				}
				this.Toolbox.SelectTool.Elements.Clear();
				this.Toolbox.SelectedTool = this.Toolbox.SelectTool;
			}	

			// Shape tool
			if(this.Toolbox.SelectedTool == this.Toolbox.ShapeTool) {
				
				List<Point2D> p = new List<Point2D>();
				foreach(Element e in this.Toolbox.SelectTool.Elements) {
					if(e.GetType() == typeof(Point2D)) {
						p.Add((Point2D)e);
					}
				}								
				Shape s = new Shape(p);
				this.File.Elements.Add(s);					
				this.Toolbox.SelectTool.Elements.Clear();
				this.Toolbox.SelectedTool = this.Toolbox.SelectTool;
			}				
		}
				
		/// <summary>
		/// Draws CAD 2D Drawing Panel
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void draw(CADSpriteBatch spriteBatch)
		{
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Background Color
			//
			spriteBatch.LayerDepth = this.ldBackRectangle;
			spriteBatch.drawRectangle(this.Left, this.Top, this.Width, this.Height, this.BackgroundColor);						
			
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Grid
			//
			spriteBatch.LayerDepth = this.ldGrid;
			
			// Vertical
			decimal x = 0;
			while(x <= DocSizeMmX) {
				int xd = (int)(x * this.RatioDotsPerMm);
				spriteBatch.drawLine(   
					this.Left + xd - this.DocViewXOffset,
					this.Top - this.DocViewYOffset,
					this.DocSizeDotsY,
					this.getGridColor(),
					true
				);
				x = x + this.gridStepMm;
			}
			
			// Horizontal
			decimal y = 0;
			while(y <= DocSizeMmY) {
				int yd = (int)(y * this.RatioDotsPerMm);
				spriteBatch.drawLine(
					this.Left - this.DocViewXOffset,
				    this.Top + yd - this.DocViewYOffset,
				    this.DocSizeDotsX,
				    this.getGridColor()
				);
				y = y + this.gridStepMm;
			}
			
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Document Content
			//			
			
			// Draw Shapes
			spriteBatch.LayerDepth = this.ldContentShapes;
			foreach(Shape p in this.File.Shapes) {
				if(this.Toolbox.SelectTool.Elements.Contains(p)) {
					// Selected Point
					this.drawShape(spriteBatch, p, Color.Red);
				} else {
					this.drawShape(spriteBatch, p, Color.LightGreen);
				}				
			}			
			
			// Draw Points
			spriteBatch.LayerDepth = this.ldContentPoints;
			foreach(Point2D p in this.File.Points) {
				if(this.Toolbox.SelectTool.Elements.Contains(p)) {
					// Selected Point
					this.drawPoint(spriteBatch, p, Color.Red);
					spriteBatch.DrawString(
						this.FontCanvas,
						(this.Toolbox.SelectTool.Elements.IndexOf(p)+1).ToString(),
						this.pointToLocation(p) + new Vector2(5,5),
						Color.Red
					);					
				} else {
					this.drawPoint(spriteBatch, p, Color.White);
				}				
			}
			
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Machine Location
			//
			spriteBatch.LayerDepth = this.ldMachinePosition;
			if(this.Machine != null && this.TextureMachineTool != null) {
				Vector2 spv = this.pointToLocation(this.Machine.Position);
				spriteBatch.Draw(
					this.TextureMachineTool,
					new Vector2(spv.X - 12, spv.Y - 12),
					null,
					null,
					new Vector2(0, 0),
					0f,
					null,
					Color.White,
					SpriteEffects.None,
					spriteBatch.LayerDepth
				);			
			}			
			
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Cursor Point
			//
			spriteBatch.LayerDepth = this.ldCursor;
			if(this.isCanvasHovered() || this.isCanvasPushed()) {
				Point2D p = this.mouseCursorToPoint(this.Mouse.CurrentState);
				string description = String.Format("X:{0},Y:{1}", p.X, p.Y);
				Point2D sp = this.getNearestSnapPoint(p);
				
				if(this.Toolbox.SelectedTool.GetType() == typeof(PointTool)) {
					this.drawPoint(spriteBatch, sp, Color.Green);
				}
				
				string snapDesc = String.Format("X:{0},Y:{1}", sp.X, sp.Y);
				
				spriteBatch.DrawString(
					this.FontCanvas,
					description,
					new Vector2(this.Mouse.CurrentState.X + 20, this.Mouse.CurrentState.Y),
					Color.Red,
					0f,
					new Vector2(0,0),
					new Vector2(1,1),
					SpriteEffects.None,
					spriteBatch.LayerDepth
				);
				
				spriteBatch.DrawString(
					this.FontCanvas,
					snapDesc,
					new Vector2(this.Mouse.CurrentState.X + 20, this.Mouse.CurrentState.Y + 20),
					Color.Green,
					0f,
					new Vector2(0,0),
					new Vector2(1,1),
					SpriteEffects.None,
					spriteBatch.LayerDepth
				);
			}
			
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Draw Scroll Bars
			//
			spriteBatch.LayerDepth = this.ldScrollBars;
			// - Vertical Scroll Bar
			if(this.VerticalScrollVisible) {
				scrollBarVertical.draw(spriteBatch);
			}
			
			// - Horizontal Scroll Bar
			if(this.HorizontalScrollVisible) {
				scrollBarHorizontal.draw(spriteBatch);
			}			
		}
		
		/// <summary>
		/// Draw CAD 2D Point
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="p"></param>
		private void drawPoint(CADSpriteBatch spriteBatch, Point2D p, Color col)
		{
			Vector2 spv = this.pointToLocation(p);			
			spriteBatch.Draw(
				this.TexturePoint,
				new Vector2(spv.X-2,spv.Y-2),
				null,
				null,
				new Vector2(0,0),
				0f,
				null,
				col,
				SpriteEffects.None,
				spriteBatch.LayerDepth
			);
		}
		
		private void drawShape(CADSpriteBatch spriteBatch, Shape s, Color col)
		{
			for(int i=0; i<s.Points.Count; i++) {
				Vector2 spv = this.pointToLocation(s.Points[i]);
				Vector2 tpv = this.pointToLocation(s.Points[((i+1)==s.Points.Count) ? 0 : i+1]);
				spriteBatch.drawLine((int)spv.X,(int)spv.Y,(int)tpv.X,(int)tpv.Y,1,col);
			}
		}
		
		/// <summary>
		/// Apply textures to Scroll Bar Control
		/// </summary>
		/// <param name="sc"></param>
		private void textureScrollBar(ScrollBar sc)
		{
			sc.TextureArrowOn = this.TextureScrollArrowOn;
			sc.TextureArrowOff = this.TextureScrollArrowOff;
			sc.TextureArrowHover = this.TextureScrollArrowHover;
			sc.TextureArrowDisabled = this.TextureScrollArrowDisabled;
			sc.TextureRangeOn = this.TextureScrollRangeOn;
			sc.TextureRangeDisabled = this.TextureScrollRangeDisabled;
			sc.TextureBarEdgeOn = this.TextureScrollBarEdgeOn;
			sc.TextureBarEdgeOff = this.TextureScrollBarEdgeOff;
			sc.TextureBarEdgeHover = this.TextureScrollBarEdgeHover;
			sc.TextureBarCenterOn = this.TextureScrollBarCenterOn;
			sc.TextureBarCenterOff = this.TextureScrollBarCenterOff;
			sc.TextureBarCenterHover = this.TextureScrollBarCenterHover;
		}
		
		//
		//
		// DRAWING FUNCTIONALITY
		//
		//
			
		/// <summary>
		/// Converts MouseState on Canvas to Point2D
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public Point2D mouseCursorToPoint(MouseState ms)
		{
			int docX = ms.X + this.DocViewXOffset - this.Left;
			int docY = ms.Y + this.DocViewYOffset - this.Top;
			decimal docMmX = (decimal)docX / (decimal)this.RatioDotsPerMm;
			decimal docMmY = (decimal)docY / (decimal)this.RatioDotsPerMm;
			return new Point2D(docMmX, docMmY);
		}
		
		/// <summary>
		/// Converts from CAD 2D point to screen location vector
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector2 pointToLocation(Point2D p)
		{
			int xd = (int)(p.X * this.RatioDotsPerMm);
			int yd = (int)(p.Y * this.RatioDotsPerMm);
			return new Vector2(
				this.Left + xd - this.DocViewXOffset,
				this.Top + yd - this.DocViewYOffset
			);
		}
		
		/// <summary>
		/// Finds nearest on grid by input point
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Point2D getNearestSnapPoint(Point2D p)
		{
			int zx = (int)(p.X / this.GridStepMm);
			int zy = (int)(p.Y / this.GridStepMm);
			decimal sx = (decimal)zx * this.GridStepMm;
			decimal sy = (decimal)zy * this.GridStepMm;
			decimal rx = p.X % this.GridStepMm;
			decimal ry = p.Y % this.GridStepMm;
			if(rx > 0.5m*this.GridStepMm) {
				sx += this.GridStepMm;
			}
			if(ry > 0.5m*this.GridStepMm) {
				sy += this.GridStepMm;
			}					
			return new Point2D(sx, sy);
		}

	}
}

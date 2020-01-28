/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 07.12.2019
 * Time: 18:53
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Troopy;
using Troopy.Xna;
using Troopy.Xna.Controls;
using CNC.CADTools;

namespace CNC
{
	/// <summary>
	/// Monogame CNC machine controller
	/// </summary>
	public class GUI : Game
	{
        // Program Configuration
        private Config config;
		
		// User Interface Game
		GraphicsDeviceManager graphics;
        CADSpriteBatch spriteBatch;

		// Graphics
		CADContentFactory cadContent;
		
		// Input
		MouseStatus mouse;
		TimeStatus time;
		
		// Controls
		Button btnXup;
		Button btnDrillPoints;
		Button btnDrillShapes;
		Button btnGoToOrigin;
		Button btnClearPoints;
		ScrollBar sbManualSpeed;
		CAD2DDrawingPanel cadPanel;
		Machine machine;
		DrillTool drillTool;
		Toolbox toolbox;
		
		// Opened Files
		FileCollection files;
		
        /// <summary>
        /// Create instance of CNC Machine GUI
        /// </summary>
		public GUI()
		{
            // Load Configuration
            this.config = new Config();
			
			// Create GUI
			graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 

            // Input
            this.mouse = new MouseStatus();
            this.time = new TimeStatus();
            Control.MouseService = this.mouse;
            Control.TimeService = this.time;
            
			// Opened files
			this.files = new FileCollection();
		}
		
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>        
        protected override void Initialize()
        {
			// Graphics settings
    		graphics.PreferredBackBufferWidth = config.windowSizeX;
    		graphics.PreferredBackBufferHeight = config.windowSizeY;
    		graphics.IsFullScreen = config.windowMax;
    		graphics.ApplyChanges();		
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Creates ContentFactory
            this.cadContent = new CADContentFactory(Content);

        	// Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new CADSpriteBatch(GraphicsDevice, this.cadContent);
            
            // Create Controls
            btnXup = new Button();
            btnXup.Left = 10;
            btnXup.Top = 10;
            btnXup.Width = 39;
            btnXup.Height = 39;
            btnXup.TextureDisabled = this.cadContent.imgButtonArrowDisabled;
            btnXup.TextureIdle = this.cadContent.imgButtonArrowOff;
            btnXup.TexturePressed = this.cadContent.imgButtonArrowOn;
            btnXup.TextureHover = this.cadContent.imgButtonArrowHover; 
            btnXup.TextureOriginX = 19;
            btnXup.TextureOriginY = 19;
            btnXup.TextureRotation = (float)Math.PI * 2 * 0;
            btnXup.Enabled = true;
            
            btnDrillPoints = new Button();
            btnDrillPoints.Left = 300;
            btnDrillPoints.Top = 10;
            btnDrillPoints.Width = 120;
            btnDrillPoints.Height = 40;    
            btnDrillPoints.TextOffsetY = 12;
            btnDrillPoints.Enabled = true;
            btnDrillPoints.Text = "Drill Points";
            btnDrillPoints.Font = this.cadContent.fontConsolas12;
            btnDrillPoints.FontColor = Color.LightGreen;
            btnDrillPoints.BackGroundColor = Color.DarkGreen;    

            btnDrillShapes = new Button();
            btnDrillShapes.Left = 720;
            btnDrillShapes.Top = 10;
            btnDrillShapes.Width = 120;
            btnDrillShapes.Height = 40;    
            btnDrillShapes.TextOffsetY = 12;
            btnDrillShapes.Enabled = true;
            btnDrillShapes.Text = "Drill Shapes";
            btnDrillShapes.Font = this.cadContent.fontConsolas12;
            btnDrillShapes.FontColor = Color.LightGreen;
            btnDrillShapes.BackGroundColor = Color.DarkGreen;              
            
            this.sbManualSpeed = new ScrollBar();
            this.sbManualSpeed.Top = 10;
            this.sbManualSpeed.Left = 860;
            this.sbManualSpeed.Width = 200;
            this.sbManualSpeed.Height = 25;
            this.sbManualSpeed.Value = 0.5f;            
            this.sbManualSpeed.Enabled = true;
            this.sbManualSpeed.Range = 0.1f;
            this.sbManualSpeed.TextureArrowOn = this.cadContent.scrollArrowOn;
			this.sbManualSpeed.TextureArrowOff = this.cadContent.scrollArrowOff;
			this.sbManualSpeed.TextureArrowHover = this.cadContent.scrollArrowHover;
			this.sbManualSpeed.TextureArrowDisabled = this.cadContent.scrollArrowDisabled;
			this.sbManualSpeed.TextureRangeOn = this.cadContent.scrollRangeOn;
			this.sbManualSpeed.TextureRangeDisabled = this.cadContent.scrollRangeDisabled;
			this.sbManualSpeed.TextureBarEdgeOn = this.cadContent.scrollBarEdgeOn;
			this.sbManualSpeed.TextureBarEdgeOff = this.cadContent.scrollBarEdgeOff;
			this.sbManualSpeed.TextureBarEdgeHover = this.cadContent.scrollBarEdgeHover;
			this.sbManualSpeed.TextureBarCenterOn = this.cadContent.scrollBarCenterOn;
			this.sbManualSpeed.TextureBarCenterOff = this.cadContent.scrollBarCenterOff;
			this.sbManualSpeed.TextureBarCenterHover = this.cadContent.scrollBarCenterHover;            
           
            btnGoToOrigin = new Button();
            btnGoToOrigin.Left = 440;
            btnGoToOrigin.Top = 10;
            btnGoToOrigin.Width = 120;
            btnGoToOrigin.Height = 40;    
            btnGoToOrigin.TextOffsetY = 12;
            btnGoToOrigin.Enabled = true;
            btnGoToOrigin.Text = "GoTo Origin";
            btnGoToOrigin.Font = this.cadContent.fontConsolas12;
            btnGoToOrigin.FontColor = Color.LightGreen;
            btnGoToOrigin.BackGroundColor = Color.DarkGreen;

            btnClearPoints = new Button();
            btnClearPoints.Left = 580;
            btnClearPoints.Top = 10;
            btnClearPoints.Width = 120;
            btnClearPoints.Height = 40;    
            btnClearPoints.TextOffsetY = 12;
            btnClearPoints.Enabled = true;
            btnClearPoints.Text = "Clear Points";
            btnClearPoints.Font = this.cadContent.fontConsolas12;
            btnClearPoints.FontColor = Color.LightGreen;
            btnClearPoints.BackGroundColor = Color.DarkGreen;            
            
            // Drawing Toolbox
            this.toolbox = new Toolbox();
            this.toolbox.TextureToolSelect = this.cadContent.imgToolsSelect;
            this.toolbox.TextureToolMove = this.cadContent.imgToolsMove;
            this.toolbox.TextureToolDelete = this.cadContent.imgToolsDelete;
            this.toolbox.TextureToolPoint = this.cadContent.imgToolsPoint;
            this.toolbox.TextureToolShape = this.cadContent.imgToolsShape;
            this.toolbox.Left = 10;
            this.toolbox.Top = 60;
            this.toolbox.LayerBase = 0.1f;
            this.toolbox.Enabled = true;
            this.toolbox.createTools();

            
            // Machine
            this.machine = new Machine();
            this.machine.Left = 60;
            this.machine.Top = 10;
            this.machine.Width = 200;
            this.machine.Height = 40;
            this.machine.Enabled = true;
            this.machine.ComPortName = "COM7";
            this.machine.TextureController = this.cadContent.machineController;
            this.machine.Enabled = true;
            try {
            	this.machine.connect();
            	this.machine.origin();
            } catch(MachineException) {
            	this.machine.SimulationMode = true;
            }
            
            // Drill tool
            this.drillTool = new DrillTool(this.machine);
            
            // CAD Panel
            cadPanel = new CAD2DDrawingPanel();
            cadPanel.Left = 10 + 46;
            cadPanel.Top = 60;
            cadPanel.Width = config.windowSizeX - 20 - 46;
            cadPanel.Height = config.windowSizeY - 70;
            cadPanel.BackgroundColor = Color.Black;
            cadPanel.ScrollSize = 25;
            cadPanel.LayerBase = 0.9f;
			cadPanel.TextureScrollArrowOn = this.cadContent.scrollArrowOn;
			cadPanel.TextureScrollArrowOff = this.cadContent.scrollArrowOff;
			cadPanel.TextureScrollArrowHover = this.cadContent.scrollArrowHover;
			cadPanel.TextureScrollArrowDisabled = this.cadContent.scrollArrowDisabled;
			cadPanel.TextureScrollRangeOn = this.cadContent.scrollRangeOn;
			cadPanel.TextureScrollRangeDisabled = this.cadContent.scrollRangeDisabled;
			cadPanel.TextureScrollBarEdgeOn = this.cadContent.scrollBarEdgeOn;
			cadPanel.TextureScrollBarEdgeOff = this.cadContent.scrollBarEdgeOff;
			cadPanel.TextureScrollBarEdgeHover = this.cadContent.scrollBarEdgeHover;
			cadPanel.TextureScrollBarCenterOn = this.cadContent.scrollBarCenterOn;
			cadPanel.TextureScrollBarCenterOff = this.cadContent.scrollBarCenterOff;
			cadPanel.TextureScrollBarCenterHover = this.cadContent.scrollBarCenterHover;
			cadPanel.FontCanvas = this.cadContent.fontConsolas12;
			cadPanel.TexturePoint = this.cadContent.imgPoint;
			cadPanel.TextureMachineTool = this.cadContent.machineTool;
			cadPanel.Enabled = true;
			cadPanel.Machine = this.machine;
			cadPanel.Toolbox = this.toolbox;
        }  

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        	this.cadContent.unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {   
        	// Update Input
        	this.mouse.update(Mouse.GetState());
        	this.time.update(gameTime);

        	// Exit
        	if(
            	GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            	Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
        		config.save();
        		Exit();
            }
    	        
        	if(btnXup.isClicked()) {
        		Exit();
        	}
        	
        	// Start drill points from Panel
        	if(btnDrillPoints.isClicked()) {
        		if(this.machine.Enabled && this.machine.Idle) {
        			this.drillTool.drillPoints(this.cadPanel.File.Points, 10m,10m,50m,10m);
        		}
        	}

        	// Start drill points from Panel
        	if(btnDrillShapes.isClicked()) {
        		if(this.machine.Enabled && this.machine.Idle) {
        			this.drillTool.drillShapes(
        				//this.cadPanel.File.Shapes, 10m, 10m, 10m, 10m, 2m
        				this.cadPanel.File.Shapes, 10m, 0.5m, 10m, 0.1m, 0.5m
        			);
        		}
        	}        	
        	
        	// Start drill points from Panel
        	if(btnGoToOrigin.isClicked()) {
        		if(this.machine.Enabled && this.machine.Idle) {
        			this.machine.goTo(0,0,0,50m);
        		}
        	}
        	
        	// Start drill points from Panel
        	if(btnClearPoints.isClicked()) {
        		this.cadPanel.File.Elements.Clear();
        	}        	
        	
        	// Update CAD Panel
        	this.machine.ManualSpeed = sbManualSpeed.Value;
        	this.machine.update();
        	this.drillTool.update();
        	this.cadPanel.update();
        	this.sbManualSpeed.update();        	
        	        	
        	
			// Update in base class
            base.Update(gameTime);
        }  

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Init SpriteBatch
            //spriteBatch.Begin( SpriteSortMode.Deferred,null,SamplerState.LinearWrap);
            spriteBatch.Begin();
            spriteBatch.gameTime = gameTime;
            spriteBatch.mouse = Mouse.GetState();

			// Draw CadPanel
			cadPanel.draw(spriteBatch);
			
			
			// Draw around CAD Panel
			
			spriteBatch.drawRectangle(
				0,
				0,
				this.config.windowSizeX,
				cadPanel.Top,
				Color.SandyBrown
			);
			
			spriteBatch.drawRectangle(
				0,
				cadPanel.Top,
				cadPanel.Left,
				this.config.windowSizeY-this.cadPanel.Top,
				Color.SandyBrown
			);
			
			spriteBatch.drawRectangle(
				this.cadPanel.Left+this.cadPanel.Width,
				this.cadPanel.Top,
				this.config.windowSizeX-this.cadPanel.Width-this.cadPanel.Left,
				this.config.windowSizeY-this.cadPanel.Top,
				Color.SandyBrown
			);
			
			spriteBatch.drawRectangle(
				this.cadPanel.Left,
				this.cadPanel.Top + this.cadPanel.Height,
				this.cadPanel.Left + this.cadPanel.Width,
				this.config.windowSizeY - (this.cadPanel.Top + this.cadPanel.Height),
				Color.SandyBrown
			);
									
						
			// Draw Button
			btnXup.draw(spriteBatch);
			btnDrillPoints.draw(spriteBatch);
			btnDrillShapes.draw(spriteBatch);
			btnGoToOrigin.draw(spriteBatch);
			btnClearPoints.draw(spriteBatch);
			sbManualSpeed.draw(spriteBatch);
			
            // Draw Machine
            machine.draw(spriteBatch);
            
            // Draw Toolbox
            toolbox.draw(spriteBatch);

            // Draw Cursor
            spriteBatch.drawCrosshair();
            
            spriteBatch.End();

            base.Draw(gameTime);
        }        
	}
}

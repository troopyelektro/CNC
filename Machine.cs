/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 12.12.2019
 * Time: 7:32
 */
using System;
using System.IO.Ports;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Troopy;
using Troopy.Xna.Controls;
using System.Runtime;
using CNC.CADElements;

namespace CNC
{
	/// <summary>
	/// Machine Controller
	/// </summary>
	public class Machine : Control
	{
		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		// Communication Interface:
		//

		/// <summary>
		/// Speed to move in manual mode 0-1
		/// </summary>
		private float manualSpeed = 0.5f;
		public float ManualSpeed {
			get {
				return this.manualSpeed;
			}
			set {
				this.manualSpeed = value;
			}
		}
		
		/// <summary>
		/// COM Port communication baud rate
		/// </summary>
		private int comBaudRate = 115200;

		/// <summary>
		/// COM Port communication baud rate
		/// </summary>
		public int ComBaudRate {
			get {
				return this.comBaudRate;
			}
		}
				
		/// <summary>
		/// COM Port name
		/// </summary>
		public string ComPortName {
			get {
				return this.comPort.PortName;
			}
			set {
				this.comPort.PortName = value;
			}
		}
		
		/// <summary>
		/// Determines, if machine is connected
		/// </summary>
		public bool IsConnected {
			get {
				return this.comPort.IsOpen;
			}
		}

		/// <summary>
		/// Determines if machine moving
		/// </summary>
		private bool moving;
		
		/// <summary>
		/// Determines if machine is idle
		/// </summary>
		public bool Idle {
			get {
				return !this.moving && (this.acknowledged || this.SimulationMode);
			}
		}	
						
		/// <summary>
		/// Acknowledge from Maschine
		/// </summary>
		private bool acknowledged;
		public bool Acknowledged {
			get {
				return this.acknowledged;
			}
		}		
		
		/// <summary>
		/// Serial Port for communication with machine
		/// </summary>
		private SerialPort comPort;
		
		private byte commandAcknowledge = 20;
		private byte commandGoTo = 30;
		private byte commandSetOrigin = 31 | (1 << 7);
		
		/// <summary>
		/// Simulation mode. If enabled, serial communication disabled
		/// </summary>
		private bool simulationMode = false;
		/// <summary>
		/// Simulation mode. If enabled, serial communication disabled
		/// </summary>		
		public bool SimulationMode {
			get {
				return this.simulationMode;
			}
			set {
				this.simulationMode = value;
			}
		}
		
		
		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		// Technlogical properties:
		//
						
		/// <summary>
		/// Current position of machine
		/// </summary>
		private Point3D position;
			
		/// <summary>
		/// Current position of machine
		/// </summary>				
		public Point3D Position {
			get {
				if(this.moving) {
					// Update position
					TimeSpan total = this.timeEnd - this.timeStart;
					TimeSpan elapsed = this.Time.CurrentState - this.timeStart;
					decimal process = (decimal)elapsed.TotalMilliseconds / (decimal)total.TotalMilliseconds;
					
					// Recalculate position
					this.position.X = this.MoveStart.X + (this.MoveEnd.X - this.MoveStart.X) * process;
					this.position.Y = this.MoveStart.Y + (this.MoveEnd.Y - this.MoveStart.Y) * process;
					this.position.Z = this.MoveStart.Z + (this.MoveEnd.Z - this.MoveStart.Z) * process;
					
					// Finish move
					if(process >= 1) {
						this.moving = false;
						this.position.setPosition(this.MoveEnd);						
					}															 
				}
				return this.position;
			}
		}				

		// 200 Steps = 1 Rotation
		// Pitch 2mm
		private decimal stepX = 0.01m;
		private decimal stepY = 0.01m;
		private decimal stepZ = 0.0025m;	
		private long goToCntX = 0;
		private long goToCntY = 0;
		private long goToCntZ = 0;		
		private TimeSpan timeStart;	
		private TimeSpan timeEnd;
		
		
		/// <summary>
		/// Initial Point of move
		/// </summary>
		private Point3D moveStart;

		/// <summary>
		/// Initial Point of move
		/// </summary>
		public Point3D MoveStart {
			get {
				return this.moveStart;
			}
		}

		/// <summary>
		/// Target Point of move
		/// </summary>		
		private Point3D moveEnd;

		/// <summary>
		/// Target Point of move
		/// </summary>		
		public Point3D MoveEnd {
			get {
				return this.moveEnd;
			}
		}	
		
		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		// User Interface:
		//
		public Texture2D TextureController;
		
		/// <summary>
		/// Creates instance of machine controller
		/// </summary>
		public Machine()
		{
			this.comPort = new SerialPort();
			this.comPort.BaudRate = this.ComBaudRate;
			this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comMachineDataReceived);
			
			// Acknowledge true on init
			this.acknowledged = true;
			
			// Init point memory
			this.position = new Point3D(0m,0m,0m);
			this.moveStart = new Point3D(0m,0m,0m);
			this.moveEnd = new Point3D(0m,0m,0m);
		}
		
		/// <summary>
		/// Connects to machine
		/// </summary>
		public void connect()
		{
			try {
				comPort.Open();
				this.acknowledged = true;
			} catch(Exception ex) {
				throw new MachineException(ex.Message);
			}
		}		
		
		/// <summary>
		/// Moves Machine to target 
		/// </summary>
		/// <param name="targetX"></param>
		/// <param name="targetY"></param>
		/// <param name="targetZ"></param>
		/// <param name="speed">[mm/s]</param>
		public void goTo(decimal targetX, decimal targetY, decimal targetZ, decimal speed)
		{
			if((this.IsConnected || this.SimulationMode) && this.Idle) {
				// Store Current TimeSpan
				this.timeStart = this.Time.CurrentState;
				
				// Set Start and End move Poins
				this.moveStart.setPosition(this.Position);
				this.moveEnd = new Point3D(targetX, targetY, targetZ);
				
				// Calculate point distance
				decimal diffX = this.moveEnd.X-this.MoveStart.X;
				decimal diffY = this.moveEnd.Y-this.MoveStart.Y;
				decimal diffZ = this.moveEnd.Z-this.MoveStart.Z;				
				decimal diffXY = (decimal)Math.Sqrt(Math.Pow((double)diffX,2) + Math.Pow((double)(diffY),2));
				decimal diffXYZ = (decimal)Math.Sqrt(Math.Pow((double)(diffXY),2) + Math.Pow((double)(diffZ),2));
				
				if(diffXYZ > 0) {
					// Calculate Estimated Time
					decimal seconds = diffXYZ / speed;	
					decimal miliseconds = seconds * 1000m;
					this.timeEnd = this.timeStart.Add(TimeSpan.FromSeconds((double)seconds));
					// Calculate Elemental speeds
					decimal speedX = speed * Math.Abs((diffX)/diffXYZ); // TODO Division by zero if same point
					decimal speedY = speed * Math.Abs((diffY)/diffXYZ);
					decimal speedZ = speed * Math.Abs((diffZ)/diffXYZ);
					
					// Calculate Target Counter
					this.goToCntX = (short)(targetX / this.stepX);
					this.goToCntY = (short)(targetY / this.stepY);
					this.goToCntZ = (short)(targetZ / this.stepZ);
					
					this.moving = true;
					this.acknowledged = false;
					
					// Create Send Buffer								
					MemoryStream ms = new MemoryStream(17);													
					byte[] bX = BitConverter.GetBytes(this.goToCntX);
					byte[] bY = BitConverter.GetBytes(this.goToCntY);
					byte[] bZ = BitConverter.GetBytes(this.goToCntZ);
					byte[] bS = BitConverter.GetBytes((long)miliseconds);
					ms.WriteByte(this.commandGoTo);
					ms.Write(bX,0,2);
					ms.Write(bY,0,2);
					ms.Write(bZ,0,2);
					ms.Write(bS,0,8);
					ms.Close();				
					
					// Send Command
					if(!this.SimulationMode) {
						comPort.Write(ms.GetBuffer(),0,15);
					}
				}				
			}
		}
		
		/// <summary>
		/// Moves Machine to target 
		/// </summary>
		public void origin()
		{
			if((this.IsConnected || this.SimulationMode) && this.Idle) {
				this.Position.X = 0m;
				this.Position.Y = 0m;
				this.Position.Z = 0m;
				this.acknowledged = false;
				
				// Create Send Buffer								
				MemoryStream ms = new MemoryStream(1);													
				ms.WriteByte(this.commandSetOrigin);
				ms.Close();				
				
				// Send Command
				if(!this.SimulationMode) {
					comPort.Write(ms.GetBuffer(),0,1);
				}
			}
		}		
				
		/// <summary>
		/// Data from machine received
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comMachineDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string buf = comPort.ReadExisting();
			for(int i=0; i<buf.Length; i++) {				
				char c = buf[i];
				if(c == (char)this.commandAcknowledge) {
					this.acknowledged = true;
				}
			}
		}

		public void update()
		{
			// Controller Pushed
			if(this.isPushed()) {
				decimal steps = (decimal)this.manualSpeed*20m;
				bool go = false;
				int x = this.Mouse.LeftButtonPressed.X - this.Left;
				int y = this.Mouse.LeftButtonPressed.Y - this.Top;
				Point3D p = new Point3D(this.Position);
				if(x >= 5 && y >= 9 && x <= 25 && y <= 29) {
					// X-
					p.X -= this.stepX * steps;
					go = true;
				}

				if(x >= 48 && y >= 9 && x <= 69 && y <= 29) {
					// X+
					p.X += this.stepX * steps;					
					go = true;
				}

				if(x >= 26 && y >= 1 && x <= 47 && y <= 19) {
					// Y-	
					p.Y -= this.stepY * steps;
					go = true;
				}

				if(x >= 26 && y >= 20 && x <= 47 && y <= 38) {
					// Y+
					p.Y += this.stepY * steps;
					go = true;
				}

				if(x >= 80 && y >= 1 && x <= 101 && y <= 19) {
					// Z+
					p.Z += this.stepZ * steps;					
					go = true;
				}

				if(x >= 80 && y >= 20 && x <= 101 && y <= 38) {
					// Z-
					p.Z -= this.stepZ * steps;
					go = true;
				}
				if(go) {
					this.goTo(p.X, p.Y, p.Z, (decimal)this.manualSpeed*5);
				}
			}
			
			if(this.isClicked()) {
				int x1 = this.Mouse.LeftButtonPressed.X - this.Left;
				int y1 = this.Mouse.LeftButtonPressed.Y - this.Top;
				int x2 = this.Mouse.LeftButtonReleased.X - this.Left;
				int y2 = this.Mouse.LeftButtonReleased.Y - this.Top;
				if(x1 >= 117 && y1 >= 1 && x1 <= 198 && y1 <= 38) {
					// Origin
					if(x2 >= 117 && y2 >= 1 && x2 <= 198 && y2 <= 38) {
						// Origin
						this.origin();
					}													
				}								
			}
		}
		
		
		/// <summary>
		/// Draw Machine Controller
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void draw(Troopy.Xna.SpriteBatch spriteBatch)
		{			
			if(this.TextureController != null) {
				spriteBatch.Draw(
					this.TextureController,
					new Vector2(this.Left, this.Top),
					Color.White
				);
			}
		}
		
	}
	
	/// <summary>
	/// Machine Exception
	/// </summary>
	public class MachineException : TroopyException
	{
		public MachineException(string message) : base(message)
		{
		}		
	}
}

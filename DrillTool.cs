/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 14.12.2019
 * Time: 11:11
 */
using System;
using System.Collections.Generic;
using CNC.CADElements;

namespace CNC
{
	/// <summary>
	/// Description of CNC_DrillTool.
	/// </summary>
	public class DrillTool
	{
		/// <summary>
		/// Provided machine
		/// </summary>
		private Machine machine;
		
		/// <summary>
		/// Determines if operation is done
		/// </summary>
		private bool done;
		/// <summary>
		/// Determines if operation is done
		/// </summary>
		public bool Done {
			get {
				return this.done;
			}
		}
		
		//
		// Technological properties
		//
		private decimal surfaceZ;
		private decimal surfaceZreserve = 1m;
		private List<Point3D> waypoints;
		private List<decimal> speeds;		
		private int waypointIndex;
		
		/// <summary>
		/// Creates an instance of drill tool, providing machine for operation
		/// </summary>
		/// <param name="m"></param>
		public DrillTool(Machine m)
		{
			this.machine = m;
			this.done = true;
		}
		
		/// <summary>
		/// Start Point drilling
		/// </summary>
		/// <param name="points"></param>
		/// <param name="sky"></param>
		/// <param name="depth"></param>
		/// <param name="speedMove"></param>
		/// <param name="speedDrill"></param>
		public void drillPoints(List<Point2D> points, decimal sky, decimal depth, decimal speedMove, decimal speedDrill)
		{
			if(this.done && this.machine.Idle) {
				this.done = false;
				this.surfaceZ = this.machine.Position.Z;
				this.waypoints = new List<Point3D>();
				this.speeds = new List<decimal>();
				this.waypointIndex = 0;
				
				// Prepare waypoints
				foreach(Point2D point in points) {
					// Under the point
					this.waypoints.Add(new Point3D(point.X, point.Y, surfaceZ - sky));
					this.speeds.Add(speedMove);
					// Nearly up the point
					this.waypoints.Add(new Point3D(point.X, point.Y, surfaceZ - surfaceZreserve));
					this.speeds.Add(speedMove);					
					// Drill
					this.waypoints.Add(new Point3D(point.X, point.Y, surfaceZ + depth));
					this.speeds.Add(speedDrill);										
					// Slowly up					
					this.waypoints.Add(new Point3D(point.X, point.Y, surfaceZ - surfaceZreserve));
					this.speeds.Add(speedDrill);										
					// Under the point
					this.waypoints.Add(new Point3D(point.X, point.Y, surfaceZ - sky));
					this.speeds.Add(speedMove);					
				}
			}
		}
		
		public void drillShapes(List<Shape> shapes, decimal sky, decimal depth, decimal speedMove, decimal speedDrill, decimal depthStep)
		{
			if(this.done && this.machine.Idle) {
				this.done = false;
				this.surfaceZ = this.machine.Position.Z;
				this.waypoints = new List<Point3D>();
				this.speeds = new List<decimal>();
				this.waypointIndex = 0;
				
				// Prepare waypoints
				foreach(Shape shape in shapes) {
					Point2D pointBegin = shape.Points[0];										
					
					// Under the point
					this.waypoints.Add(new Point3D(pointBegin.X,pointBegin.Y, surfaceZ - sky));
					this.speeds.Add(speedMove);
					// Nearly up the point
					this.waypoints.Add(new Point3D(pointBegin.X, pointBegin.Y, surfaceZ - surfaceZreserve));
					this.speeds.Add(speedMove);					
					
					bool doneZ=false;
					for(decimal z=this.surfaceZ+depthStep;  !doneZ ; z=z+depthStep) {
						// FIX Z > DEPTH
						if(z>(this.surfaceZ+depth)) {
							z=this.surfaceZ+depth;
							doneZ = true;
						}																		
						
						// Run Shape
						foreach(Point2D p in shape.Points) {
							// Drill
							this.waypoints.Add(new Point3D(p.X, p.Y, z));
							this.speeds.Add(speedDrill);															
						}					
						this.waypoints.Add(new Point3D(pointBegin.X, pointBegin.Y, z));
						this.speeds.Add(speedDrill);						
					}

					// Slowly up					
					this.waypoints.Add(new Point3D(pointBegin.X, pointBegin.Y, surfaceZ - surfaceZreserve));
					this.speeds.Add(speedDrill);										
					// Under the point
					this.waypoints.Add(new Point3D(pointBegin.X, pointBegin.Y, surfaceZ - sky));
					this.speeds.Add(speedMove);					
				}
			}		
		}
		
		/// <summary>
		/// Updates operation if running
		/// </summary>
		public void update()
		{
			if(!this.done && this.machine.Idle && this.waypointIndex < this.waypoints.Count) {				
				Point3D p = this.waypoints[waypointIndex];
				decimal s = this.speeds[waypointIndex];
				this.machine.goTo(p.X, p.Y, p.Z, s);				
				waypointIndex++;
			}
			
			// Check if operation is done
			if(!this.done && this.machine.Idle && this.waypointIndex == this.waypoints.Count) {
				this.done = true;
			}			
		}
	}
}

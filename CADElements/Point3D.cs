/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 12.12.2019
 * Time: 12:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CNC.CADElements
{
	/// <summary>
	/// Description of CNC_Point3D.
	/// </summary>
	public class Point3D : Point2D
	{
		/// <summary>
		/// Location Z of point
		/// </summary>
		private decimal z;

		/// <summary>
		/// Location Z of point
		/// </summary>
		public decimal Z {
			get {
				return this.z;
			}
			set {
				this.z = value;
			}
		}

		/// <summary>
		/// Create instance of CAD 3D Point
		/// </summary>
		/// <param name="locX"></param>
		/// <param name="locY"></param>
		/// <param name="locZ"></param>
		public Point3D(decimal locX, decimal locY, decimal locZ) : base(locX, locY)
		{
			this.z = locZ;
		}
		
		/// <summary>
		/// Create instance of CAD 3D Point
		/// </summary>
		/// <param name="locX"></param>
		/// <param name="locY"></param>
		/// <param name="locZ"></param>
		public Point3D(Point3D p) : base(p.X, p.Y)
		{
			this.z = p.Z;
		}		
		
		/// <summary>
		/// Set Position by another point
		/// </summary>
		/// <param name="p">Another point</param>
		public void setPosition(Point3D p)
		{
			this.X = p.X;
			this.Y = p.Y;
			this.Z = p.Z;
		}
	}
}

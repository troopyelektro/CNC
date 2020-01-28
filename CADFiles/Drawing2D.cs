/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 06.12.2019
 * Time: 7:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using CNC.CADElements;

namespace CNC.CADFiles
{
	/// <summary>
	/// File for 2D Drawing
	/// </summary>
	public class Drawing2D : File
	{	
		//
		// DOCUMENT WIDTH
		// 		
		
		/// <summary>
		/// Document size in Milimeters, width
		/// </summary>
		private decimal docSizeMmX = 100m;		
		/// <summary>
		/// Document size in Milimeters, width
		/// </summary>
		public decimal DocSizeMmX {
			get {
				return this.docSizeMmX;
			}
			set {
				this.docSizeMmX = value;
			}
		}

		//
		// DOCUMENT HEIGHT
		// 
		
		/// <summary>
		/// Document size in Milimeters, height
		/// </summary>
		private decimal docSizeMmY = 50m;
		/// <summary>
		/// Document size in Milimeters, height
		/// </summary>
		public decimal DocSizeMmY {
			get {
				return this.docSizeMmY;
			}
			set {
				this.docSizeMmY = value;
			}
		}

		/// <summary>
		/// Points on 2D Drawing
		/// </summary>
		private List<Element> elements;

		/// <summary>
		/// Points on 2D Drawing
		/// </summary>		
		public List<Element> Elements {
			get {
				return this.elements;
			}
			set {
				this.elements = value;
			}
		}
		
		/// <summary>
		/// Get Points in file
		/// </summary>
		public List<Point2D> Points {
			get {
				List<Point2D> pl = new List<Point2D>();
				foreach(Element e in this.Elements) {
					if(e.GetType() ==  typeof(Point2D)) {
						pl.Add((Point2D)e);
					}
				}
				return pl;
			}
		}
		
		/// <summary>
		/// Get Points in file
		/// </summary>
		public List<Shape> Shapes {
			get {
				List<Shape> pl = new List<Shape>();
				foreach(Element e in this.Elements) {
					if(e.GetType() ==  typeof(Shape)) {
						pl.Add((Shape)e);
					}
				}
				return pl;
			}
		}		

		/// <summary>
		/// Makes an instance of new file
		/// </summary>
		public Drawing2D() : base()
		{	
			this.Elements = new List<Element>();
		}
		
		/// <summary>
		/// Makes an instance of existing file
		/// </summary>
		/// <param name="fileName"></param>
		public Drawing2D(string fileName) : base(fileName)
		{
			this.Elements = new List<Element>();
		}
		
		public bool deletePoint(Point2D p)
		{
			List<Shape> shapesDelete = new List<Shape>();
			
			if(this.Elements.Contains(p)) {
				foreach(Element e in this.Elements) {
					// Remove point from shapes
					if(e.GetType() == typeof(Shape)) {
					   	Shape s = (Shape)e;
					   	if(s.Points.Contains(p)) {
					   		s.Points.Remove(p);
					   		// Remove also shape if it has not enough points
					   		if(s.Points.Count < 3) {
					   			shapesDelete.Add(s);
					   		}
					   	}					   	
					}
				}
				this.Elements.Remove(p);
				foreach(Shape s in shapesDelete) {
					this.Elements.Remove(s);
				}
				return true;
			} else {
				return false;
			}
		}
		
		public bool deleteShape(Shape s)
		{			
			if(this.Elements.Contains(s)) {
				this.Elements.Remove(s);
				return true;
			} else {
				return false;
			}

		}		
	}
}

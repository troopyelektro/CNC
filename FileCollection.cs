/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 06.12.2019
 * Time: 6:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using CNC.CADFiles;

namespace CNC
{
	/// <summary>
	/// Keeps set of files, which are opened by application
	/// </summary>
	public class FileCollection
	{
		/// <summary>
		/// List of CAD Files opened
		/// </summary>
		private List<File> items;
		
		/// <summary>
		/// List of CAD Files opened 
		/// </summary>
		public List<File> Items {
			get {
				return this.items;
			}
		}
		
		/// <summary>
		/// Creates instance of FileCollection
		/// </summary>
		public FileCollection()
		{
			this.items = new List<File>();
		}
	}
}

/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 06.12.2019
 * Time: 6:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CNC.CADFiles
{
	/// <summary>
	/// Instance of CAD File
	/// </summary>
	public class File
	{
		/// <summary>
		/// Determines if file is new (file name is not defined)
		/// </summary>
		private bool isNew = true;

		/// <summary>
		/// Determines if file is new (file name is not defined)
		/// </summary>
		public bool IsNew {
			get {
				return this.isNew;
			}
		}
		
		/// <summary>
		/// file Path including file Name
		/// </summary>
		private string fileName;
		
		/// <summary>
		/// file Path including file Name
		/// </summary>
		public string FileName {
			get {
				return this.fileName;
			}
		}

		/// <summary>
		/// Makes an instance of new file
		/// </summary>
		public File()
		{					
		}
		
		/// <summary>
		/// Makes an instance of existing file
		/// </summary>
		/// <param name="fileName"></param>
		public File(string fileName)
		{
			this.isNew = false;
		}
	}
}

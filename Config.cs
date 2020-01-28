/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 07.12.2019
 * Time: 11:52
 */
using System;
using Troopy;
using System.IO;

namespace CNC
{
	/// <summary>
	/// Operates with application configuration
	/// </summary>
	public class Config
	{
		/// <summary>
		/// Configuration Parameter Names
		/// </summary>
		private string[] ParameterNames {
			get {
				return new String[] {
					"windowSizeX",
					"windowSizeY",
					"windowMax"					
				};
			}
		}

		/// <summary>
		/// Configuration Parameter Defaults
		/// </summary>
		private string[] ParameterDefaults {
			get {
				return new String[] {
					"800",
					"600",
					"false",					
				};			
			}
		}

		/// <summary>
		/// Main Window Resolution Width
		/// </summary>
		public int windowSizeX {
			get {
				return this.parameterCollection.getInt("windowSizeX");
			}
			set {
				this.parameterCollection.setInt("windowSizeX", value);
			}
		}
		
		/// <summary>
		/// Main Window Resolution Height
		/// </summary>
		public int windowSizeY {
			get {
				return this.parameterCollection.getInt("windowSizeY");
			}
			set {
				this.parameterCollection.setInt("windowSizeY", value);
			}
		}

		/// <summary>
		/// Main Window Resolution Height
		/// </summary>
		public bool windowMax {
			get {
				return this.parameterCollection.getBool("windowMax");
			}
			set {
				this.parameterCollection.setBool("windowMax", value);
			}
		}			
								
		/// <summary>
		/// Get FileName of Configuration file
		/// </summary>
		public string FileName {
			get {
				return AppBase.getAppPath() + "\\config.ini";
			}
		}
								
		/// <summary>
		/// Configurations Parameter Collection
		/// </summary>
		private ParameterCollection parameterCollection;
		

		
		/// <summary>
		/// Creates instance of configuration.
		/// File exists: Load
		/// File not exists: Create file and fillup default values
		/// </summary>
		public Config()
		{
			// Config file exists?
			if(File.Exists(this.FileName)) {
				// Yes
				this.load();			
			} else {
				// No
				this.parameterCollection = new ParameterCollection(this.ParameterNames, "", this.ParameterDefaults);				
				this.save();
			}
		}
		
		/// <summary>
		/// Loads configuration from file
		/// </summary>
		public void load()
		{
			// Create stream Reader from File
			StreamReader fr = new StreamReader(this.FileName);
			this.parameterCollection = new ParameterCollection(this.ParameterNames, fr.ReadToEnd(), this.ParameterDefaults);
			fr.Close();
		}
		
		/// <summary>
		/// saves configuration to file
		/// </summary>
		public void save()
		{
			// Create stream writer
			StreamWriter fw;
			fw = new StreamWriter(this.FileName);
			fw.Write(this.parameterCollection.Data);
			fw.Close();
		}
	}
}

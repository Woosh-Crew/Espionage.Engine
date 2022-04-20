using System;
using System.IO;
using System.Linq;

namespace Espionage.Engine.Resources
{
	[Group( "Data" ), Path( "custom_assets", "assets://Data", Overridable = true )]
	public abstract class Asset : IResource, IWatchable
	{
		public Library ClassInfo { get; }
		public string Name { get; set; }

		public Asset()
		{
			ClassInfo = Library.Register( this );
		}

		// Compile

		internal void Compile( BinaryWriter writer )
		{
			writer.Write( ClassInfo.Id );

			foreach ( var property in ClassInfo.Properties.Where( e => e.Serialized ) )
			{
				// Write ID and Offset into file
				writer.Write( property.Identifier );
			}

			// Data

			OnCompile( writer );
		}

		protected virtual void OnCompile( BinaryWriter writer ) { }

		// Resource

		int IResource.Identifier { get; set; }
		bool IResource.Persistant { get; set; }

		void IResource.Setup( string path )
		{
			Source = new( path );
		}

		protected FileInfo Source { get; private set; }

		void IResource.Load()
		{
			using var file = Source.Open( FileMode.Open, FileAccess.Read );
			using var reader = new BinaryReader( file );

			Assert.IsTrue( reader.ReadInt32() != ClassInfo.Id );
			OnLoad( reader );
		}

		protected virtual void OnLoad( BinaryReader reader ) { }

		bool IResource.Unload()
		{
			OnUnload();
			return true;
		}

		protected virtual void OnUnload() { }

		// Hotloading

		void IWatchable.OnHotload()
		{
			(this as IResource).Load();
		}

		void IWatchable.OnDeleted() { }

		// Compiler

		internal class Compiler : ICompiler<Asset>
		{
			public void Compile( Asset asset )
			{
				var extension = asset.ClassInfo.Components.Get<FileAttribute>()?.Extension ?? asset.ClassInfo.Name.ToLower();

				// Create path, just in case
				Files.Pathing.Create( "assets://Data" );

				using var stopwatch = Debugging.Stopwatch( $"{asset.ClassInfo.Title} Compiled", true );
				using var file = File.Create( Files.Pathing.Absolute( $"custom_assets://{asset.Name}.{extension}" ) );
				using var writer = new BinaryWriter( file );

				try
				{
					asset.Compile( writer );
				}
				catch ( Exception e )
				{
					Debugging.Log.Error( "Compile Failed!" );
					Debugging.Log.Exception( e );
				}
			}
		}
	}
}

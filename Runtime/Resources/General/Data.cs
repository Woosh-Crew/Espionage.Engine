using System;
using System.IO;
using System.Linq;
using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	[Group( "Data" ), Path( "data", "assets://Data" )]
	public abstract class Data : IAsset, IWatchable
	{
		public Library ClassInfo { get; }
		public string Name { get; set; }

		public Data()
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

		public Resource Resource { get; set; }
		protected FileInfo Source { get; private set; }

		void IAsset.Setup( Pathing path )
		{
			Source = new( path );
		}

		void IAsset.Load()
		{
			using var file = Source.Open( FileMode.Open, FileAccess.Read );
			using var reader = new BinaryReader( file );

			Assert.IsTrue( reader.ReadInt32() != ClassInfo.Id );
			OnLoad( reader );
		}

		protected virtual void OnLoad( BinaryReader reader ) { }

		void IAsset.Unload()
		{
			OnUnload();
		}

		protected virtual void OnUnload() { }

		IAsset IAsset.Clone()
		{
			return this;
		}

		// Hotloading

		void IWatchable.OnHotload()
		{
			(this as IAsset).Load();
		}

		void IWatchable.OnDeleted() { }

		// Compiler

		internal class Compiler : ICompiler<Data>
		{
			public void Compile( Data data )
			{
				var extension = data.ClassInfo.Components.Get<FileAttribute>()?.Extension ?? data.ClassInfo.Name.ToLower();

				// Create path, just in case
				Files.Pathing( "data://" ).Create();

				using var stopwatch = Debugging.Stopwatch( $"{data.ClassInfo.Title} Compiled", true );
				using var file = File.Create( Files.Pathing( $"data://{data.Name}.{extension}" ).Absolute() );
				using var writer = new BinaryWriter( file );

				try
				{
					data.Compile( writer );
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

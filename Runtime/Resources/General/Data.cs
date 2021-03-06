using System;
using System.IO;
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

		public void Compile()
		{
			Resources.Compiler.Compile<Data>( this );
		}

		internal void Compile( BinaryWriter writer )
		{
			writer.Write( ClassInfo.Id );
			OnCompile( writer );
		}

		protected virtual void OnCompile( BinaryWriter writer ) { }

		// Resource

		public Resource Resource { get; set; }
		protected FileInfo Source { get; private set; }

		void IAsset.Setup( Pathing path )
		{
			Source = new( path.Absolute() );
			Name = path.Name( false );
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

		[Library( "data.compiler" )]
		internal class Compiler : ICompiler<Data>, ILibrary
		{
			public Library ClassInfo => typeof( Compiler );

			public void Compile( Data data )
			{
				var extension = data.ClassInfo.Components.Get<FileAttribute>()?.Extension ?? data.ClassInfo.Name.ToLower();
				var output = Files.Pathing( $"{(data.ClassInfo.Components.Get<PathAttribute>()?.ShortHand ?? "data")}://" ).Absolute();

				// Create path, just in case
				output.Create();

				var outputPath = Files.Pathing( $"{output.Output}/{data.Name}.{extension}" ).Absolute();
				using var stopwatch = Debugging.Stopwatch( $"{data.ClassInfo.Title} Compiled [{outputPath.Output}]", true );
				using var file = File.Create( outputPath );
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

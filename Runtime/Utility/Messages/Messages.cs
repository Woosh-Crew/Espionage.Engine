using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Messages is responsible for sending messages between editor and
	/// the built version of the game. We can use this for invoking commands
	/// in the console from the editor process. (Launch maps in game from editor)
	/// </summary>
	public static class Messages
	{
		// Editor [Server]

		#if UNITY_EDITOR

		public static void Send( string message )
		{
			if ( Writer == null )
			{
				Threading.Create( "editor_ipc", new( () => Server( message ) ) { IsBackground = true } );
			}
			else
			{
				Dev.Log.Info( "Pipe Already Exists, Sending message" );
				Write( message );
			}
		}

		private static void Write( string message )
		{
			try
			{
				Writer.WriteLine( message );
			}
			catch ( IOException e )
			{
				// Catch the IOException that is raised if the pipe is broken or disconnected
				Dev.Log.Error( $"[SERVER] Error: {e.Message}" );
			}
		}


		private static Process Game { get; set; }
		private static StreamWriter Writer { get; set; }
		private static AnonymousPipeServerStream Pipe { get; set; }

		private static void Server( string launchArgs )
		{
			UnityEditor.EditorApplication.quitting += () =>
			{
				// Close Game, if we're shutting the editor down.
				Game.Close();
			};

			Game = new() { StartInfo = new( Files.Pathing.Absolute( "compiled://<executable>" ), launchArgs ) { UseShellExecute = false } };
			Pipe = new( PipeDirection.Out, HandleInheritability.Inheritable );

			Dev.Log.Info( "[SERVER] Creating Pipe" );
			Dev.Log.Info( $"[SERVER] Current Transmission Mode: {Pipe.TransmissionMode}." );

			// Pass the client process a handle to the server.
			Game.StartInfo.Arguments += " -connect " + Pipe.GetClientHandleAsString();
			Game.Start();

			Pipe.DisposeLocalCopyOfClientHandle();

			try
			{
				// Read Input from Editor
				Writer = new( Pipe );
				Writer.AutoFlush = true;

				Write( $"Connected to {Process.GetCurrentProcess().ProcessName}" );
				Pipe.WaitForPipeDrain();
			}
			catch ( IOException e )
			{
				// Catch the IOException that is raised if the pipe is broken or disconnected
				Dev.Log.Error( $"[SERVER] Error: {e.Message}" );
			}

			Game.WaitForExit();
			Game.Close();

			Shutdown();

			Dev.Log.Info( "[SERVER] Client quit. Server terminating." );
			Threading.Running["editor_ipc"].Close();
		}

		private static void Shutdown()
		{
			Writer?.Dispose();
			Pipe?.Dispose();

			Writer = null;
			Pipe = null;
		}

		#endif

		// Game [Client]

		internal static void Connect( string handle )
		{
			var thread = Threading.Create( "game_ipc", new( () => Client( handle ) ) { IsBackground = true } );
			Application.quitting += thread.Close;
		}

		private static void Receive( string message )
		{
			if ( message.StartsWith( "+" ) )
			{
				// Call on main thread
				Threading.Main.Enqueue( () => Dev.Terminal.Invoke( message[1..] ) );
			}
		}

		private static void Client( string handle )
		{
			Dev.Log.Info( $"[CLIENT] Connecting to pipe {handle}" );

			// Open Pipe to Server
			using PipeStream game = new AnonymousPipeClientStream( PipeDirection.In, handle );
			using var sr = new StreamReader( game );

			Dev.Log.Info( $"[CLIENT] Current Transmission Mode: {game.TransmissionMode}." );

			string message;
			while ( (message = sr.ReadLine()) != null )
			{
				Dev.Log.Info( "[CLIENT] Received Message: " + message );
				Receive( message );
			}
		}
	}
}

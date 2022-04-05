using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace Espionage.Engine
{
	public static class Messages
	{
		public static void Send( string message )
		{
			if ( Writer == null )
			{
				var thread = new Thread( () => Server( message ) );
				thread.Start();
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

		internal static void Connect( string pipeHandle )
		{
			var thread = new Thread( () => Client( pipeHandle ) );
			thread.Start();

			Application.quitting += thread.Abort;
		}

		private static void Receive( string message )
		{
			if ( message.StartsWith( "+" ) )
			{
				// Dev.Terminal.Invoke( message[1..]);
			}
		}

		// Editor [Server]

		private static StreamWriter Writer { get; set; }

		private static void Server( string launchArgs )
		{
			var game = new Process() { StartInfo = new( Files.Pathing.Absolute( "compiled://<executable>" ), launchArgs ) };
			Dev.Log.Info( "Creating Pipe" );

			var editor = new AnonymousPipeServerStream( PipeDirection.Out, HandleInheritability.Inheritable );

			Dev.Log.Info( $"[SERVER] Current Transmission Mode: {editor.TransmissionMode}." );

			// Pass the client process a handle to the server.
			game.StartInfo.Arguments += " -connect " + editor.GetClientHandleAsString();

			Dev.Log.Info( game.StartInfo.Arguments );
			game.StartInfo.UseShellExecute = false;
			game.Start();

			editor.DisposeLocalCopyOfClientHandle();

			try
			{
				// Read Input from Editor
				Writer = new( editor );

				Writer.AutoFlush = true;
				Write( $"Connected to {Process.GetCurrentProcess().ProcessName}" );
				editor.WaitForPipeDrain();
			}
			catch ( IOException e )
			{
				// Catch the IOException that is raised if the pipe is broken or disconnected
				Dev.Log.Error( $"[SERVER] Error: {e.Message}" );
			}

			game.WaitForExit();

			Writer.Dispose();
			editor.Dispose();

			game.Close();

			Writer = null;

			Dev.Log.Info( "[SERVER] Client quit. Server terminating." );
		}

		// Game [Client]

		private static void Client( string pipeHandle )
		{
			Dev.Log.Info( $"Connecting to pipe {pipeHandle}" );

			// Open Pipe to Server
			using PipeStream game = new AnonymousPipeClientStream( PipeDirection.In, pipeHandle );
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

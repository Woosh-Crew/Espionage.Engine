using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Commands
{
	/// <summary> Attribute Command Provider caches commands based off an attribute </summary>
	/// <typeparam name="T"> Attribute, should also have interface ICommandCreator </typeparam>
	internal class AttributeCommandProvider<T> : ICommandProvider where T : Attribute
	{
		//
		// Commands
		private ICommandInvoker _invoker;
		public IReadOnlyCollection<Command> All => _invoker.All;

		//
		// History
		private static HashSet<string> _history = new HashSet<string>();
		public IReadOnlyCollection<string> History => _history;

		public AttributeCommandProvider()
		{
			_invoker = new SimpleCommandInvoker();
		}

		public AttributeCommandProvider( ICommandInvoker invoker )
		{
			_invoker = invoker;
		}

		//
		// Provider
		//

		public Task Initialize()
		{
			return Task.Run( () =>
				{
					// Get every CmdAttribute using Linq
					var types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany( e => e.GetTypes()
										.SelectMany( e => e.GetMembers( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
										.Where( e => e.IsDefined( typeof( T ) ) ) ) );

					foreach ( var info in types )
					{
						foreach ( var item in (info.GetCustomAttribute<T>() as ICommandCreator).Create( info ) )
							Add( item );
					}
				} );
		}

		public void Add( Command command )
		{
			_invoker.Add( command );
		}

		public void Invoke( string command, string[] args )
		{
			_invoker.Invoke( command, args );

			// Add to history stack, for use later
			_history.Add( $"{command} {string.Join( ' ', args )}" );
		}

		public void LaunchArgs( string arg )
		{

		}
	}
}

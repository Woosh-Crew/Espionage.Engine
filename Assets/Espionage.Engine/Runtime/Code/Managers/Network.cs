using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Espionage.Engine
{
	public class Network : NetworkManager
	{
		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterSceneLoad )]
		private static void Initialization()
		{
			var manager = new GameObject( "Espionage.Engine.Network" ).AddComponent<Network>();
			manager.gameObject.AddComponent<NetworkManagerHUD>();
			manager.transport = manager.gameObject.AddComponent<kcp2k.KcpTransport>();
		}

		public override void OnClientConnect()
		{
			// Register all ILibrary, so Clients can spawn them
			foreach ( var item in Library.GetAll() )
				Library.RegisterEntity( item );

			base.OnClientConnect();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();

			Library.Creator.Create<TestEntity>();
		}

		public override void OnServerReady( NetworkConnection conn )
		{
			base.OnServerReady( conn );

			var newEntity = new GameObject( typeof( Client ).FullName ).AddComponent<Client>();
			NetworkServer.AddPlayerForConnection( conn, newEntity.gameObject, newEntity.ClassInfo.Id );
		}
	}
}

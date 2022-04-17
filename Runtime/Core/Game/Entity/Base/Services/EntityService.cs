namespace Espionage.Engine.Services
{
	internal class EntityService : Service
	{
		public override void OnUpdate()
		{
			foreach ( var entity in Entity.All )
			{
				entity?.Thinking.Run();
			}
		}
	}
}

namespace Espionage.Engine.Services
{
	internal class EntityService : Service
	{
		public override void OnUpdate()
		{
			for ( var index = 0; index < Entity.All.Count; index++ )
			{
				var entity = Entity.All[index];
				entity.Thinking.Think();
			}
		}
	}
}

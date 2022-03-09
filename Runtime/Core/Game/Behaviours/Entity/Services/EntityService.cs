namespace Espionage.Engine.Services
{
	internal class EntityService : Service
	{
		public override void OnUpdate()
		{
			for ( var index = 0; index < Entity.All.Count; index++ )
			{
				var entity = Entity.All[index];

				if ( entity == null )
				{
					continue;
				}

				var timeSince = entity.timeSinceLastThink;

				// If we can't think, don't think.
				if ( entity.nextThink == 0 || !(timeSince > entity.nextThink) )
				{
					continue;
				}

				entity.nextThink = 0;

				entity.Think( timeSince );
				entity.timeSinceLastThink = 0;
			}
		}
	}
}

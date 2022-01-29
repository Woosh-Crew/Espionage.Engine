namespace Espionage.Engine
{
	public interface ICamera
	{
		void Build( ref Tripod.Setup camSetup );
		
		void Activated();
		void Deactivated();
	}
}

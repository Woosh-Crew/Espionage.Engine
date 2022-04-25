using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public interface IAsset : ILibrary
	{
		/// <summary> Where did this asset come from? </summary>
		Resource Resource { set; }

		/// <summary>
		/// Gets called on the main asset when setting up the
		///  resource. Use this for assigning file providers
		///, or mutating components.
		/// </summary>
		/// <param name="path"></param>
		void Setup( Pathing path );

		void Load();
		void Unload();


		/// <summary>
		/// Clones the asset for use in instances, you can
		/// return itself to make it not use instances.
		/// </summary>
		IAsset Clone();
	}
}

namespace Espionage.Engine
{
	/// <summary>
	/// Stores class information, used for networking, meta data, etc.
	/// Which can be accessed in the <see cref="Library"/>.<see cref="Library.Database"/>
	/// </summary>
	public interface ILibrary
	{
		/// <summary>
		/// ClassInfo holds this classes meta data, that is stored in the Library Cache.
		/// Library stores all Meta Data relating to this current class,
		/// such as Functions, Properties, Groups, Components, etc.
		/// </summary>
		Library ClassInfo { get; }
	}
}

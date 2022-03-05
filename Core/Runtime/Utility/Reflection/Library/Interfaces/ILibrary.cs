namespace Espionage.Engine
{
	/// <summary>
	/// Stores class information, used for networking, meta data, etc.
	/// Which can be accessed in the <see cref="Library"/>.<see cref="Library.Database"/>
	/// </summary>
	public interface ILibrary
	{
		/// <summary>
		/// The Library reference for this class.
		/// </summary>
		Library ClassInfo { get; }
	}
}

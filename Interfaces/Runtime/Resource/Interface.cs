using System;
using Espionage.Engine.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Interfaces
{
	[CreateAssetMenu, Group( "Interfaces" ), File( Extension = "ui" )]
	public class Interface : Asset<VisualTreeAsset> { }
}

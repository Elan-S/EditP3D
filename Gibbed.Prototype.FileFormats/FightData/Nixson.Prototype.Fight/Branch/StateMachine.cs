using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Branch
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownBranch(BranchHash.StateMachine)]
	public class StateMachine : BaseBranch
	{
		public BaseProperty InitialTracks { get; set; } = new PropertyTrackGroup(PropertyHash.InitialTracks);
		public BaseProperty Functions { get; set; } = new PropertyTrackGroup(PropertyHash.Functions);
		public BaseProperty ExitTracks { get; set; } = new PropertyTrackGroup(PropertyHash.ExitTracks);
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.InitialTracks);
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.Functions);
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.ExitTracks);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.InitialTracks = BaseProperty.DeserializeTrackProperty(game, input, endianess, PropertyHash.InitialTracks);
			this.Functions = BaseProperty.DeserializeTrackProperty(game, input, endianess, PropertyHash.Functions);
			this.ExitTracks = BaseProperty.DeserializeTrackProperty(game, input, endianess, PropertyHash.ExitTracks);
		}
	}
}

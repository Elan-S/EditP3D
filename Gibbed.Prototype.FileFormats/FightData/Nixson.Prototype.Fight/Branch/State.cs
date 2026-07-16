using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Branch
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownBranch(BranchHash.State)]
	public class State : BaseBranch
	{
		public PropertyTrackGroup EnterTracks { get; set; } = new PropertyTrackGroup(PropertyHash.EnterTracks);
		public PropertyTrackGroup ExitTracks { get; set; } = new PropertyTrackGroup(PropertyHash.ExitTracks);
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.EnterTracks);
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.ExitTracks);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.EnterTracks = BaseProperty.DeserializeTrackProperty(game, input, endianess, PropertyHash.EnterTracks);
			this.ExitTracks = BaseProperty.DeserializeTrackProperty(game, input, endianess, PropertyHash.ExitTracks);
		}
		public enum StateTypeHash : ulong
		{
			Default,
			Complete = 14580388344362222301UL,
			Failed = 10329798022809230167UL
		}
	}
}

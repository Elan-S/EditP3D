using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Branch
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownBranch(16414221063871377845UL)]
	public class Transition : BaseBranch
	{
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.StateRef.Serialize(output, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(game, output, endianess, this.Conditions);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.StateRef = new BranchReference(input, endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(game, input, endianess, PropertyHash.Conditions);
		}
		public BranchReference StateRef = new BranchReference();
	}
}

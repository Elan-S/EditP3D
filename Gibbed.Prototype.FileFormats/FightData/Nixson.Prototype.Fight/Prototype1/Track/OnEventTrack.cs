using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12465713598218928841UL)]
	public class OnEventTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public float HookTime { get; set; }
		public int EventMask { get; set; }
		public bool InitialTest { get; set; }
		public BranchReference BranchRef { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueF32(this.HookTime, endianess);
			output.WriteValueS32(this.EventMask);
			output.WriteValueB32(this.InitialTest, endianess);
			this.BranchRef.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.HookTime = input.ReadValueF32(endianess);
			this.EventMask = input.ReadValueS32(endianess);
			this.InitialTest = input.ReadValueB32(endianess);
			this.BranchRef = new BranchReference(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
	}
}

using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.TryOpenStrafeRun)]
	public class TryOpenStrafeRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MaxHeightPath { get; set; }
		public float MaxHeightEnd { get; set; }
		public float MinHeightWithTarget { get; set; }
		public float EndHigherThanTarget { get; set; }
		public float MaxHeightStartWithTarget { get; set; }
		public float MinHeightStartWithTarget { get; set; }
		public int MaxLength { get; set; }
		public int MinLength { get; set; }
		public int MinLengthAfterTarget { get; set; }
		public float MinLengthBeforeTarget { get; set; }
		public float MaxHeightToIgnoreStartLength { get; set; }
		public float FreeRadiusStart { get; set; }
		public float FreeRadiusPath { get; set; }
		public float FreeRadiusEnd { get; set; }
		public BranchReference IfFound { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MaxHeightPath, endianess);
			output.WriteValueF32(this.MaxHeightEnd, endianess);
			output.WriteValueF32(this.MinHeightWithTarget, endianess);
			output.WriteValueF32(this.EndHigherThanTarget, endianess);
			output.WriteValueF32(this.MaxHeightStartWithTarget, endianess);
			output.WriteValueF32(this.MinHeightStartWithTarget, endianess);
			output.WriteValueS32(this.MaxLength, endianess);
			output.WriteValueS32(this.MinLength, endianess);
			output.WriteValueS32(this.MinLengthAfterTarget, endianess);
			output.WriteValueF32(this.MinLengthBeforeTarget, endianess);
			output.WriteValueF32(this.MaxHeightToIgnoreStartLength, endianess);
			output.WriteValueF32(this.FreeRadiusStart, endianess);
			output.WriteValueF32(this.FreeRadiusPath, endianess);
			output.WriteValueF32(this.FreeRadiusEnd, endianess);
			this.IfFound.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MaxHeightPath = input.ReadValueF32(endianess);
			this.MaxHeightEnd = input.ReadValueF32(endianess);
			this.MinHeightWithTarget = input.ReadValueF32(endianess);
			this.EndHigherThanTarget = input.ReadValueF32(endianess);
			this.MaxHeightStartWithTarget = input.ReadValueF32(endianess);
			this.MinHeightStartWithTarget = input.ReadValueF32(endianess);
			this.MaxLength = input.ReadValueS32(endianess);
			this.MinLength = input.ReadValueS32(endianess);
			this.MinLengthAfterTarget = input.ReadValueS32(endianess);
			this.MinLengthBeforeTarget = input.ReadValueF32(endianess);
			this.MaxHeightToIgnoreStartLength = input.ReadValueF32(endianess);
			this.FreeRadiusStart = input.ReadValueF32(endianess);
			this.FreeRadiusPath = input.ReadValueF32(endianess);
			this.FreeRadiusEnd = input.ReadValueF32(endianess);
			this.IfFound.Deserialize(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
	}
}

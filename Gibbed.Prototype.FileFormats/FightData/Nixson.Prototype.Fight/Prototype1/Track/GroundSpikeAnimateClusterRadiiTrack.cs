using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17195525113194113875UL)]
	public class GroundSpikeAnimateClusterRadiiTrack : P1Track
	{
		public float Delay { get; set; }
		public GroundSpikeAnimateClusterRadiiTrack.VelocityType SpawnRadiusVelocityType { get; set; }
		public float SpawnEndRadiusScalar { get; set; }
		public GroundSpikeAnimateClusterRadiiTrack.VelocityType SpreadRadiusVelocityType { get; set; }
		public float SpreadEndRadiusScalar { get; set; }
		public GroundSpikeAnimateClusterRadiiTrack.VelocityType OffsetVelocityType { get; set; }
		public float OffsetAtEnd { get; set; }
		public float AnimationTimeMin { get; set; }
		public float AnimationTimeMax { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.Delay, endianess);
			BaseProperty.SerializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(output, endianess, this.SpawnRadiusVelocityType);
			output.WriteValueF32(this.SpawnEndRadiusScalar, endianess);
			BaseProperty.SerializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(output, endianess, this.SpreadRadiusVelocityType);
			output.WriteValueF32(this.SpreadEndRadiusScalar, endianess);
			BaseProperty.SerializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(output, endianess, this.OffsetVelocityType);
			output.WriteValueF32(this.OffsetAtEnd, endianess);
			output.WriteValueF32(this.AnimationTimeMin, endianess);
			output.WriteValueF32(this.AnimationTimeMax, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Delay = input.ReadValueF32(endianess);
			this.SpawnRadiusVelocityType = BaseProperty.DeserializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(input, endianess);
			this.SpawnEndRadiusScalar = input.ReadValueF32(endianess);
			this.SpreadRadiusVelocityType = BaseProperty.DeserializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(input, endianess);
			this.SpreadEndRadiusScalar = input.ReadValueF32(endianess);
			this.OffsetVelocityType = BaseProperty.DeserializePropertyEnum<GroundSpikeAnimateClusterRadiiTrack.VelocityType>(input, endianess);
			this.OffsetAtEnd = input.ReadValueF32(endianess);
			this.AnimationTimeMin = input.ReadValueF32(endianess);
			this.AnimationTimeMax = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
		public enum VelocityType : ulong
		{
			OffshootsInactive = 4778735518721658666UL,
			OffshootsActive = 7949029966231454343UL,
			AccDecExponential = 174932811517141887UL,
			AccDec = 4489517269923741034UL,
			DecelerateExponential = 8982505683990869611UL,
			Decelerate = 9186242548299919630UL,
			AccelerateExponential = 11292729051863009090UL,
			Accelerate = 15267335328970854751UL,
			Linear = 4206860010037910535UL
		}
	}
}

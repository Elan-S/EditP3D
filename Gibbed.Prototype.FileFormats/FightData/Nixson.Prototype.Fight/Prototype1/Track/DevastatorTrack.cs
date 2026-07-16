using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13902508776967827755UL)]
	public class DevastatorTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Radius { get; set; }
		public int TargetCount { get; set; }
		public BranchReference TargetBranch { get; set; } = new BranchReference();
		public BranchReference RandomTargetBranch { get; set; } = new BranchReference();
		public int MinTargetCount { get; set; }
		public float RandomHeightArc { get; set; }
		public float RandomMinRadius { get; set; }
		public float MinHeightArcOnGround { get; set; }
		public int MinTargetCategory { get; set; }
		public int MinTargetPriority { get; set; }
		public float AttackTimeBeginStart { get; set; }
		public float AttackTimeBeginStop { get; set; }
		public bool AttackTimeBeginRandom { get; set; }
		public float AttackTimeDuration { get; set; }
		public float AttackDamage { get; set; }
		public float RandomDamage { get; set; }
		public float CameraMaxArc { get; set; }
		public int CameraMaxTargets { get; set; }
		public int CameraMinTargetCategory { get; set; }
		public int CameraMinTargetPriority { get; set; }
		public float CameraLookAtTargetArc { get; set; }
		public float CameraLookAtTargetTime { get; set; }
		public float CameraSwitchTargetTime { get; set; }
		public bool UseCharacterHeap { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueS32(this.TargetCount, endianess);
			this.TargetBranch.Serialize(output, endianess);
			this.RandomTargetBranch.Serialize(output, endianess);
			output.WriteValueS32(this.MinTargetCount, endianess);
			output.WriteValueF32(this.RandomHeightArc, endianess);
			output.WriteValueF32(this.RandomMinRadius, endianess);
			output.WriteValueF32(this.MinHeightArcOnGround, endianess);
			output.WriteValueS32(this.MinTargetCategory, endianess);
			output.WriteValueS32(this.MinTargetPriority, endianess);
			output.WriteValueF32(this.AttackTimeBeginStart, endianess);
			output.WriteValueF32(this.AttackTimeBeginStop, endianess);
			output.WriteValueB32(this.AttackTimeBeginRandom, endianess);
			output.WriteValueF32(this.AttackTimeDuration, endianess);
			output.WriteValueF32(this.AttackDamage, endianess);
			output.WriteValueF32(this.RandomDamage, endianess);
			output.WriteValueF32(this.CameraMaxArc, endianess);
			output.WriteValueS32(this.CameraMaxTargets, endianess);
			output.WriteValueS32(this.CameraMinTargetCategory, endianess);
			output.WriteValueS32(this.CameraMinTargetPriority, endianess);
			output.WriteValueF32(this.CameraLookAtTargetArc, endianess);
			output.WriteValueF32(this.CameraLookAtTargetTime, endianess);
			output.WriteValueF32(this.CameraSwitchTargetTime, endianess);
			output.WriteValueB32(this.UseCharacterHeap, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.TargetCount = input.ReadValueS32(endianess);
			this.TargetBranch = new BranchReference(input, endianess);
			this.RandomTargetBranch = new BranchReference(input, endianess);
			this.MinTargetCount = input.ReadValueS32(endianess);
			this.RandomHeightArc = input.ReadValueF32(endianess);
			this.RandomMinRadius = input.ReadValueF32(endianess);
			this.MinHeightArcOnGround = input.ReadValueF32(endianess);
			this.MinTargetCategory = input.ReadValueS32(endianess);
			this.MinTargetPriority = input.ReadValueS32(endianess);
			this.AttackTimeBeginStart = input.ReadValueF32(endianess);
			this.AttackTimeBeginStop = input.ReadValueF32(endianess);
			this.AttackTimeBeginRandom = input.ReadValueB32(endianess);
			this.AttackTimeDuration = input.ReadValueF32(endianess);
			this.AttackDamage = input.ReadValueF32(endianess);
			this.RandomDamage = input.ReadValueF32(endianess);
			this.CameraMaxArc = input.ReadValueF32(endianess);
			this.CameraMaxTargets = input.ReadValueS32(endianess);
			this.CameraMinTargetCategory = input.ReadValueS32(endianess);
			this.CameraMinTargetPriority = input.ReadValueS32(endianess);
			this.CameraLookAtTargetArc = input.ReadValueF32(endianess);
			this.CameraLookAtTargetTime = input.ReadValueF32(endianess);
			this.CameraSwitchTargetTime = input.ReadValueF32(endianess);
			this.UseCharacterHeap = input.ReadValueB32(endianess);
		}
	}
}

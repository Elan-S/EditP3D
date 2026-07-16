using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13102492510152418326UL)]
	public class MotionTrailTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Shader { get; set; }
		public float GenerateTime { get; set; }
		public float FadeInTime { get; set; }
		public float FadeOutTime { get; set; }
		public float TextureTime { get; set; }
		public float FadeTime { get; set; }
		public float FadeInTrailTime { get; set; }
		public float ShrinkTime { get; set; }
		public bool Dragged { get; set; }
		public ulong ShrinkJoint { get; set; }
		public Vector ShrinkOffset { get; set; } = new Vector();
		public int NumJoints { get; set; }
		public ulong Joint1 { get; set; }
		public Vector JointOffset1 { get; set; } = new Vector();
		public ulong Joint2 { get; set; }
		public Vector JointOffset2 { get; set; } = new Vector();
		public ulong Joint3 { get; set; }
		public Vector JointOffset3 { get; set; } = new Vector();
		public ulong Joint4 { get; set; }
		public Vector JointOffset4 { get; set; } = new Vector();
		public ulong Joint5 { get; set; }
		public Vector JointOffset5 { get; set; } = new Vector();
		public ulong Joint6 { get; set; }
		public Vector JointOffset6 { get; set; } = new Vector();
		public bool AbortWhenInterrupted { get; set; }
		public bool RunTillStop { get; set; }
		public int Slot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Shader, endianess);
			output.WriteValueF32(this.GenerateTime, endianess);
			output.WriteValueF32(this.FadeInTime, endianess);
			output.WriteValueF32(this.FadeOutTime, endianess);
			output.WriteValueF32(this.TextureTime, endianess);
			output.WriteValueF32(this.FadeTime, endianess);
			output.WriteValueF32(this.FadeInTrailTime, endianess);
			output.WriteValueF32(this.ShrinkTime, endianess);
			output.WriteValueB32(this.Dragged, endianess);
			output.WriteValueU64(this.ShrinkJoint, endianess);
			this.ShrinkOffset.Serialize(output, endianess);
			output.WriteValueS32(this.NumJoints, endianess);
			output.WriteValueU64(this.Joint1, endianess);
			this.JointOffset1.Serialize(output, endianess);
			output.WriteValueU64(this.Joint2, endianess);
			this.JointOffset2.Serialize(output, endianess);
			output.WriteValueU64(this.Joint3, endianess);
			this.JointOffset3.Serialize(output, endianess);
			output.WriteValueU64(this.Joint4, endianess);
			this.JointOffset4.Serialize(output, endianess);
			output.WriteValueU64(this.Joint5, endianess);
			this.JointOffset5.Serialize(output, endianess);
			output.WriteValueU64(this.Joint6, endianess);
			this.JointOffset6.Serialize(output, endianess);
			output.WriteValueB32(this.AbortWhenInterrupted, endianess);
			output.WriteValueB32(this.RunTillStop, endianess);
			output.WriteValueS32(this.Slot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Shader = input.ReadValueU64(endianess);
			this.GenerateTime = input.ReadValueF32(endianess);
			this.FadeInTime = input.ReadValueF32(endianess);
			this.FadeOutTime = input.ReadValueF32(endianess);
			this.TextureTime = input.ReadValueF32(endianess);
			this.FadeTime = input.ReadValueF32(endianess);
			this.FadeInTrailTime = input.ReadValueF32(endianess);
			this.ShrinkTime = input.ReadValueF32(endianess);
			this.Dragged = input.ReadValueB32(endianess);
			this.ShrinkJoint = input.ReadValueU64(endianess);
			this.ShrinkOffset = new Vector(input, endianess);
			this.NumJoints = input.ReadValueS32(endianess);
			this.Joint1 = input.ReadValueU64(endianess);
			this.JointOffset1 = new Vector(input, endianess);
			this.Joint2 = input.ReadValueU64(endianess);
			this.JointOffset2 = new Vector(input, endianess);
			this.Joint3 = input.ReadValueU64(endianess);
			this.JointOffset3 = new Vector(input, endianess);
			this.Joint4 = input.ReadValueU64(endianess);
			this.JointOffset4 = new Vector(input, endianess);
			this.Joint5 = input.ReadValueU64(endianess);
			this.JointOffset5 = new Vector(input, endianess);
			this.Joint6 = input.ReadValueU64(endianess);
			this.JointOffset6 = new Vector(input, endianess);
			this.AbortWhenInterrupted = input.ReadValueB32(endianess);
			this.RunTillStop = input.ReadValueB32(endianess);
			this.Slot = input.ReadValueS32(endianess);
		}
	}
}

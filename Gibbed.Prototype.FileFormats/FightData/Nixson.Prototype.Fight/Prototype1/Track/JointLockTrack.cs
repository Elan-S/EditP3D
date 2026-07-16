using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.JointLock)]
	public class JointLockTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong LockedJoint { get; set; }
		public JointLockTrack.TranslationLockType TranslationLock { get; set; }
		public Vector OffsetTranslation { get; set; } = new Vector();
		public Vector OffsetOrientation { get; set; } = new Vector();
		public bool AutoUpdate { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.LockedJoint, endianess);
			BaseProperty.SerializePropertyEnum<JointLockTrack.TranslationLockType>(output, endianess, this.TranslationLock);
			this.OffsetTranslation.Serialize(output, endianess);
			this.OffsetOrientation.Serialize(output, endianess);
			output.WriteValueB32(this.AutoUpdate, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.LockedJoint = input.ReadValueU64(endianess);
			this.TranslationLock = BaseProperty.DeserializePropertyEnum<JointLockTrack.TranslationLockType>(input, endianess);
			this.OffsetTranslation.Deserialize(input, endianess);
			this.OffsetOrientation.Deserialize(input, endianess);
			this.AutoUpdate = input.ReadValueB32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		public enum TranslationLockType : ulong
		{
			All = 279702787393UL,
			Y = 89UL,
			XZ = 5772786UL,
			None = 22018610510307286UL
		}
	}
}

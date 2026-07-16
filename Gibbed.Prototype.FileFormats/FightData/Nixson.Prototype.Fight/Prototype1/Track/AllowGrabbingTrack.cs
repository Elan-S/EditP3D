using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(9600416208127396491UL)]
	public class AllowGrabbingTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public AllowGrabbingTrack.GrabStart OnBegin { get; set; }
		public AllowGrabbingTrack.GrabEnd OnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			BaseProperty.SerializePropertyEnum<AllowGrabbingTrack.GrabStart>(output, endianess, this.OnBegin);
			BaseProperty.SerializePropertyEnum<AllowGrabbingTrack.GrabEnd>(output, endianess, this.OnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.OnBegin = BaseProperty.DeserializePropertyEnum<AllowGrabbingTrack.GrabStart>(input, endianess);
			this.OnEnd = BaseProperty.DeserializePropertyEnum<AllowGrabbingTrack.GrabEnd>(input, endianess);
		}
		public enum GrabStart : ulong
		{
			DoNothing = 16252691783617167704UL,
			Allow = 4586725638409169159UL,
			Forbid = 9905570320066967050UL
		}
		public enum GrabEnd : ulong
		{
			Restore = 8128040577595920368UL,
			Allow = 4586725638409169159UL,
			Forbid = 9905570320066967050UL
		}
	}
}

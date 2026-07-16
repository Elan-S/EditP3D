using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17207175970774856696UL)]
	public class HeliBoundaryStatusCondition : P1Condition
	{
		public HeliBoundaryStatusCondition.HeliBoundaryType HeliBoundaryStatus { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<HeliBoundaryStatusCondition.HeliBoundaryType>(output, endianess, this.HeliBoundaryStatus);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.HeliBoundaryStatus = BaseProperty.DeserializePropertyEnum<HeliBoundaryStatusCondition.HeliBoundaryType>(input, endianess);
		}
		public enum HeliBoundaryType : ulong
		{
			Ok = 4909933728871709489UL,
			Warn = 18283616285219566495UL,
			Kill = 18275712200623121687UL
		}
	}
}

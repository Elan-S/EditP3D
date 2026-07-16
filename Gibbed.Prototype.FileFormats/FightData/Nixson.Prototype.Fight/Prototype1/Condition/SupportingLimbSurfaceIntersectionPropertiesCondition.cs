using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(14188159592347492064UL)]
	public class SupportingLimbSurfaceIntersectionPropertiesCondition : P1Condition
	{
		public Collidables TestBits { get; set; }
		public SupportingLimbSurfaceIntersectionPropertiesCondition.MatchType MatchAgainst { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.TestBits);
			BaseProperty.SerializePropertyEnum<SupportingLimbSurfaceIntersectionPropertiesCondition.MatchType>(output, endianess, this.MatchAgainst);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TestBits = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
			this.MatchAgainst = BaseProperty.DeserializePropertyEnum<SupportingLimbSurfaceIntersectionPropertiesCondition.MatchType>(input, endianess);
		}
		public enum MatchType : ulong
		{
			any = 417410176246UL,
			all = 417410307425UL
		}
	}
}

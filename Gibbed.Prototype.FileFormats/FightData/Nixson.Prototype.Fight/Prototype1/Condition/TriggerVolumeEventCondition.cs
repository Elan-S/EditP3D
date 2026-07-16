using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	public class TriggerVolumeEventCondition : P1Condition
	{
		public TriggerVolumeEventCondition.TriggerCollisionType Type { get; set; }
		public string TriggerVolumeName { get; set; }
		public int TriggerVolumeAttributesTags { get; set; }
		public string TriggerVolumeAttributesTagsTriggerAttributeKey1 { get; set; }
		public string TriggerVolumeAttributesTagsTriggerAttributeValue1 { get; set; }
		public string TriggerVolumeAttributesTagsTriggerAttributeKey2 { get; set; }
		public string TriggerVolumeAttributesTagsTriggerAttributeValue2 { get; set; }
		public string TriggerVolumeAttributesTagsTriggerTagList { get; set; }
		public string TouchName { get; set; }
		public int TouchAttributesTags { get; set; }
		public string TouchAttributesTagsTouchAttributeKey1 { get; set; }
		public string TouchAttributesTagsTouchAttributeValue1 { get; set; }
		public string TouchAttributesTagsTouchAttributeKey2 { get; set; }
		public string TouchAttributesTagsTouchAttributeValue2 { get; set; }
		public string TouchAttributesTagsTouchTagList { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<TriggerVolumeEventCondition.TriggerCollisionType>(output, endianess, this.Type);
			output.WriteStringAlignedU32(this.TriggerVolumeName, endianess);
			output.WriteValueS32(this.TriggerVolumeAttributesTags, endianess);
			output.WriteStringAlignedU32(this.TriggerVolumeAttributesTagsTriggerAttributeKey1, endianess);
			output.WriteStringAlignedU32(this.TriggerVolumeAttributesTagsTriggerAttributeValue1, endianess);
			output.WriteStringAlignedU32(this.TriggerVolumeAttributesTagsTriggerAttributeKey2, endianess);
			output.WriteStringAlignedU32(this.TriggerVolumeAttributesTagsTriggerAttributeValue2, endianess);
			output.WriteStringAlignedU32(this.TriggerVolumeAttributesTagsTriggerTagList, endianess);
			output.WriteStringAlignedU32(this.TouchName, endianess);
			output.WriteValueS32(this.TouchAttributesTags, endianess);
			output.WriteStringAlignedU32(this.TouchAttributesTagsTouchAttributeKey1, endianess);
			output.WriteStringAlignedU32(this.TouchAttributesTagsTouchAttributeValue1, endianess);
			output.WriteStringAlignedU32(this.TouchAttributesTagsTouchAttributeKey2, endianess);
			output.WriteStringAlignedU32(this.TouchAttributesTagsTouchAttributeValue2, endianess);
			output.WriteStringAlignedU32(this.TouchAttributesTagsTouchTagList, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<TriggerVolumeEventCondition.TriggerCollisionType>(input, endianess);
			this.TriggerVolumeName = input.ReadStringAlignedU32(endianess);
			this.TriggerVolumeAttributesTags = input.ReadValueS32(endianess);
			this.TriggerVolumeAttributesTagsTriggerAttributeKey1 = input.ReadStringAlignedU32(endianess);
			this.TriggerVolumeAttributesTagsTriggerAttributeValue1 = input.ReadStringAlignedU32(endianess);
			this.TriggerVolumeAttributesTagsTriggerAttributeKey2 = input.ReadStringAlignedU32(endianess);
			this.TriggerVolumeAttributesTagsTriggerAttributeValue2 = input.ReadStringAlignedU32(endianess);
			this.TriggerVolumeAttributesTagsTriggerTagList = input.ReadStringAlignedU32(endianess);
			this.TouchName = input.ReadStringAlignedU32(endianess);
			this.TouchAttributesTags = input.ReadValueS32(endianess);
			this.TouchAttributesTagsTouchAttributeKey1 = input.ReadStringAlignedU32(endianess);
			this.TouchAttributesTagsTouchAttributeValue1 = input.ReadStringAlignedU32(endianess);
			this.TouchAttributesTagsTouchAttributeKey2 = input.ReadStringAlignedU32(endianess);
			this.TouchAttributesTagsTouchAttributeValue2 = input.ReadStringAlignedU32(endianess);
			this.TouchAttributesTagsTouchTagList = input.ReadStringAlignedU32(endianess);
		}
		public enum TriggerCollisionType : ulong
		{
			Either = 7792769599423272283UL,
			Enter = 4872555661335756302UL,
			Exit = 19477321536111832UL
		}
	}
}

using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17401459951903935692UL)]
	public class GroundSpikeTestStateCondition : P1Condition
	{
		public GroundSpikeTestStateCondition.GroundSpikeTestType TestType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<GroundSpikeTestStateCondition.GroundSpikeTestType>(output, endianess, this.TestType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TestType = BaseProperty.DeserializePropertyEnum<GroundSpikeTestStateCondition.GroundSpikeTestType>(input, endianess);
		}
		public enum GroundSpikeTestType : ulong
		{
			Descending = 605466029595790150UL,
			AllSpawnedMotionIncomplete = 8429933045917250773UL,
			AllSpawnedMotionComplete = 3617334664706998148UL,
			ClusterMotionIncomplete = 50436313802653084UL,
			ClusterMotionComplete = 13785681490621827537UL,
			ClusterInactive = 11364860035905269777UL,
			ClusterActive = 10578384007259670492UL,
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

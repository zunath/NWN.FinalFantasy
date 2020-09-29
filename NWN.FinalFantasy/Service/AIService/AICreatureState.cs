namespace NWN.FinalFantasy.Service.AIService
{
    public class AICreatureState
    {
        public AICreatureState(string instructionSetId)
        {
            InstructionSetId = instructionSetId;
        }

        public string InstructionSetId { get; set; }
        public double ProcessTime { get; set; }
    }
}

namespace HuffmanCodes
{
    internal class HuffmanNode
    {
        public string Word { get; set; }
        public double Frequency { get; set; }
        public List<HuffmanNode> Children { get; set; } = new List<HuffmanNode>();
    }
}

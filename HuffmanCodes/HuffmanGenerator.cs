namespace HuffmanCodes
{
    internal class HuffmanGenerator
    {
        private Dictionary<string, double> wordFrequencies;

        public HuffmanGenerator(Dictionary<string, double> frequencies)
        {
            wordFrequencies = frequencies;
        }

        public HuffmanNode BuildHuffmanTree(int blockSize)
        {
            var priorityQueue = new SortedDictionary<double, List<HuffmanNode>>();

            var sortedWordFrequencies = wordFrequencies.OrderBy(pair => pair.Key);

            var remainder = wordFrequencies.Count % blockSize;
            var blockCounter = 0;

            foreach (var entry in sortedWordFrequencies)
            {
                var node = new HuffmanNode { Word = entry.Key, Frequency = entry.Value, Children = new List<HuffmanNode>() };
                priorityQueue[entry.Value].Add(node);

                if (++blockCounter == remainder)
                {
                    blockCounter = 0;
                    priorityQueue = new SortedDictionary<double, List<HuffmanNode>>(priorityQueue);
                }
            }
       
            while (priorityQueue.Count > 1)
            {
                var pair = priorityQueue.First();
                var freq = pair.Key;
                var nodes = pair.Value;

                if (nodes.Count == 1)
                {
                    priorityQueue.Remove(freq);
                }
                else
                {
                    priorityQueue[freq] = nodes.Skip(1).ToList();
                }

                var newNode = new HuffmanNode
                {
                    Word = null,
                    Frequency = freq,
                    Children = nodes.ToList()
                };

                var newFreq = freq + priorityQueue.Keys.FirstOrDefault();
                if (!priorityQueue.ContainsKey(newFreq))
                {
                    priorityQueue[newFreq] = new List<HuffmanNode>();
                }

                priorityQueue[newFreq].Add(newNode);

                if (++blockCounter == blockSize && priorityQueue.Count > 1)
                {
                    blockCounter = 0;
                    priorityQueue = new SortedDictionary<double, List<HuffmanNode>>(priorityQueue);
                }
            }

            return priorityQueue.First().Value.First();
        }

        public Dictionary<string, string> GenerateHuffmanCodes(HuffmanNode root, string currentCode = "")
        {
            var codes = new Dictionary<string, string>();

            if (root != null)
            {
                if (root.Word != null)
                {
                    codes[root.Word] = currentCode;
                }
                else
                {
                    foreach (var child in root.Children)
                    {
                        codes = codes
                            .Concat(GenerateHuffmanCodes(child, currentCode + "0"))
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }
            }

            return codes;
        }
    }
}

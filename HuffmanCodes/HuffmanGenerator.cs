namespace HuffmanCodes
{
    internal class HuffmanGenerator
    {
        private Dictionary<string, double> wordFrequencies;

        // Конструктор класса HuffmanGenerator.
        // frequencies: Словарь, содержащий слова и их частоты появления.
        public HuffmanGenerator(Dictionary<string, double> frequencies)
        {
            wordFrequencies = frequencies;
        }

        // Построение дерева Хаффмана и упорядоченного списка узлов.
        // blockSize: Размер блока для группировки узлов при слиянии.
        // sortedWordFrequencies: Выходной параметр, содержит упорядоченный список узлов.
        public void BuildHuffmanTree(int blockSize, out List<HuffmanNode> sortedWordFrequencies)
        {
            sortedWordFrequencies = new List<HuffmanNode>();
            var priorityQueue = new List<KeyValuePair<double, HuffmanNode>>();
            var remainder = wordFrequencies.Count % blockSize;

            // Создание упорядоченного списка узлов.
            foreach (var entry in wordFrequencies.OrderByDescending(pair => int.Parse(pair.Key)))
            {
                var node = new HuffmanNode { Word = entry.Key, Frequency = entry.Value };
                sortedWordFrequencies.Add(node);
                priorityQueue.Add(new KeyValuePair<double, HuffmanNode>(node.Frequency, node));
            }

            // Обработка остатка от деления размера списка на размер блока.
            if (remainder != 0)
            {
                var newNode = new HuffmanNode { Word = "", Frequency = 0 };
                for (var i = 0; i < remainder; i++)
                {
                    var node = priorityQueue[0].Value;
                    var freq = priorityQueue[0].Key;
                    priorityQueue.Remove(priorityQueue.First());
                    node.Code = (blockSize - i - 1).ToString();
                    node.Children.Add(newNode);
                    newNode.Frequency += freq;
                    newNode.Word += i < remainder - 1 ? node.Word + "and" : node.Word;
                }
                AddToPriorityQueue(priorityQueue, newNode.Frequency, newNode);
            }

            // Основной цикл слияния узлов.
            if (priorityQueue.Count >= blockSize)
            {
                while (priorityQueue.Count > 1)
                {
                    var newNode = new HuffmanNode { Word = "", Frequency = 0 };
                    for (var i = 0; i < blockSize; i++)
                    {
                        var node = priorityQueue[0].Value;
                        var freq = priorityQueue[0].Key;
                        priorityQueue.Remove(priorityQueue.First());
                        node.Code = (blockSize - i - 1).ToString();
                        node.Children.Add(newNode);
                        newNode.Frequency += freq;
                        newNode.Word += i < blockSize - 1 ? node.Word + "and" : node.Word;
                    }
                    AddToPriorityQueue(priorityQueue, newNode.Frequency, newNode);
                }
            }
        }

        // Добавление узла в приоритетную очередь.
        private void AddToPriorityQueue(List<KeyValuePair<double, HuffmanNode>> priorityQueue, double frequency, HuffmanNode node)
        {
            int index = 0;

            // Если очередь пуста, добавляем новый узел.
            if (priorityQueue.Count == 0)
            {
                priorityQueue.Add(new KeyValuePair<double, HuffmanNode>(node.Frequency, node));
            }
            else
            {
                // Находим индекс, на который нужно вставить новый элемент.
                while (index < priorityQueue.Count && frequency > priorityQueue[index].Key)
                {
                    index++;
                }

                // Вставляем новый элемент на найденный индекс.
                priorityQueue.Insert(index, new KeyValuePair<double, HuffmanNode>(frequency, node));
            }
        }

        // Генерация кодов Хаффмана для узла и его дочерних узлов.
        public string GenerateHuffmanCodes(HuffmanNode node)
        {
            string codes;
            codes = node.Code;
            foreach (var child in node.Children)
            {
                if (child.Code != null)
                    codes += GenerateHuffmanCodes(child);
            }
            return codes;
        }
    }
}

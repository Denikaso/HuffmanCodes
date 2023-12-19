using HuffmanCodes;
using System.Globalization;

Console.Write("Введите размер блока: ");
if (!int.TryParse(Console.ReadLine(), out int blockSize) || blockSize <= 0)
{
    Console.WriteLine("Некорректный размер блока.");
    return;
}

Dictionary<string, double> wordFrequencies = ReadFrequenciesFromFile("C:\\Уник\\ТиК\\HuffmanCodes\\frequencies.txt", blockSize);

if (Math.Abs(wordFrequencies.Values.Sum() - 1.0) > 0.0001)
{
    Console.WriteLine("Ошибка: Сумма частот не равна 1.0");
    return;
}

var huffmanGenerator = new HuffmanGenerator(wordFrequencies);
List<HuffmanNode> sortedWordFrequencies = new List<HuffmanNode>();
huffmanGenerator.BuildHuffmanTree(blockSize, out sortedWordFrequencies);
double averageLength = 0;
foreach (HuffmanNode node in sortedWordFrequencies)
{
    string code = huffmanGenerator.GenerateHuffmanCodes(node);
    string reversedCode = new string(code.Reverse().ToArray());

    averageLength += reversedCode.Length * node.Frequency;

    Console.Write("Код для слова ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{node.Word}: ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(reversedCode);
}

Console.Write("Средняя длина слова: ");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write($"{averageLength:F2}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();


Dictionary<string, double> ReadFrequenciesFromFile(string filePath, int blockSize)
{
    var frequencies = new Dictionary<string, double>();

    using (var reader = new StreamReader(filePath))
    {
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var parts = line.Split(':');
            var word = parts[0].Trim();
            var frequency = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            frequencies.Add(word, frequency);
        }
    }
    return frequencies;
}

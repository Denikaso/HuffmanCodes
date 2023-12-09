using HuffmanCodes;
using System.Globalization;

Console.Write("Введите размер блока: ");
if (!int.TryParse(Console.ReadLine(), out int blockSize) || blockSize <= 0)
{
    Console.WriteLine("Некорректный размер блока.");
    return;
}

Dictionary<string, double> wordFrequencies = ReadFrequenciesFromFile("frequencies.txt", blockSize);

if (Math.Abs(wordFrequencies.Values.Sum() - 1.0) > 0.0001)
{
    Console.WriteLine("Ошибка: Сумма частот не равна 1.0");
    return;
}

var huffmanGenerator = new HuffmanGenerator(wordFrequencies);

var huffmanTree = huffmanGenerator.BuildHuffmanTree(blockSize);

var huffmanCodes = huffmanGenerator.GenerateHuffmanCodes(huffmanTree);


foreach (var entry in huffmanCodes)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

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

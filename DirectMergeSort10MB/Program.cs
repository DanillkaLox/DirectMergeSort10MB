using System.Diagnostics;

class ExternalMergeSort
{
    static void Main()
    {
        string inputFilePath = "A.txt";
        string fileBPath = "B.txt";
        string fileCPath = "C.txt";
        
        GenerateFile(inputFilePath, 10 * 1024 * 1024);
        
        int numberOfElements = File.ReadLines(inputFilePath).Count();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        MergeSort(inputFilePath, fileBPath, fileCPath, numberOfElements);
        
        stopwatch.Stop();
        Console.WriteLine($"Сортування завершено за {stopwatch.Elapsed.TotalSeconds} секунд.");
    }

    static void GenerateFile(string filePath, int sizeInBytes)
    {
        Random random = new Random();
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            int bytesWritten = 0;
            while (bytesWritten < sizeInBytes)
            {
                int number = random.Next(0, 10000);
                string line = number.ToString();
                writer.WriteLine(line);
                bytesWritten += line.Length + Environment.NewLine.Length;
            }
        }
        Console.WriteLine("Файл згенеровано.");
    }

    static void MergeSort(string inputFilePath, string fileBPath, string fileCPath, int n)
    {
        bool sorted = false;
        int step = 1;
        
        while (!sorted)
        {
            SplitFile(inputFilePath, fileBPath, fileCPath, step);
            
            sorted = MergeFiles(inputFilePath, fileBPath, fileCPath, step);

            step *= 2;
        }
    }

    static void SplitFile(string inputFilePath, string fileBPath, string fileCPath, int step)
    {
        using (StreamReader readerA = new StreamReader(inputFilePath))
        using (StreamWriter writerB = new StreamWriter(fileBPath))
        using (StreamWriter writerC = new StreamWriter(fileCPath))
        {
            bool writeToB = true;
            int count = 0;
            
            while (!readerA.EndOfStream)
            {
                string line = readerA.ReadLine();
                if (writeToB)
                    writerB.WriteLine(line);
                else
                    writerC.WriteLine(line);
                
                count++;
                if (count == step)
                {
                    writeToB = !writeToB;
                    count = 0;
                }
            }
        }
    }

    static bool MergeFiles(string inputFilePath, string fileBPath, string fileCPath, int step)
    {
        using (StreamReader readerB = new StreamReader(fileBPath))
        using (StreamReader readerC = new StreamReader(fileCPath))
        using (StreamWriter writerA = new StreamWriter(inputFilePath))
        {
            string lineB = readerB.ReadLine();
            string lineC = readerC.ReadLine();

            while (lineB != null || lineC != null)
            {
                int countB = 0, countC = 0;

                while (countB < step && countC < step && lineB != null && lineC != null)
                {
                    if (int.Parse(lineB) < int.Parse(lineC))
                    {
                        writerA.WriteLine(lineB);
                        lineB = readerB.ReadLine();
                        countB++;
                    }
                    else
                    {
                        writerA.WriteLine(lineC);
                        lineC = readerC.ReadLine();
                        countC++;
                    }
                }

                while (countB < step && lineB != null)
                {
                    writerA.WriteLine(lineB);
                    lineB = readerB.ReadLine();
                    countB++;
                }

                while (countC < step && lineC != null)
                {
                    writerA.WriteLine(lineC);
                    lineC = readerC.ReadLine();
                    countC++;
                }
            }
        }
        
        return step >= File.ReadLines(inputFilePath).Count()/2;
    }
}

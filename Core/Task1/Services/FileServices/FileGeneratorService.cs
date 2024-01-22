using Core.Task1.Services.FileServices.Abstract;
using Core.Task1.Services.FileServices.Exceptions;
using Core.Task1.Utilities.Generators.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices
{
    public class FileGeneratorService : IFileGeneratorService
    {
        private readonly IDataGenerator generator;

        public FileGeneratorService(IDataGenerator generator)
        {
            this.generator = generator;
        }

        public async Task CreateFilesAsync(string path, int numberOfFiles, int numberOfLines)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            await Task.Run(() =>
            {
                string filePath = string.Empty;
                Parallel.For(0, numberOfFiles, i =>
                {
                    try
                    {
                        filePath = Path.Combine(path, $"file{i}.txt");
                        CreateFile(filePath, numberOfLines);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                });
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }

        private void CreateFile(string filePath, int numberOfLines)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    string content = CreateFileContent(numberOfLines);
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                throw new FileGeneratorException($"Error occurred while creating file '{filePath}'.\n{ex.Message}", filePath, ex);
            }
        }

        private string CreateFileContent(int numberOfLines)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numberOfLines; i++)
            {
                sb.AppendLine(CreateLine());
            }

            return sb.ToString();
        }

        private string CreateLine()
        {
            const string separator = "||";
            int length = 10;
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}",
                separator,
                GetStringDate(),
                generator.GenerateLatinChars(length),
                generator.GenerateRussianChars(length),
                generator.GeneratePositiveEvenNumber(1, 100_000_000),
                generator.GeneratePositiveDoubleNumber(1, 20)
            );
        }

        private string GetStringDate()
        {
            DateTime startDate = DateTime.Now.AddYears(-5);
            DateTime endDate = DateTime.Now;
            return generator.GenerateDate(startDate, endDate).ToString("dd.MM.yyyy");
        }
    }
}
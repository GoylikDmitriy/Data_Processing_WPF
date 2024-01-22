using Core.Task1.Services.FileServices.Abstract;
using Core.Task1.Services.FileServices.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task1.Services.FileServices
{
    public class FileMergeService : IFileMergeService
    {
        private int removedLinesCount;
        public int RemovedLinesCount => removedLinesCount;

        public async Task MergeFilesAsync(string path, string patternToRemove)
        {
            try
            {
                var files = Directory.GetFiles(path, "*.txt");
                string mergedFilePath = Path.Combine(path, "merged_file.txt");
                var buffer = new List<string>();
                await Task.Run(() =>
                {
                    foreach (var file in files)
                    {
                        try
                        {
                            var lines = File.ReadLines(file);
                            ProcessLines(lines, patternToRemove, buffer);
                        }
                        catch (IOException ex)
                        {
                            throw new FileMergeException($"Error occurred while reading file: '{file}'.\n{ex.Message}", ex);
                        }

                        try
                        {
                            WriteLinesToFile(mergedFilePath, buffer);
                        }
                        catch (IOException ex)
                        {
                            throw new FileMergeException($"Error occurred while writing to file: '{file}'.\n{ex.Message}", ex);
                        }

                        buffer.Clear();
                    }
                });
            }
            catch (IOException ex)
            {
                throw new FileMergeException($"Error occurred while getting files.\n{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new FileMergeException(ex.Message, ex);
            }
        }

        private void ProcessLines(IEnumerable<string> lines, string patternToRemove, List<string> buffer)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (!string.IsNullOrEmpty(patternToRemove) && line.Contains(patternToRemove))
                {
                    Interlocked.Increment(ref removedLinesCount);
                    continue;
                }

                buffer.Add(line);
            }
        }

        private void WriteLinesToFile(string filePath, IEnumerable<string> lines)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                foreach (var line in lines)
                    writer.WriteLine(line);

                writer.Flush();
            }
        }
    }
}

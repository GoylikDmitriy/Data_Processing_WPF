using Core.Task1.Services.DbServices;
using Core.Task1.Services.DbServices.Abstract;
using Core.Task1.Services.FileServices;
using Core.Task1.Services.FileServices.Abstract;
using Core.Task1.Utilities.Generators;
using Core.Task1.Utilities.Mappers;
using Core.Task2.Model;
using Core.Task2.Services.DbServices;
using Core.Task2.Services.DbServices.Abstract;
using Core.Task2.Services.DbServices.Exceptions;
using Core.Task2.Services.FileServices;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace DataProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFileService fileService;
        private readonly IDbImporter dbImporter;
        private readonly IDbTbsReader reader;

        private CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
            fileService = new FileService(new FileGeneratorService(new DataGenerator()), new FileMergeService());
            dbImporter = new DbImporter(new DbDataImporter(new DataMapper()), new DbTbsImporter(new TrialBalanceSheetReader(new ExcelReader())));
            reader = new DbTbsReader();
            SetFileNamesToListBox();
        }

        private async void btnGenerateFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedDirectory = SelectDirectory("Select empty folder");
                if (!string.IsNullOrEmpty(selectedDirectory))
                {
                    if (IsFolderEmpty(selectedDirectory))
                    {
                        await fileService.CreateFilesAsync(selectedDirectory, 100, 100_000);
                        lblGeneratedFiles.Content = $"Files were generated to {selectedDirectory}.";
                    }
                    else
                    {
                        MessageBox.Show("Folder is not empty. Please choose an empty folder.", "WARNING");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private async void btnMergeFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedDirectory = SelectDirectory("Select folder");
                if (!string.IsNullOrEmpty(selectedDirectory))
                {
                    int removedLinesCount = await fileService.MergeFilesAsync(selectedDirectory, txtLineToDelete.Text);
                    lblRemovedLinesCount.Content = $"{removedLinesCount} lines were deleted.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedFilePath = SelectFile("Select file to import");
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    var progress = new Progress<int>(count =>
                    {
                        lblUploadedFiles.Content = $"Imported {count} records out of {dbImporter.TotalRecords}";
                    });

                    cancellationTokenSource = new CancellationTokenSource();
                    await dbImporter.ImportDataAsync(selectedFilePath, progress, cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show("Import stopped by the user.", "INFO");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private void btnStopImport_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private async void btnImportExcelFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedFilePath = SelectFile("Select excel file to import");
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    FileName fileName = await dbImporter.ImportTbsAsync(selectedFilePath);
                    lblImportedExcelFile.Content = $"File {fileName.Name} has been imported.";
                    listBoxImportedExcelFiles.DisplayMemberPath = "Name";
                    listBoxImportedExcelFiles.Items.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private void btnShowData_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = listBoxImportedExcelFiles.SelectedItem as FileName;
            if (selectedItem != null)
            {
                new TbsWindow(selectedItem, reader).Show();
            }
            else
            {
                MessageBox.Show("You have to select file to show.", "INFO");
            }
        }

        private void SetFileNamesToListBox()
        {
            try
            {
                var fileNames = reader.ReadAllFileNames();
                foreach (var fileName in fileNames)
                {
                    listBoxImportedExcelFiles.DisplayMemberPath = "Name";
                    listBoxImportedExcelFiles.Items.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private string SelectDirectory(string description)
        {
            var dialog = OpenFileDialog(description, false, false);
            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName);
            }

            return null;
        }

        private string SelectFile(string description)
        {
            var dialog = OpenFileDialog(description, true, true);
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }

            return null;
        }

        private OpenFileDialog OpenFileDialog(string description, bool validateNames, bool checkFileExists)
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = description;
            dialog.ValidateNames = validateNames;
            dialog.CheckFileExists = checkFileExists;
            return dialog;
        }

        private bool IsFolderEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}
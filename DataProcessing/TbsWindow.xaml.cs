using Core.Task2.Model;
using Core.Task2.Services.DbServices;
using Core.Task2.Services.DbServices.Abstract;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataProcessing
{
    /// <summary>
    /// Interaction logic for TbsWindow.xaml
    /// </summary>
    public partial class TbsWindow : Window
    {
        FileName fileName;
        private readonly IDbTbsReader reader;
        public TbsWindow(FileName fileName, IDbTbsReader reader)
        {
            InitializeComponent();
            this.fileName = fileName;
            this.reader = reader;
            ShowData();
        }

        private void ShowData()
        {
            decimal totalObAssets = 0;
            decimal totalObLiabilities = 0;
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalCbAssets = 0;
            decimal totalCbLiabilities = 0;

            try
            {
                int rowIndex = 5;
                Bank bank = reader.ReadBankByFileNameId(fileName);
                Period period = reader.ReadPeriodByFileNameId(fileName);
                tblPeriod.Text = $"за период с {period.StartDate:dd.MM.yyyy} по {period.EndDate:dd.MM.yyyy}";
                tblBank.Text = $"по банку {bank.Name}";

                var accountClasses = reader.ReadAccountClassesByBankIdAndPeriodId(bank, period);
                foreach (var accountClass in accountClasses)
                {
                    SetupAccountClass(accountClass, rowIndex++);

                    decimal totalClassObAssets = 0;
                    decimal totalClassObLiabilities = 0;
                    decimal totalClassDebit = 0;
                    decimal totalClassCredit = 0;
                    decimal totalClassCbAssets = 0;
                    decimal totalClassCbLiabilities = 0;
                    var accounts = reader.ReadAccountsByAccountClassId(accountClass);
                    foreach (var account in accounts)
                    {
                        SetupAccount(account, rowIndex);

                        // opening balance
                        var ob = SetupOpeningBalance(account, rowIndex);
                        totalClassObAssets += ob.Assets;
                        totalClassObLiabilities += ob.Liabilities;

                        // transaction
                        var transaction = SetupTransaction(account, rowIndex);
                        totalClassDebit += transaction.Debit;
                        totalClassCredit += transaction.Credit;

                        // closing balance
                        var cb = SetupClosingBalance(account, rowIndex);
                        totalClassCbAssets += cb.Assets;
                        totalClassCbLiabilities += cb.Liabilities;

                        rowIndex++;
                    }

                    SetupTotalInfo($"{accountClass.Number}", totalClassObAssets, totalClassObLiabilities, totalClassDebit, totalClassCredit, totalClassCbAssets, totalClassCbLiabilities, rowIndex);

                    totalObAssets += totalClassObAssets;
                    totalObLiabilities += totalClassObLiabilities;
                    totalDebit += totalClassDebit;
                    totalCredit += totalClassCredit;
                    totalCbAssets += totalClassCbAssets;
                    totalCbLiabilities += totalClassCbLiabilities;

                    rowIndex++;
                }

                SetupTotalInfo("БАЛАНС", totalObAssets, totalObLiabilities, totalDebit, totalCredit, totalCbAssets, totalCbLiabilities, rowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                Console.Write(ex.StackTrace);
            }
        }

        private void SetupTotalInfo(string accountText, decimal totalObAssets, decimal totalObLiabilities, decimal totalDebit, decimal totalCredit, decimal totalCbAssets, decimal totalCbLiabilities, int row)
        {
            AddRow();
            Border border = SetupBorder(SetupTextBlock(accountText, FontWeights.Bold, HorizontalAlignment.Center), new Thickness(10, 0, 0, 0), 2);
            SetupGridAndAdd(border, row, 0);

            border = SetupBorder(SetupTextBlock($"{totalObAssets:#,##0.00}", FontWeights.Bold), new Thickness(0), 2);
            SetupGridAndAdd(border, row, 1);

            border = SetupBorder(SetupTextBlock($"{totalObLiabilities:#,##0.00}", FontWeights.Bold), new Thickness(0), 2);
            SetupGridAndAdd(border, row, 2);

            border = SetupBorder(SetupTextBlock($"{totalDebit:#,##0.00}", FontWeights.Bold), new Thickness(0), 2);
            SetupGridAndAdd(border, row, 3);

            border = SetupBorder(SetupTextBlock($"{totalCredit:#,##0.00}", FontWeights.Bold), new Thickness(0), 2);
            SetupGridAndAdd(border, row, 4);

            border = SetupBorder(SetupTextBlock($"{totalCbAssets:#,##0.00}", FontWeights.Bold), new Thickness(0), 2);
            SetupGridAndAdd(border, row, 5);

            border = SetupBorder(SetupTextBlock($"{totalCbLiabilities:#,##0.00}", FontWeights.Bold), new Thickness(0, 0, 10, 0), 2);
            SetupGridAndAdd(border, row, 6);
        }

        private void SetupAccountClass(AccountClass accountClass, int row)
        {
            AddRow();
            TextBlock tbl = SetupTextBlock($"КЛАСС {accountClass.Number} {accountClass.Description}", FontWeights.Bold, HorizontalAlignment.Center);
            Border border = SetupBorder(tbl, new Thickness(10, 0, 10, 0), 2);
            SetupGridAndAdd(border, row, 0, 7);
        }

        private void SetupAccount(Account account, int row)
        {
            AddRow();
            Border border = SetupBorder(SetupTextBlock($"{account.Number}", HorizontalAlignment.Center), new Thickness(10, 0, 0, 0));
            SetupGridAndAdd(border, row, 0);
        }

        private OpeningBalance SetupOpeningBalance(Account account, int row)
        {
            var ob = reader.ReadOpeningBalanceByAccountId(account);
            Border border = SetupBorder(SetupTextBlock($"{ob.Assets:#,##0.00}"), new Thickness(0));
            SetupGridAndAdd(border, row, 1);

            border = SetupBorder(SetupTextBlock($"{ob.Liabilities:#,##0.00}"), new Thickness(0));
            SetupGridAndAdd(border, row, 2);

            return ob;
        }

        private Transaction SetupTransaction(Account account, int row)
        {
            var transaction = reader.ReadTransactionByAccountId(account);
            Border border = SetupBorder(SetupTextBlock($"{transaction.Debit:#,##0.00}"), new Thickness(0));
            SetupGridAndAdd(border, row, 3);

            border = SetupBorder(SetupTextBlock($"{transaction.Credit:#,##0.00}"), new Thickness(0));
            SetupGridAndAdd(border, row, 4);

            return transaction;
        }

        private ClosingBalance SetupClosingBalance(Account account, int row)
        {
            var cb = reader.ReadClosingBalanceByAccountId(account);
            Border border = SetupBorder(SetupTextBlock($"{cb.Assets:#,##0.00}"), new Thickness(0));
            SetupGridAndAdd(border, row, 5);

            border = SetupBorder(SetupTextBlock($"{cb.Liabilities:#,##0.00}"), new Thickness(0, 0, 10, 0));
            SetupGridAndAdd(border, row, 6);

            return cb;
        }

        private TextBlock SetupTextBlock(string text, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right)
        {
            TextBlock tbl = new TextBlock();
            tbl.Text = text;
            tbl.VerticalAlignment = VerticalAlignment.Center;
            tbl.HorizontalAlignment = horizontalAlignment;
            tbl.Margin = new Thickness(0, 0, 2, 0);
            tbl.FontSize = 15;
            return tbl;
        }

        private TextBlock SetupTextBlock(string text, FontWeight fontWeight, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right)
        {
            TextBlock tbl = SetupTextBlock(text, horizontalAlignment);
            tbl.FontWeight = fontWeight;
            return tbl;
        }

        private Border SetupBorder(UIElement child, Thickness margin, int thickness = 1)
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.CadetBlue);
            border.BorderThickness = new Thickness(thickness);
            border.Margin = margin;
            border.Child = child;
            return border;
        }

        private void SetupGridAndAdd(Border border, int row, int column, int columnSpan = 1)
        {
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            Grid.SetColumnSpan(border, columnSpan);
            TbsGrid.Children.Add(border);
        }

        private void AddRow()
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(30);
            TbsGrid.RowDefinitions.Add(row);
        }
    }
}

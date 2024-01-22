using Core.Task2.Services.FileServices.Exceptions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.FileServices
{
    public class ExcelReader
    {
        public IEnumerable<IEnumerable<string>> Read(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = Path.GetExtension(path) switch
                    {
                        ".xls" => new HSSFWorkbook(fileStream),
                        ".xlsx" => new XSSFWorkbook(fileStream),
                        _ => throw new ReaderException("Unsupported file format.")
                    };

                    ISheet sheet = workbook.GetSheetAt(0);
                    List<List<string>> excel = new List<List<string>>();
                    for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row != null)
                        {
                            List<string> rowData = new List<string>();
                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {
                                ICell cell = row.GetCell(cellIndex);
                                if (cell != null)
                                {
                                    string cellValue = cell.ToString();
                                    rowData.Add(cellValue);
                                }
                            }

                            excel.Add(rowData);
                        }
                    }

                    workbook.Close();
                    return excel;
                }
            }
            catch (Exception ex)
            {
                throw new ReaderException($"Error occured while reading excel file: {path}\n{ex.Message}", ex);
            }
        }
    }
}

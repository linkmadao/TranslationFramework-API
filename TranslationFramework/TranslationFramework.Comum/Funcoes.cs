using OfficeOpenXml;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace TranslationFramework.Comum
{
    public static class Funcoes
    {
        public static bool IsEnumerable(this Type type)
        {
            if (type == null || type == typeof(string) || type == typeof(byte[]))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static ExcelAddressBase GetValuedDimension(this ExcelWorksheet worksheet)
        {
            var dimension = worksheet.Dimension;
            if (dimension == null) return null;
            var cells = worksheet.Cells[dimension.Address];
            Int32 minRow = 0, minCol = 0, maxRow = 0, maxCol = 0;
            var hasValue = false;
            foreach (var cell in cells.Where(cell => cell.Value != null))
            {
                if (!hasValue)
                {
                    minRow = cell.Start.Row;
                    minCol = cell.Start.Column;
                    maxRow = cell.End.Row;
                    maxCol = cell.End.Column;
                    hasValue = true;
                }
                else
                {
                    if (cell.Start.Column < minCol)
                    {
                        minCol = cell.Start.Column;
                    }
                    if (cell.End.Row > maxRow)
                    {
                        maxRow = cell.End.Row;
                    }
                    if (cell.End.Column > maxCol)
                    {
                        maxCol = cell.End.Column;
                    }
                }
            }
            return hasValue ? new ExcelAddressBase(minRow, minCol, maxRow, maxCol) : null;
        }

        public static byte[] ConvertStringToUtf8(this object texto)
        {
            if (texto == null)
            {
                return new byte[] { };
            }

            return Encoding.UTF8.GetBytes(texto.ToString());
        }

        public static string ConvertUtf8ToString(this byte[] texto)
        {
            if (texto == null)
            {
                return null;
            }

            return Encoding.UTF8.GetString(texto);
        }
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DataGridView_Merge_Cells
{
    public class DataGridViewMergedTextBoxColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewMergedTextBoxColumn()
        {
            CellTemplate = new DataGridViewMergedTextBoxCell();
        }

        private class DataGridViewMergedTextBoxCell : DataGridViewTextBoxCell
        {
            private bool IsRepeatedCellValue(int rowIndex, int colIndex)
            {
                if (rowIndex == 0)
                    return false;

                DataGridViewCell currCell = this.DataGridView.Rows[rowIndex].Cells[colIndex];
                DataGridViewCell prevCell = this.DataGridView.Rows[rowIndex - 1].Cells[colIndex];

                return Object.Equals(currCell.Value, prevCell.Value);
            }

            protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
            {
                if ((rowIndex > 0) && IsRepeatedCellValue(rowIndex, this.ColumnIndex))
                    return string.Empty;
                else
                    return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                if (rowIndex < this.DataGridView.Rows.Count - 1)
                {
                    if (IsRepeatedCellValue(rowIndex + 1, this.ColumnIndex))
                    {
                        advancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }

                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}


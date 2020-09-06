using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals
{
    /// <summary>
    /// 为Grid添加边框
    /// </summary>
    public class GridHelperBor
    {
        static List<RecordGridRowCol> lstRowCol = new List<RecordGridRowCol>();
        private static void RefreshGrid(Grid grid, int lineWidth, Brush color)
        {
            lstRowCol.Clear();
            for (var i = 0; i < grid.Children.Count; i++)
            {
                var child = grid.Children[i];

                var bd = child as Border;
                if (bd != null && bd.Tag != null && bd.Tag.ToString() == "gridline")
                {
                    grid.Children.Remove(bd);
                }
                else
                {
                    var item = grid.Children[i] as FrameworkElement;
                    var row = Grid.GetRow(item);
                    var col = Grid.GetColumn(item);
                    var rowspan = Grid.GetRowSpan(item);
                    var columnspan = Grid.GetColumnSpan(item);

                    //存储合并的行列
                    if (rowspan > 1 || columnspan > 1)
                        lstRowCol.Add(new RecordGridRowCol() { gridRow = row, gridCol = col, gridRowSpan = rowspan, gridColSpan = columnspan });
                }
            }

            try
            {
                var rows = grid.RowDefinitions.Count;
                var cols = grid.ColumnDefinitions.Count;

                //边界考虑
                if (rows == 0)
                    rows = 1;
                if (cols == 0)
                    cols = 1;

                List<RecordGridRowCol> lstCombine = CalculationRowCol(lstRowCol);
                int _row = 0;
                int _col = 0;
                //生成行列
                for (var i = 0; i < rows; i++)
                {
                    //当前是否存在合并的行列                  
                    if (lstRowCol.Count > 0)
                    {
                        if (lstRowCol[0].gridRowSpan > 1)
                            _row = lstRowCol[0].gridRowSpan - 1;
                        if (lstRowCol[0].gridColSpan > 1)
                            _col = lstRowCol[0].gridColSpan - 1;
                    }
                    for (var j = 0; j < cols; j++)
                    {
                        //从左边界和上边界开始画边框
                        var thick = new Thickness(lineWidth, lineWidth, 0, 0);
                        var margin = new Thickness(-lineWidth / 2d, -lineWidth / 2d, 0, 0);
                        //边界考虑
                        if (i == 0)
                            margin.Top = 0;
                        if (i == rows - 1)
                            thick.Bottom = lineWidth;
                        if (j == 0)
                            margin.Left = 0;
                        if (j == cols - 1)
                            thick.Right = lineWidth;

                        if (lstCombine.Count > 0)
                        {
                            var tep = lstCombine.Where(a => a.gridRow == i && a.gridCol == j).ToList();
                            if (tep.Count > 0)
                            {
                                //有合并列列
                                if (tep[0].gridColSpan == 1)
                                    thick.Left = 0;

                                //有合并行
                                if (tep[0].gridRowSpan == 1)
                                    thick.Top = 0;
                            }
                        }

                        var bd = new Border
                        {
                            BorderThickness = thick,
                            Margin = margin,
                            BorderBrush = color,
                            Tag = "gridline"
                        };
                        Grid.SetRow(bd, i);
                        Grid.SetColumn(bd, j);
                        grid.Children.Add(bd);
                    }
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
            grid.InvalidateArrange();
            grid.InvalidateVisual();
        }
        /// <summary>
        /// 计算出需要合并的单元格
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        static List<RecordGridRowCol> CalculationRowCol(List<RecordGridRowCol> lst)
        {
            List<RecordGridRowCol> tem = new List<RecordGridRowCol>();

            foreach (var item in lst)
            {
                int row = item.gridRow;
                int col = item.gridCol;
                if (item.gridColSpan > 1)
                {
                    for (int i = col + 1; i <= item.gridColSpan + col - 1; i++)
                        tem.Add(new RecordGridRowCol() { gridRow = row, gridCol = i, gridColSpan = 1 });
                }
                if (item.gridRowSpan > 1)
                {
                    for (int i = row + 1; i <= item.gridRowSpan + row - 1; i++)
                    {
                        tem.Add(new RecordGridRowCol() { gridRow = i, gridCol = col, gridRowSpan = 1 });
                        if (item.gridColSpan > 1)
                        {
                            for (int p = 0; p < item.gridColSpan; p++)
                                tem.Add(new RecordGridRowCol() { gridRow = i, gridCol = col + p, gridRowSpan = 1, gridColSpan = 1 });
                        }
                    }
                }
            }
            return tem;
        }

        #region 线颜色

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.RegisterAttached("LineColor", typeof(Brush), typeof(GridHelperBor),
                new PropertyMetadata(Brushes.Black, GridLinesPropertyChanged));

        public static Brush GetLineColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(LineColorProperty);
        }

        public static void SetLineColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(LineColorProperty, value);
        }

        #endregion

        #region 线宽度
        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.RegisterAttached("LineWidth", typeof(int), typeof(GridHelperBor),
                new PropertyMetadata(1, GridLinesPropertyChanged));

        public static int GetLineWidth(DependencyObject obj)
        {
            return (int)obj.GetValue(LineWidthProperty);
        }

        public static void SetLineWidth(DependencyObject obj, int value)
        {
            obj.SetValue(LineWidthProperty, value);
        }
        #endregion

        #region 是否显示线

        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.RegisterAttached("ShowGridLines", typeof(bool), typeof(GridHelperBor),
                new PropertyMetadata(false, GridLinesPropertyChanged));

        public static bool GetShowGridLines(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowGridLinesProperty);
        }
        public static void SetShowGridLines(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowGridLinesProperty, value);
        }
        #endregion

        private static void GridLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
            {
                return;
            }
            var showLines = GetShowGridLines(grid);
            var color = GetLineColor(grid);
            var lineWidth = GetLineWidth(grid);
            if (showLines)
            {
                //  grid.SnapsToDevicePixels = true;
                grid.Loaded += delegate { RefreshGrid(grid, lineWidth, color); };
            }
        }
    }

    public class RecordGridRowCol
    {
        public int gridRow { get; set; }
        public int gridCol { get; set; }
        public int gridRowSpan { get; set; }
        public int gridColSpan { get; set; }
    }
}
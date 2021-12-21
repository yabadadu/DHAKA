using DevExpress.XtraGrid.Helpers;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Drawing;

namespace HitopsCommon.GridCommon
{
    class CellDrawHelper
    {
        public static void DrawCellBorder(RowCellCustomDrawEventArgs e)
        {
            Brush brush = Brushes.Green;

            e.Cache.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 2, 3)); // Top
            e.Cache.FillRectangle(brush, new Rectangle(e.Bounds.Right - 2, e.Bounds.Y - 2, 3, e.Bounds.Height + 3)); // Rigth
            e.Cache.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Bottom - 2, e.Bounds.Width + 2, 3)); // Bottom
            e.Cache.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 2, 3, e.Bounds.Height + 3)); // Left
        }

        public static void DoDefaultDrawCell(GridView view, RowCellCustomDrawEventArgs e)
        {
            e.Appearance.FillRectangle(e.Cache, e.Bounds);
            ((IViewController)view.GridControl).EditorHelper.DrawCellEdit(
                        new GridViewDrawArgs(e.Cache, (view.GetViewInfo() as GridViewInfo), e.Bounds),
                                            (e.Cell as GridCellInfo).Editor,
                                            (e.Cell as GridCellInfo).ViewInfo,
                                            e.Appearance,
                                            (e.Cell as GridCellInfo).CellValueRect.Location
            );
        }
    }
}

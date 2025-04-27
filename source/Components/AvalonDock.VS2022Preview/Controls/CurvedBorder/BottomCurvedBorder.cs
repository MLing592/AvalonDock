using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonDock.Controls;
/// <summary>
/// 暂未使用/NotUsage
/// </summary>
public class BottomCurvedBorder : CurvedBorderBase
{
	protected override Size GetBackgroundSize(Size originalSize, Thickness border)
	{
		return new Size(originalSize.Width - border.Left - border.Right, originalSize.Height - border.Bottom);
	}

	protected override Vector GetBackgroundOffset(Thickness border) => new Vector(border.Left, 0.0);

	protected override CurvedBorderBase.CurveInfo GetCurveInfo(Size size, Thickness border, Vector offset, bool isBorder)
	{
		double halfLeft = 0.5 * border.Left;
		double halfTop = 0.5 * border.Top;
		double halfRight = 0.5 * border.Right;
		double halfBottom = 0.5 * border.Bottom;
		CurvedBorderBase.CurveInfo curveInfo;
		if (isBorder)
		{
			curveInfo = new CurvedBorderBase.CurveInfo(size, border, offset);
			curveInfo.LeftTop = Math.Max(0.0, this.CornerRadius.TopLeft - halfLeft);
			curveInfo.TopLeft = Math.Max(0.0, this.CornerRadius.TopLeft - halfTop);
			curveInfo.TopRight = Math.Max(0.0, this.CornerRadius.TopRight - halfTop);
			curveInfo.RightTop = Math.Max(0.0, this.CornerRadius.TopRight - halfRight);
			curveInfo.RightBottom = this.CornerRadius.BottomRight + halfRight;
			curveInfo.BottomRight = this.CornerRadius.BottomRight + halfBottom;
			curveInfo.BottomLeft = this.CornerRadius.BottomLeft + halfBottom;
			curveInfo.LeftBottom = this.CornerRadius.BottomLeft + halfLeft;
		}
		else
		{
			curveInfo = new CurvedBorderBase.CurveInfo(size, new Thickness(border.Left, 0.0, border.Right, border.Bottom), offset);
			curveInfo.LeftTop = this.CornerRadius.TopLeft + halfLeft;
			curveInfo.TopLeft = this.CornerRadius.TopLeft + halfTop;
			curveInfo.TopRight = this.CornerRadius.TopRight + halfTop;
			curveInfo.RightTop = this.CornerRadius.TopRight + halfRight;
			curveInfo.RightBottom = Math.Max(0.0, this.CornerRadius.BottomRight - halfRight);
			curveInfo.BottomRight = Math.Max(0.0, this.CornerRadius.BottomRight - halfBottom);
			curveInfo.BottomLeft = Math.Max(0.0, this.CornerRadius.BottomLeft - halfBottom);
			curveInfo.LeftBottom = Math.Max(0.0, this.CornerRadius.BottomLeft - halfLeft);
		}
		return curveInfo;
	}

	protected override void GenerateGeometry(
	  StreamGeometryContext context,
	  CurvedBorderBase.CurveInfo info)
	{
		Size size = info.Size;
		Vector offset = info.Offset;
		Point startPoint = new Point(0.0, 0.0) + offset;
		Point endPoint1 = new Point(0.0, info.Border.Top) + offset;
		double y1 = this.CornerRadius.TopLeft + info.Border.Top;

		Point endPoint2 = new Point(this.CornerRadius.TopLeft, y1) + offset;
		double y2 = size.Height - this.CornerRadius.BottomLeft;
		Point endPoint3 = new Point(this.CornerRadius.TopLeft, y2) + offset;
		Point endPoint4 = new Point(this.CornerRadius.TopLeft + this.CornerRadius.BottomLeft, size.Height) + offset;
		Point endPoint5 = new Point(size.Width - this.CornerRadius.BottomRight - this.CornerRadius.TopRight, size.Height) + offset;
		Point endPoint6 = new Point(size.Width - this.CornerRadius.TopRight, size.Height - this.CornerRadius.BottomLeft) + offset;
		Point endPoint7 = new Point(size.Width - this.CornerRadius.TopRight, this.CornerRadius.TopRight + info.Border.Top) + offset;
		Point endPoint8 = new Point(size.Width, info.Border.Top) + offset;
		Point endPoint9 = new Point(size.Width, 0.0) + offset;

		context.BeginFigure(startPoint, true, true);
		if (IsLeftFirst)
		{
			endPoint3.X = startPoint.X;
			this.DrawLineTo(context, endPoint3);
			endPoint4 = new Point(this.CornerRadius.BottomLeft, 0.0) + offset;
			this.DrawArcTo(context, endPoint4, info.LeftBottom, info.BottomLeft, SweepDirection.Counterclockwise);
		}
		else
		{
			this.DrawLineTo(context, endPoint1);
			this.DrawArcTo(context, endPoint2, info.LeftTop, info.TopLeft, SweepDirection.Clockwise);
			this.DrawLineTo(context, endPoint3);
			this.DrawArcTo(context, endPoint4, info.LeftBottom, info.BottomLeft, SweepDirection.Counterclockwise);
		}
		this.DrawLineTo(context, endPoint5);
		this.DrawArcTo(context, endPoint6, info.RightBottom, info.BottomRight, SweepDirection.Counterclockwise);
		this.DrawLineTo(context, endPoint7);
		this.DrawArcTo(context, endPoint8, info.RightTop, info.TopRight, SweepDirection.Clockwise);
		this.DrawLineTo(context, endPoint9);

		if (Debugger.IsAttached)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(info.ToString());
			sb.Append($"M{startPoint.X},{startPoint.Y} ");
			if (IsLeftFirst)
			{
				sb.Append($"L{endPoint3.X},{endPoint3.Y} ");
				sb.Append($"A{info.LeftBottom},{info.BottomLeft} 0 0 0 {endPoint4.X},{endPoint4.Y} ");
			}
			else
			{
				sb.Append($"L{endPoint1.X},{endPoint1.Y} ");
				sb.Append($"A{info.LeftTop},{info.TopLeft} 0 0 1 {endPoint2.X},{endPoint2.Y} ");
				sb.Append($"L{endPoint3.X},{endPoint3.Y} ");
				sb.Append($"A{info.LeftBottom},{info.BottomLeft} 0 0 0 {endPoint4.X},{endPoint4.Y} ");
			}
			sb.Append($"L{endPoint5.X},{endPoint5.Y} ");
			sb.Append($"A{info.RightBottom},{info.BottomRight} 0 0 0 {endPoint6.X},{endPoint6.Y} ");
			sb.Append($"L{endPoint7.X},{endPoint7.Y} ");
			sb.Append($"A{info.RightTop},{info.TopRight} 0 0 1 {endPoint8.X},{endPoint8.Y} ");
			sb.Append($"L{endPoint9.X},{endPoint9.Y}");
			Trace.WriteLine(sb.ToString());
		}
	}

	

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace AvalonDock.Controls;

public class TopCurvedBorder : CurvedBorderBase
{
	protected override Size GetBackgroundSize(Size originalSize, Thickness border)
	{
		return new Size(originalSize.Width - border.Left - border.Right, originalSize.Height - border.Top);
	}

	protected override Vector GetBackgroundOffset(Thickness border)
	{
		return new Vector(border.Left, border.Top);
	}

	protected override CurvedBorderBase.CurveInfo GetCurveInfo(Size size, Thickness border, Vector offset, bool isBorder)
	{
		double halfLeft = 0.5 * border.Left;
		double halfTop = 0.5 * border.Top;
		double halfRight = 0.5 * border.Right;
		double halfBottom = 0.5 * border.Bottom;
		CurvedBorderBase.CurveInfo curveInfo;
		if (isBorder)
		{
			// 计算各角的曲线偏移量，用 CornerRadius 的值加上边框厚度的一半，偏移为0
			curveInfo = new CurvedBorderBase.CurveInfo(size, border, offset);
			curveInfo.LeftTop = this.CornerRadius.TopLeft + halfLeft;
			curveInfo.TopLeft = this.CornerRadius.TopLeft + halfTop;
			curveInfo.TopRight = this.CornerRadius.TopRight + halfTop;
			curveInfo.RightTop = this.CornerRadius.TopRight + halfRight;
			curveInfo.RightBottom = Math.Max(0.0, this.CornerRadius.BottomRight - halfRight);
			curveInfo.BottomRight = Math.Max(0.0, this.CornerRadius.BottomRight - halfBottom);
			curveInfo.BottomLeft = Math.Max(0.0, this.CornerRadius.BottomLeft - halfBottom);
			curveInfo.LeftBottom = Math.Max(0.0, this.CornerRadius.BottomLeft - halfLeft);
		}
		else
		{
			// 计算各角的曲线偏移量，用 CornerRadius 的值减去边框厚度的一半
			curveInfo = new CurvedBorderBase.CurveInfo(size, new Thickness(border.Left, border.Top, border.Right, 0.0), offset);
			curveInfo.LeftTop = Math.Max(0.0, this.CornerRadius.TopLeft - halfLeft);
			curveInfo.TopLeft = Math.Max(0.0, this.CornerRadius.TopLeft - halfTop);
			curveInfo.TopRight = Math.Max(0.0, this.CornerRadius.TopRight - halfTop);
			curveInfo.RightTop = Math.Max(0.0, this.CornerRadius.TopRight - halfRight);
			curveInfo.RightBottom = this.CornerRadius.BottomRight + halfRight;
			curveInfo.BottomRight = this.CornerRadius.BottomRight + halfBottom;
			curveInfo.BottomLeft = this.CornerRadius.BottomLeft + halfBottom;
			curveInfo.LeftBottom = this.CornerRadius.BottomLeft + halfLeft;
		}
		return curveInfo;
	}

	protected override void GenerateGeometry(StreamGeometryContext context, CurvedBorderBase.CurveInfo info)
	{
		//容器大小(287,25)/(285,24)
		Size size = info.Size;
		//起始偏移(0,0)/(1,1)
		Vector offset = info.Offset;
		//起始点(0,25)/(1,25)
		Point startPoint = new Point(0.0, size.Height) + offset;
		//点1(0,24)/(1,25)
		Point endPoint1 = new Point(0.0, size.Height - info.Border.Bottom) + offset;
		//CornerRadius(1,1,1,1)
		double y = size.Height - this.CornerRadius.BottomLeft - info.Border.Bottom;
		Point endPoint2 = new Point(this.CornerRadius.BottomLeft, y) + offset;
		Point endPoint3 = new Point(this.CornerRadius.BottomLeft, this.CornerRadius.TopLeft) + offset;
		Point endPoint4 = new Point(this.CornerRadius.BottomLeft + this.CornerRadius.TopLeft, 0.0) + offset;
		Point endPoint5 = new Point(size.Width - this.CornerRadius.TopRight - this.CornerRadius.BottomRight, 0.0) + offset;
		Point endPoint6 = new Point(size.Width - this.CornerRadius.BottomRight, this.CornerRadius.TopRight) + offset;
		Point endPoint7 = new Point(size.Width - this.CornerRadius.BottomRight, size.Height - this.CornerRadius.BottomRight - info.Border.Bottom) + offset;
		Point endPoint8 = new Point(size.Width, size.Height - info.Border.Bottom) + offset;
		Point endPoint9 = new Point(size.Width, size.Height) + offset;

		context.BeginFigure(startPoint, true, true);//(0,25)
		if (IsLeftFirst)
		{
			endPoint3.X = startPoint.X;
			this.DrawLineTo(context, endPoint3);//(0,1)
			endPoint4 = new Point(this.CornerRadius.TopLeft, 0.0) + offset;
			this.DrawArcTo(context, endPoint4, info.LeftTop, info.TopLeft, SweepDirection.Clockwise);//(2,0)
		}
		else
		{
			this.DrawLineTo(context, endPoint1);//(0,24)/(1,25)
			this.DrawArcTo(context, endPoint2, info.LeftBottom, info.BottomLeft, SweepDirection.Counterclockwise);//(1, 23)/(2,24)
			this.DrawLineTo(context, endPoint3);//(1,1)/(2,2)
			this.DrawArcTo(context, endPoint4, info.LeftTop, info.TopLeft, SweepDirection.Clockwise);//(2,0)/(3,1)
		}
		this.DrawLineTo(context, endPoint5);//(285,0)/(284,1)
		this.DrawArcTo(context, endPoint6, info.RightTop, info.TopRight, SweepDirection.Clockwise);//(286,1)/(285,2)
		this.DrawLineTo(context, endPoint7);//(286,23)/(285,24)
		this.DrawArcTo(context, endPoint8, info.RightBottom, info.BottomRight, SweepDirection.Counterclockwise);//(287,24)/(286,25)
		this.DrawLineTo(context, endPoint9);//(287,25)/(286,25)

		if (Debugger.IsAttached)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(info.ToString());
			sb.Append($"M{startPoint.X},{startPoint.Y} ");
			if (IsLeftFirst)
			{
				sb.Append($"L{endPoint3.X},{endPoint3.Y} ");
				sb.Append($"A{info.LeftTop},{info.TopLeft} 0 0 1 {endPoint4.X},{endPoint4.Y} ");
			}
			else
			{
				sb.Append($"L{endPoint1.X},{endPoint1.Y} ");
				sb.Append($"A{info.LeftBottom},{info.BottomLeft} 0 0 0 {endPoint2.X},{endPoint2.Y} ");
				sb.Append($"L{endPoint3.X},{endPoint3.Y} ");
				sb.Append($"A{info.LeftTop},{info.TopLeft} 0 0 1 {endPoint4.X},{endPoint4.Y} ");
			}
			sb.Append($"L{endPoint5.X},{endPoint5.Y} ");
			sb.Append($"A{info.RightTop},{info.TopRight} 0 0 1 {endPoint6.X},{endPoint6.Y} ");
			sb.Append($"L{endPoint7.X},{endPoint7.Y} ");
			sb.Append($"A{info.RightBottom},{info.BottomRight} 0 0 0 {endPoint8.X},{endPoint8.Y} ");
			sb.Append($"L{endPoint9.X},{endPoint9.Y}");
			Trace.WriteLine(sb.ToString());
		}
	}
}

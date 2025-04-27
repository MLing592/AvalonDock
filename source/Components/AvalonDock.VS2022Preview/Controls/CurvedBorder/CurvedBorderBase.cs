using AvalonDock.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvalonDock.Controls;
public abstract class CurvedBorderBase : Border
{
    //private static readonly Thickness EmptyThickness = new Thickness(0.0);
	private static readonly Vector NoOffset = new Vector(0.0, 0.0);
	protected static readonly bool DoNotRoundBorders = false;

	static CurvedBorderBase() { }


	/// <summary>
	/// True: Use Border
	/// </summary>
	public bool IsBaseBorder
	{
		get { return (bool)GetValue(IsBaseBorderProperty); }
		set 
		{ 
			SetValue(IsBaseBorderProperty, value); 
			this.InvalidateVisual(); 
		}
	}
	public static readonly DependencyProperty IsBaseBorderProperty =
		DependencyProperty.Register("IsBaseBorder", typeof(bool), typeof(CurvedBorderBase), new PropertyMetadata(false));

	/// <summary>
	/// IsRowFirst
	/// </summary>
	public bool IsLeftFirst
	{
		get { return (bool)GetValue(IsLeftFirstProperty); }
		set { SetValue(IsLeftFirstProperty, value);}
	}
	public static readonly DependencyProperty IsLeftFirstProperty =
		DependencyProperty.Register("IsLeftFirst", typeof(bool), typeof(CurvedBorderBase), new PropertyMetadata(false));


	public Geometry BackgroundGeometry { get; private set; }

	public Geometry BorderGeometry { get; private set; }

	/// <summary>
	/// 获取背景大小
	/// </summary>
	/// <param name="originalSize"></param>
	/// <param name="border"></param>
	/// <returns></returns>
	protected abstract Size GetBackgroundSize(Size originalSize, Thickness border);

	/// <summary>
	/// 获取背景偏移
	/// </summary>
	/// <param name="border"></param>
	/// <returns></returns>
	protected abstract Vector GetBackgroundOffset(Thickness border);

	/// <summary>
	/// 获取弧角信息
	/// </summary>
	/// <param name="size">容器信息</param>
	/// <param name="border">边框信息</param>
	/// <param name="offset">偏移信息</param>
	/// <param name="isBorder">是否外扩</param>
	/// <returns></returns>
	protected abstract CurvedBorderBase.CurveInfo GetCurveInfo(Size size, Thickness border, Vector offset, bool isBorder);

	/// <summary>
	/// 生成Geometry
	/// </summary>
	/// <param name="context"></param>
	/// <param name="info"></param>
	protected abstract void GenerateGeometry(StreamGeometryContext context, CurvedBorderBase.CurveInfo info);
	
	protected override Size ArrangeOverride(Size finalSize)
	{
		if (IsBaseBorder)
		{
			return base.ArrangeOverride(finalSize);
		}

#if NET462_OR_GREATER || NETCOREAPP3_0_OR_GREATER
		DpiScale dpi = VisualTreeHelper.GetDpi((Visual)this);
		// scale xy
		double left = RoundValue(this.BorderThickness.Left, dpi.DpiScaleX);
		double top = RoundValue(this.BorderThickness.Top, dpi.DpiScaleY);
		double right = RoundValue(this.BorderThickness.Right, dpi.DpiScaleX);
		double bottom = RoundValue(this.BorderThickness.Bottom, dpi.DpiScaleY);
#else
		// scale xy
		double left = RoundValue(this.BorderThickness.Left, 1.0d);
		double top = RoundValue(this.BorderThickness.Top, 1.0d);
		double right = RoundValue(this.BorderThickness.Right, 1.0d);
		double bottom = RoundValue(this.BorderThickness.Bottom, 1.0d);
#endif

		Thickness border = new Thickness(left, top, right, bottom);
		Size backgroundSize = this.GetBackgroundSize(finalSize, border);
		Vector backgroundOffset = this.GetBackgroundOffset(border);

		this.BorderGeometry = CreateGeometry(this.GetCurveInfo(finalSize, border, CurvedBorderBase.NoOffset, true));
		this.BackgroundGeometry = CreateGeometry(this.GetCurveInfo(backgroundSize, border, backgroundOffset, false));
		return base.ArrangeOverride(finalSize);

		static double RoundValue(double value, double dpiScale)
		{
			return CurvedBorderBase.DoNotRoundBorders ? value : Math.Round(value * dpiScale) / dpiScale;
		}

		Geometry CreateGeometry(CurvedBorderBase.CurveInfo info)
		{
			StreamGeometry geometry = new StreamGeometry();
			using (StreamGeometryContext context = geometry.Open())
			{
				this.GenerateGeometry(context, info);
				geometry.Freeze();
			}
			return (Geometry)geometry;
		}
	}
	protected override void OnRender(DrawingContext dc)
	{
		if (this.BorderGeometry == null || this.BackgroundGeometry == null || this.IsBaseBorder)
		{
			base.OnRender(dc);
		}
		else
		{
			if (this.BorderBrush != null)
				dc.DrawGeometry(this.BorderBrush, (Pen)null, this.BorderGeometry);
			if (this.Background == null)
				return;
			dc.DrawGeometry(this.Background, (Pen)null, this.BackgroundGeometry);
		}
	}

	/// <summary>
	/// 绘制弧线
	/// </summary>
	/// <param name="context">context</param>
	/// <param name="endPoint">终点</param>
	/// <param name="radiusX">圆弧X</param>
	/// <param name="radiusY">圆弧Y</param>
	/// <param name="direction">绘制方向，0:逆时针，1:顺时针</param>
	protected void DrawArcTo(StreamGeometryContext context, Point endPoint, double radiusX, double radiusY, SweepDirection direction)
	{
		//终点、圆角半径XY，旋转角度、优弧Or劣弧、绘制方向、是否描边、是否连接前一线段
		context.ArcTo(endPoint, new Size(radiusX, radiusY), 0.0, false, direction, true, false);
	}

	/// <summary>
	/// 绘制直线
	/// </summary>
	/// <param name="context">context</param>
	/// <param name="endPoint">终点</param>
	protected void DrawLineTo(StreamGeometryContext context, Point endPoint)
	{
		//终点、是否描边、是否连接前一线段
		context.LineTo(endPoint, true, false);
	}

	/// <summary>
	/// 更新路径/UpdatePath，上下圆角半径一致
	/// </summary>
	/// <param name="containerWidth">容器宽度</param>
	/// <param name="containerHeight">容器高度</param>
	/// <param name="corner">圆角半径/Radius</param>
	/// <param name="compressCableLineLength">对默认连接线长度累加/Accumulate the default cable length.</param>
	/// <returns></returns>
	[Obsolete]
	private static Geometry UpdateTabGeometryBase(double containerWidth, double containerHeight, double corner = 4, int compressCableLineLength = 0)
	{
		//计算基于上一命令终点/Calculate based on the previous command's endpoint 
		double startPointX = 0;
		double cornerRx = corner;
		double cornerRy = cornerRx;
		//连接两段弧线的线段长度，默认占满空间时为height-cornerRy * 2,设置cableLine为-1则为占满空间-1的长度/Default cable length
		double lineLength = containerHeight - cornerRy * 2;
		//拒绝无效请求/Reject invalid request
		if (Math.Abs(compressCableLineLength) < lineLength)
		{
			lineLength += compressCableLineLength;
		}
		double leftArc1Y = containerHeight - cornerRy;
		double leftLine2Y = leftArc1Y - lineLength;
		double leftArc3StartPoint = cornerRy * 2;
		double leftArc3Y = containerHeight - cornerRy * 2 - lineLength;
		double middleLine4X = containerWidth - cornerRx * 2;
		double rightBaseX = middleLine4X + cornerRx;

		//释义explain path command:
		//M(Move):画笔移动到点(X,Y),不做绘制
		//A(Arc):从当前位置绘制弧线到指定位置:
		//		 椭圆X半径，椭圆Y半径;
		//		 X轴旋转度数(顺时针方向为正);
		//		 0劣弧(圆心角小于180)/1优弧(圆心角大于180);
		//		 起点到终点弧线(0逆时针/1顺时针)画线;
		//		 终点X坐标，终点Y坐标
		//L(Line):从当前位置绘制线段到指定位置
		//Z:闭合路径起始终点
		//参考路径/Reference path:width = 108, height = 20
		//M 0 20 A 4 4 0 0 0 4,16 L 4,4 A 4 4 0 0 1 8,0 L 100,0 A 4 4 0 0 1 104,4 L 104,16 A 4 4 0 0 0 108,20
		string xmlns = @"xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" ";
		string start = $"M{startPointX},{containerHeight} ";
		string leftArc1 = $"A{cornerRx},{cornerRy} 0 0 0 {cornerRx},{leftArc1Y} ";
		string leftLin2 = $"L{cornerRx},{leftLine2Y} ";
		string leftArc3 = $"A{cornerRx},{cornerRy} 0 0 1 {leftArc3StartPoint},{leftArc3Y} ";
		string middleLine = $"L{middleLine4X},{leftArc3Y} ";

		string rightArc3 = $"A{cornerRx},{cornerRy} 0 0 1 {rightBaseX},{leftLine2Y}";
		string rightLin2 = $"L{rightBaseX},{leftArc1Y} ";
		string rightArc1 = $"A{cornerRx},{cornerRy} 0 0 0 {rightBaseX + cornerRx},{containerHeight} ";
		string end = "Z";
		string pathTemplate = $@"<Path {xmlns} Data=""{start}{leftArc1}{leftLin2}{leftArc3}{middleLine}{rightArc3}{rightLin2}{rightArc1}{end}"" />";

		//conveter
		System.Windows.Shapes.Path geometry = System.Windows.Markup.XamlReader.Parse(pathTemplate) as System.Windows.Shapes.Path;
		return geometry?.Data??new StreamGeometry();
	}
	/// <summary>
	/// 更新路径/UpdatePath,相比<see cref="UpdateTabGeometryBase"/>，下圆角半径比上圆角大<paramref name="bottomCornerAdd"/>个单位,且左侧特殊处理
	/// </summary>
	/// <param name="containerWidth">容器宽度</param>
	/// <param name="containerHeight">容器高度</param>
	/// <param name="corner">圆角半径/Radius</param>
	/// <param name="compressCableLineLength">对默认连接线长度累加/Accumulate the default cable length.</param>
	/// <param name="bottomCornerAdd">下圆角半径比上圆角大多少个单位</param>
	/// <returns></returns>
	[Obsolete]
	private Geometry UpdateTabGeometrySpecial(double containerWidth, double containerHeight, double corner = 4, int compressCableLineLength = 0, int bottomCornerAdd = 2)
	{
		//var key = Tuple.Create(containerWidth, containerHeight);
		//if(_catch.TryGetValue(key,out var value))
		//{
		//	return value;
		//}
		//计算基于上一命令终点/Calculate based on the previous command's endpoint 
		double startPointX = 0;
		double cornerRx = corner;
		double cornerRy = cornerRx;
		//连接两段弧线的线段长度，默认占满空间时为height-cornerRy * 2,设置cableLine为-1则为占满空间-1的长度/Default cable length
		double lineLength = containerHeight - cornerRy * 2;
		//拒绝无效请求/Reject invalid request
		if (Math.Abs(compressCableLineLength) < lineLength)
		{
			lineLength += compressCableLineLength;
		}
		double leftArc1Y = containerHeight - cornerRy;
		double leftLine2Y = leftArc1Y - lineLength;
		double leftArc3StartPoint = cornerRy * 2;
		double leftArc3Y = containerHeight - cornerRy * 2 - lineLength;
		double middleLine4X = containerWidth - cornerRx * 2;
		double rightBaseX = middleLine4X + cornerRx;

		//释义explain path command:
		//M(Move):画笔移动到点(X,Y),不做绘制
		//A(Arc):从当前位置绘制弧线到指定位置:
		//		 椭圆X半径，椭圆Y半径;
		//		 X轴旋转度数(顺时针方向为正);
		//		 0劣弧(圆心角小于180)/1优弧(圆心角大于180);
		//		 起点到终点弧线(0逆时针/1顺时针)画线;
		//		 终点X坐标，终点Y坐标
		//L(Line):从当前位置绘制线段到指定位置
		//Z:闭合路径起始终点
		//参考路径/Reference path:width = 108, height = 20
		//M 0 20 A 4 4 0 0 0 4,16 L 4,4 A 4 4 0 0 1 8,0 L 100,0 A 4 4 0 0 1 104,4 L 104,16 A 4 4 0 0 0 108,20
		string xmlns = @"xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" ";
		string start = $"M{startPointX},{containerHeight} ";
		string leftArc1 = $"A{cornerRx},{cornerRy + bottomCornerAdd} 0 0 0 {cornerRx},{leftArc1Y - bottomCornerAdd} ";
		string leftLin2 = $"L{cornerRx},{leftLine2Y} ";
		string leftArc3 = $"A{cornerRx},{cornerRy} 0 0 1 {leftArc3StartPoint},{leftArc3Y} ";
		string middleLine = $"L{middleLine4X},{leftArc3Y} ";

		string rightArc3 = $"A{cornerRx},{cornerRy} 0 0 1 {rightBaseX},{leftLine2Y}";
		string rightLin2 = $"L{rightBaseX},{leftArc1Y - bottomCornerAdd} ";
		string rightArc1 = $"A{cornerRx},{cornerRy + bottomCornerAdd} 0 0 0 {rightBaseX + cornerRx},{containerHeight} ";
		//string end = "Z";
		string pathTemplate = string.Empty;
		//if (this.RowPos is RowPos.First)
		//{
		//	leftLin2 = $"L{startPointX},{leftLine2Y} ";
		//	leftArc3 = $"A{cornerRx},{cornerRy} 0 0 1 {cornerRy},{leftArc3Y} ";
		//	pathTemplate = $@"<Path {xmlns} Data=""{start}{leftLin2}{leftArc3}{middleLine}{rightArc3}{rightLin2}{rightArc1}"" />";
		//}
		//else
		{
			pathTemplate = $@"<Path {xmlns} Data=""{start}{leftArc1}{leftLin2}{leftArc3}{middleLine}{rightArc3}{rightLin2}{rightArc1}"" />";
		}
		if (Debugger.IsAttached)
		{
			Trace.WriteLine($"containerWidth:{containerWidth},containerHeight:{containerHeight},pathTemplate:{pathTemplate}");
		}
		//conveter
		System.Windows.Shapes.Path geometry = System.Windows.Markup.XamlReader.Parse(pathTemplate) as System.Windows.Shapes.Path;
		//_cache.Add(key, geometry.Data);
		return geometry?.Data ?? new StreamGeometry();
	}

	protected struct CurveInfo(Size size, Thickness border, Vector offset)
	{
		/// <summary>
		/// 容器Size
		/// </summary>
		public Size Size { get; } = size;

		/// <summary>
		/// 边框信息
		/// </summary>
		public Thickness Border { get; } = border;

		/// <summary>
		/// 起始偏移
		/// </summary>
		public Vector Offset { get; } = offset;

		public double LeftTop { get; set; } = 0.0;

		public double TopLeft { get; set; } = 0.0;

		public double TopRight { get; set; } = 0.0;

		public double RightTop { get; set; } = 0.0;

		public double RightBottom { get; set; } = 0.0;

		public double BottomRight { get; set; } = 0.0;

		public double BottomLeft { get; set; } = 0.0;

		public double LeftBottom { get; set; } = 0.0;

		public override string ToString()
		{
			return (@$"info.Size:{this.Size}, info.Offset:{this.Offset}, info.Border:{this.Border},
info.LeftTop:({LeftTop},{TopLeft}), info.TopRight:({TopRight},{RightTop}), info.RightBottom:({RightBottom},{BottomRight}), info.BottomLeft:({BottomLeft},{LeftBottom})");
		}
	}
}

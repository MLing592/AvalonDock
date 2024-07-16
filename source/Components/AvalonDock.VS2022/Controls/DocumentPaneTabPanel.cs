/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvalonDock.Controls
{
	/// <summary>
	/// Provides a panel that contains the TabItem Headers of the <see cref="LayoutDocumentPaneControl"/>.
	/// Used except by VS2022
	/// </summary>
	public class DocumentPaneTabOriginPanel : Panel
	{
		#region Constructors

		/// <summary>
		/// Static constructor
		/// </summary>
		public DocumentPaneTabOriginPanel()
		{
			this.FlowDirection = System.Windows.FlowDirection.LeftToRight;
		}

		#endregion Constructors

		#region Overrides

		//确定子元素大小需求并计算Panel大小,接收Size类型参数availableSize，表示Panel可用空间的大小
		//在方法内部，应该：
		//1.检查Panel是否为空以及存在子元素。如果没有子元素，您可以将可用大小作为理想大小返回，或者返回具有零高度和宽度的尺寸。
		//2.遍历子元素列表并使用每个子元素的DesiredSize属性来确定其测量大小向量。
		//3.基于子元素的大小计算出整个Panel的大小。
		protected override Size MeasureOverride(Size availableSize)
		{
			#region 原Panel
			Size desideredSize = new Size();
			foreach (FrameworkElement child in Children)//遍历所有子元素，并计算其大小需求
			{
				child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));// 测量子元素的大小需求
				desideredSize.Width += child.DesiredSize.Width;//累加子元素的宽度作为这个面板的理想宽度

				desideredSize.Height = Math.Max(desideredSize.Height, child.DesiredSize.Height);//计算出最大需要的高度，作为这个面板的理想高度
			}

			return new Size(Math.Min(desideredSize.Width, availableSize.Width), desideredSize.Height);//返回一个新尺寸，即最小控件面板和其子元素所需的尺寸
			#endregion
		}

		//确定每个子元素的最终位置，并设置其大小。按照布局策略排列子元素
		//在方法内部应该：
		//1.遍历所有子元素，并使用它们的MeasuredSize和位置偏移计算出每个子元素应该渲染的最终区域。
		//2.对于所有子元素，将这些最终区域指定为每个子元素的Arrange方法的输入参数之一。在这里，可以调整子元素的大小或位置，以满足特定的布局需求。此外，您还可以根据子元素的其他属性进行自定义处理，例如旋转或倾斜。
		//3.最后，返回Panel所需的最终尺寸，以便系统了解如何呈现整个Panel。
		protected override Size ArrangeOverride(Size finalSize)
		{
			#region 原Panel
			//遍历所有可见子元素
		   var visibleChildren = Children.Cast<UIElement>().Where(ch => ch.Visibility != System.Windows.Visibility.Collapsed);
			// 定义偏移量，并标记是否跳过所有其他元素
			var offset = 0.0;
			var skipAllOthers = false;
			foreach (TabItem doc in visibleChildren)
			{
				// 如果已经遍历到最后，或者在添加当前项之后剩余的空间不足，则标记后面的元素为隐藏
				if (skipAllOthers || offset + doc.DesiredSize.Width > finalSize.Width)
				{
					// 在当前项为选定内容并且未可见时，在布局中重新排列其他项以使其能够正确显示
					bool isLayoutContentSelected = false;
					var layoutContent = doc.Content as LayoutContent;
					if (layoutContent != null)
						isLayoutContentSelected = layoutContent.IsSelected;
					if (isLayoutContentSelected && !doc.IsVisible)
					{
						// 找到父容器和父选择器
						var parentContainer = layoutContent.Parent as ILayoutContainer;
						var parentSelector = layoutContent.Parent as ILayoutContentSelector;
						var parentPane = layoutContent.Parent as ILayoutPane;
						int contentIndex = parentSelector.IndexOf(layoutContent);
						if (contentIndex > 0 &&
							parentContainer.ChildrenCount > 1)
						{
							// 将选定的布局内容移到第一个位置
							parentPane.MoveChild(contentIndex, 0);
							parentSelector.SelectedContentIndex = 0;
							// 重新排列子元素以反映新的布局
							return ArrangeOverride(finalSize);
						}
					}
					doc.Visibility = System.Windows.Visibility.Hidden;
					skipAllOthers = true;
				}
				// 否则，将元素设置为可见，并使用其所需大小来安排它
				else
				{
					doc.Visibility = System.Windows.Visibility.Visible;
					doc.Arrange(new Rect(offset, 0.0, doc.DesiredSize.Width, finalSize.Height));
					offset += doc.ActualWidth + doc.Margin.Left + doc.Margin.Right;
				}
			}
			// 返回最终尺寸
			return finalSize;
			#endregion
		}


		protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
		{
			//if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
			//    LayoutDocumentTabItem.IsDraggingItem())
			//{
			//    var contentModel = LayoutDocumentTabItem.GetDraggingItem().Model;
			//    var manager = contentModel.Root.Manager;
			//    LayoutDocumentTabItem.ResetDraggingItem();
			//    System.Diagnostics.Trace.WriteLine("OnMouseLeave()");

			//    manager.StartDraggingFloatingWindowForContent(contentModel);
			//}

			base.OnMouseLeave(e);
		}

		#endregion Overrides

	}


	//used by VS2022
	//VS2022使用的自定义WrapPanel
	public class DocumentPaneTabPanel : WrapPanel
	{
		public DocumentPaneTabPanel() 
		{
			this.FlowDirection = System.Windows.FlowDirection.LeftToRight;
			this.Loaded += DocumentPaneTabPanel_Loaded;
		}

		private void DocumentPaneTabPanel_Loaded(object sender, RoutedEventArgs e)
		{
			//寻找documentPane
			var pane = (this.Children.OfType<TabItem>().FirstOrDefault()?.Content as LayoutDocument)?.FindParent<LayoutDocumentPane>();
			//LayoutDocumen is arranged in reverse order by fixed elements and moved to the front to return to positive order.
			//LayoutDocumen被固定的元素逆序排列且移到最前返回正序排列
			var LayoutDocumentElements = pane.Children.OfType<LayoutDocument>()
				.Where(x=>x.IsFixed)
				.OrderByDescending(p => pane.Children.IndexOf(p))
				.ToArray();
			for (int i = 0; i < LayoutDocumentElements.Count(); i++)
			{
				pane.MoveChild(pane.IndexOfChild(LayoutDocumentElements[i]), 0);
			}
			//Non-LayoutDocument is arranged in reverse order and moved to the front to return to positive order.
			//非LayoutDocument逆序排列且移到最前返回正序排列
			var unLayoutDocumentElements = pane.Children
				.Where(x => !(x is LayoutDocument))
				.OrderByDescending(p => pane.Children.IndexOf(p))
				.ToArray();
			for (int i = 0; i < unLayoutDocumentElements.Count(); i++)
			{
				pane.MoveChild(pane.IndexOfChild(unLayoutDocumentElements[i]), 0);
			}
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			//During initialization, an element is added, and the current element is visualAdded. When an element is moved, the current element is visualRemoved.
			//初始化时为添加元素，当前元素为visualAdded，移动元素时，当前元素为visualRemoved
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			double currentLineLength = 0;                
			double maxLineHeight = 0;                    
			double currentLineMaxHeight = 0;            
			var infiniteConstraintHgt = new Size(constraint.Width, double.PositiveInfinity);   


			foreach (UIElement child in InternalChildren)
			{
				bool needsNewLine = false;
				// if this element isn"t visible,skip
				// 如果该子元素不可见，则跳过
				if (!child.IsVisible) { continue; }

				// fixed LayoutDocument,new line
				// 固定文档另立一行
				var currentContent = (child as TabItem)?.Content;
				var ine = Math.Max(Children.IndexOf(child) - 1, 0);
				var frontContent = (Children[ine] as TabItem)?.Content;
				if ((currentContent is LayoutDocument current && frontContent is LayoutDocument front) && 
					(current?.IsFixed == false && front?.IsFixed == true))
				{
					needsNewLine = true;
				}

				// not LayoutDocument,new line
				// 非文档另立一行
				if (currentContent is LayoutDocument && !(frontContent is LayoutDocument))
				{
					needsNewLine = true;
				}

				// element cumulative width exceeeds constraint.Width
				// 元素累计宽度超出一行则换行
				if (currentLineLength + child.DesiredSize.Width > constraint.Width)
				{
					needsNewLine = true;
				}

				// needs new line,update maxLineHeight,currentLineLength and currentLineMaxHeight
				// 如果需要换行，则更新总高度、当前行最大高度和当前行宽度
				if (needsNewLine)
				{
					maxLineHeight += currentLineMaxHeight;
					currentLineLength = 0;
					currentLineMaxHeight = 0;
				}

				child.Measure(infiniteConstraintHgt);
				var childDesiredSize = child.DesiredSize;
				currentLineLength += childDesiredSize.Width;
				currentLineMaxHeight = Math.Max(currentLineMaxHeight, childDesiredSize.Height);
			}

			// Calculate the total height to get the final size
			// 将最后一行的最大高度加入总高度中，得到最终的尺寸大小
			maxLineHeight += currentLineMaxHeight;
			var finalSize = new Size(constraint.Width, Math.Min(maxLineHeight, constraint.Height));

			return finalSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			// Get base method size
			// 获取原生Wrappanel计算
			var baseSize = base.ArrangeOverride(finalSize);
			#region 自定义Wrappanel

			// current element X-axis position
			// 当前控件将要布局的子元素的 X 轴坐标
			double currentX = 0;
			//current element Y-axis position
			// 当前控件将要布局的子元素的 Y 轴坐标
			double currentY = 0; 
			double currentLineLength = 0; 
			double currentLineMaxHeight = 0; 
			int currentLineNumber = 0; 
			foreach (UIElement child in InternalChildren)
			{
				// if this element isn"t visible,skip
				// 如果该子元素不可见，则跳过
				if (!child.IsVisible) { continue; } 

				double width = child.DesiredSize.Width;
				double height = child.DesiredSize.Height;
				bool needsNewLine = false;

				var currentContent = (child as TabItem)?.Content;
				var ine = Math.Max(Children.IndexOf(child) - 1, 0);
				var frontContent = (Children[ine] as TabItem)?.Content;

				//Boundary between fixed and non-fixed documents,new line
				//固定与非固定文档的分界处换行
				if ((currentContent is LayoutDocument current && frontContent is LayoutDocument front) &&
					(current?.IsFixed == false && front?.IsFixed == true) ||
					(currentLineLength + width > finalSize.Width))
				{
					needsNewLine = true;
					currentLineNumber++; 
					currentY += currentLineMaxHeight; 
					currentLineLength = 0; 
					currentLineMaxHeight = 0; 					
				}

				// not LayoutDocument,new line
				// 非文档另立一行
				if (currentContent is LayoutDocument && !(frontContent is LayoutDocument))
				{
					needsNewLine = true;
					currentLineNumber++; 
					currentY += currentLineMaxHeight; 
					currentLineLength = 0; 
					currentLineMaxHeight = 0; 
				}

				// "If no line-break is needed, then it will be appended at the end of the current line."
				// 如果不需要换行，则排在当前行的尾部
				if (!needsNewLine) 
				{
					child.Arrange(new Rect(currentX, currentY, width, height));
					currentX += width;
					currentLineLength += width;
					currentLineMaxHeight = Math.Max(currentLineMaxHeight, height);
				}
				// "Otherwise, the element will be placed at the beginning of the next line."
				// 否则将子元素放到下一行的开头
				else
				{
					child.Arrange(new Rect(0, currentY + currentLineMaxHeight, width, height));
					currentX = width;
					currentLineLength = width;
					currentLineMaxHeight = height;
					baseSize.Height += currentLineMaxHeight; 
				}
			}
			#endregion
			return baseSize;

		} 
		
	}



}
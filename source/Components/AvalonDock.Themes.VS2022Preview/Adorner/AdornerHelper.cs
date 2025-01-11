using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace AvalonDock.Themes
{
	/// <summary>
	/// 装饰器辅助类
	/// 由于更改TabItem遮罩实现，现暂无使用
	/// </summary>
	public class AdornerHelper
	{
		public static UIElement GetTabItemAdorner(DependencyObject obj)
		{
			return (UIElement)obj.GetValue(TabItemAdornerContent);
		}
		public static void SetTabItemAdorner(DependencyObject obj, UIElement value)
		{
			obj.SetValue(TabItemAdornerContent, value);
		}
		public static readonly DependencyProperty TabItemAdornerContent =
	DependencyProperty.RegisterAttached("TabItemAdornerContent", typeof(UIElement), typeof(AdornerHelper), new PropertyMetadata(null, TabItemChangeEvent));

		private static void TabItemChangeEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var c = d as FrameworkElement;
			if (c == null)
				return;
			var adronerContent = e.NewValue as UIElement;
			if (!c.IsLoaded)
			{
				if (adronerContent != null)
				{
					RoutedEventHandler l = null;
					l = (s, E) =>
					{
						var content = GetTabItemAdorner(c);
						if (content != null)
						{
							var layer = AdornerLayer.GetAdornerLayer(c);
							if (layer == null)
								throw new Exception("GetAdornerLayer Failed! 获取控件装饰层失败，控件可能没有装饰层！");
							layer.Add(new TabItemAdorner((UIElement)c, (UIElement)e.NewValue));
						}
						c.Loaded -= l;
					};
					c.Loaded += l;
				}
			}
			else
			{
				var layer = AdornerLayer.GetAdornerLayer(d as Visual);
				if (layer == null)
					throw new Exception("GetAdornerLayer Failed! 获取控件装饰层失败，控件可能没有装饰层！");
				if (e.OldValue != null)
				{

					var adorners = layer.GetAdorners(c);
					foreach (var i in adorners)
					{
						if (i is TabItemAdorner)
						{
							var na = i as TabItemAdorner;
							if (na.Child == e.OldValue)
							{
								layer.Remove(i);
								break;
							}
						}
					}
				}
				if (adronerContent != null)
				{
					layer.Add(new TabItemAdorner((UIElement)c, (UIElement)e.NewValue));
				}
			}
		}
	}
}

using AvalonDock.Themes.VS2022Preview.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonDock.Themes
{
	public class TabItemAdorner : Adorner
	{
		UIElement _child;
		public static DockingManager dockingManager = null;
		private static ResourceDictionary dic = null;
		public TabItemAdorner(UIElement adornedElement, UIElement child) : base(adornedElement)
		{
			_child = child;
			AddVisualChild(_child);
		}
		public UIElement Child => _child;
		protected override Visual GetVisualChild(int index)
		{
			return _child;
		}
		protected override int VisualChildrenCount => 1;
		protected override Size ArrangeOverride(Size finalSize)
		{
			_child.Arrange(new Rect(new Point(0, 0), finalSize));
			return finalSize;
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if (_child is Rectangle rec)
			{
				// Todo:需要寻找DockingManager的资源字典
				if(dockingManager is null)
				{
					dockingManager = FindAncestor<DockingManager>(AdornedElement);
				}
				if(dockingManager is not null)
				{
					var color = SearchResourceDict(dockingManager.Resources);
					if(color is not null) rec.Stroke = (Brush)color;
				}
				
			}
		}
		public static T FindAncestor<T>(DependencyObject child) where T : DependencyObject
		{
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);
			while (parentObject != null)
			{
				T parent = parentObject as T;
				if (parent != null)
				{
					return parent;
				}
				parentObject = VisualTreeHelper.GetParent(parentObject);
			}
			return null;
		}

		private object SearchResourceDict(ResourceDictionary resourceDict, int SearchDepth = 3)
		{
			Func<IEnumerable<ResourceDictionary>, int, object> loopThroughDicts = null;
			loopThroughDicts = (dicts, level) =>
			{
				if (level > SearchDepth) return false;
				foreach (ResourceDictionary dict in dicts)
				{
					var key = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabUnselectedBackground");
					if (key != null)
					{
						return dict[key];
					}

					if (dict.MergedDictionaries.Count > 0)
					{
						return loopThroughDicts(dict.MergedDictionaries.OfType<ResourceDictionary>(), level + 1);
					}
				}
				return false;
			};
			if (dic is null)
			{
				return loopThroughDicts(resourceDict.MergedDictionaries.OfType<ResourceDictionary>(), 1);
			}
			else
			{
				var key = dic.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabUnselectedBackground");
				if (key != null)
				{
					return dic[key];
				}
				else
				{
					return null;
				}
			}
		}
	}
}

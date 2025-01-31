using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System.Drawing;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;

namespace AvalonDock.Helper
{
	public class ResourceHelper
	{
		private static SolidColorBrush defaultColor = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString("#9183EE") };
		//"Search resource dictionary, with a default maximum search depth of 3."
		//搜索文档井动态资源，最大搜索深度默认为3
		public static bool FindDocmentWellResource(ResourceDictionary resourceDict, ComponentResourceKey brushKey, int SearchDepth = 3)
		{
			Func<ResourceDictionary, bool> func = null;
			func = (dict) =>
			{
				//左侧矩形条颜色
				var leftKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabUnselectedRectangleBackground");
				//选中时文档Boder色
				var middleKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabSelectedActiveBackground");
				//选中时文档背景色
				var backgroundKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabSelectedBackground");

				//资源色
				var borderBrushKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId == brushKey.ResourceId);
				var brushBackgroundKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == $"{brushKey.ResourceId}_2");

				if (leftKey != null && middleKey != null && backgroundKey != null)
				{
					// 无色时特殊设置
					if(brushKey.ResourceId.ToString() == "DocumentWellTabBackground2E2E2E")
					{
						dict[leftKey] = dict[borderBrushKey];
						dict[middleKey] = defaultColor;
					}
					else
					{
						dict[leftKey] = dict[borderBrushKey];
						dict[middleKey] = dict[borderBrushKey];
					}

					dict[backgroundKey] = dict[brushBackgroundKey];
					return true;
				}
				return false;
			};
			return FindResourceDictionary(resourceDict, func, SearchDepth);
		}

		//"Search resource dictionary, with a default maximum search depth of 3."
		//搜索资源字典，最大搜索深度默认为3
		private static bool FindResourceDictionary(ResourceDictionary resourceDict, Func<ResourceDictionary, bool> condition, int SearchDepth = 3)
		{
			Func<IEnumerable<ResourceDictionary>, int, bool> loopThroughDicts = null;
			loopThroughDicts = (dicts, level) =>
			{
				if (level > SearchDepth) return false;
				foreach (ResourceDictionary dict in dicts)
				{
					if (condition(dict))
					{
						return true;
					}
					if (dict.MergedDictionaries.Count > 0)
					{
						return loopThroughDicts(dict.MergedDictionaries.OfType<ResourceDictionary>(), level + 1);
					}
				}
				return false;
			};
			return loopThroughDicts(resourceDict.MergedDictionaries.OfType<ResourceDictionary>(), 1);
		}
	}
}

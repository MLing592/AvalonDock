/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Commands;
using AvalonDock.Layout;
using AvalonDock.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AvalonDock.Controls
{
	/// <inheritdoc />
	/// <summary>
	/// This is a wrapper for around the custom Pane content view of <see cref="LayoutElement"/>.
	/// Implements the <see cref="AvalonDock.Controls.LayoutItem" />
	///
	/// All DPs implemented here can be bound in a corresponding style to control parameters
	/// in dependency properties via binding in MVVM.
	/// </summary>
	/// <seealso cref="AvalonDock.Controls.LayoutItem" />
	public class LayoutDocumentItem : LayoutItem
	{
		#region fields

		private LayoutDocument _document;   // The content of this item
		#region Add
		private ICommand _defaultChangeTabColorCommand;
		private ICommand _defaultFixCommand;
		#endregion

		#endregion fields

		#region Constructors

		/// <summary>Class constructor</summary>
		internal LayoutDocumentItem()
		{
		}

		#endregion Constructors

		#region Properties

		#region Description

		/// <summary><see cref="Description"/> dependency property.</summary>
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(LayoutDocumentItem),
					new FrameworkPropertyMetadata(null, OnDescriptionChanged));

		/// <summary>Gets/sets the description to display (in the <see cref="NavigatorWindow"/>) for the Pane item.</summary>
		[Bindable(true), Description("Gets/sets the description to display (in the NavigatorWindow) for the Pane item."), Category("Other")]
		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		/// <summary>Handles changes to the <see cref="Description"/> property.</summary>
		private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutDocumentItem)d).OnDescriptionChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="Description"/> property.</summary>
		protected virtual void OnDescriptionChanged(DependencyPropertyChangedEventArgs e) => _document.Description = (string)e.NewValue;

		#endregion Description

		#region ChangeTabColorCommand 改变选项卡颜色

		/// <summary><see cref="ChangeTabColorCommand"/> dependency property.</summary>
		public static readonly DependencyProperty ChangeTabColorCommandProperty = DependencyProperty.Register(nameof(ChangeTabColorCommand), typeof(ICommand), typeof(LayoutDocumentItem),
				new FrameworkPropertyMetadata(null, OnChangeTabColorCommandChanged, CoerceChangeTabColorCommandValue));

		/// <summary>Gets/sets the command to execute when user click the Pane close button.</summary>
		[Bindable(true), Description("Gets/sets the command to execute when user click the Pane command of change tab color ."), Category("Other")]
		public ICommand ChangeTabColorCommand
		{
			get => (ICommand)GetValue(ChangeTabColorCommandProperty);
			set => SetValue(ChangeTabColorCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="ChangeTabColorCommand"/> property.</summary>
		/// <summary>属性值更改时引发的回调函数 <see cref="ChangeTabColorCommand"/> property.</summary>
		private static void OnChangeTabColorCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		/// <summary>Coerces the <see cref="ChangeTabColorCommand"/>  value.</summary>
		/// <summary>用来强制限制属性值的范围或取值 <see cref="ChangeTabColorCommand"/>  value.</summary>
		private static object CoerceChangeTabColorCommandValue(DependencyObject d, object value) => value;

		/// <summary>
		/// Can be executeed or not
		/// 是否可以执行
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanExecuteChangeTabColorCommand(object parameter) => _document != null;

		/// <summary>
		/// executive command
		/// 执行命令
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void ExecuteChangeTabColorCommand(object parameter)
		{
			//Get Root Manager
			//获取根组件DockingManager
			DockingManager dockingManager = this.LayoutElement?.Root.Manager;
			if (dockingManager == null) return;

			//Get parameter to brush
			//转换brush
			var brush = parameter as SolidColorBrush;
			if (brush == null) return;

			//"To retrieve the key value of the theme resources and replace the resource color, you need to modify 'DockingManager.Theme'.
			//获取主题资源键值，更换资源颜色
			var result = SearchResourceDict(dockingManager.Resources, brush);
			if(!result) SearchResourceDict(Application.Current.Resources, brush);
		}

		//"Search resource dictionary, with a default maximum search depth of 3."
		//搜索资源字典，最大搜索深度默认为3
		private bool SearchResourceDict(ResourceDictionary resourceDict,SolidColorBrush brush,int SearchDepth = 3)
		{
			Func<IEnumerable<ResourceDictionary>, int,bool> loopThroughDicts = null;
			loopThroughDicts = (dicts, level) =>
			{
				if (level > SearchDepth) return false;
				foreach (ResourceDictionary dict in dicts)
				{
					var leftKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabUnselectedRectangleBackground");
					var middleKey = dict.Keys.OfType<ComponentResourceKey>().FirstOrDefault(k => k.ResourceId.ToString() == "DocumentWellTabSelectedActiveBackground");
					if (leftKey != null && middleKey != null)
					{
						dict[leftKey] = brush;
						dict[middleKey] = brush;
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


		#endregion ChangeTabColorCommand

		#region FixCommand 固定选项卡

		/// <summary><see cref="FixCommand"/> dependency property.</summary>
		public static readonly DependencyProperty FixCommandProperty = DependencyProperty.Register(nameof(FixCommand), typeof(ICommand), typeof(LayoutDocumentItem),
				new FrameworkPropertyMetadata(null, OnFixCommandChanged, CoerceFixCommandValue));

		/// <summary>Gets/sets the command to execute when user click the Pane close button.</summary>
		[Bindable(true), Description("Gets/sets the command to execute when user click the Document Tab command of fixed to sort ."), Category("Other")]
		public ICommand FixCommand
		{
			get => (ICommand)GetValue(FixCommandProperty);
			set => SetValue(FixCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="FixCommand"/> property.</summary>
		/// <summary>属性值更改时引发的回调函数 <see cref="FixCommand"/> property.</summary>
		private static void OnFixCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		/// <summary>Coerces the <see cref="FixCommand"/>  value.</summary>
		/// <summary>用来强制限制属性值的范围或取值 <see cref="FixCommand"/>  value.</summary>
		private static object CoerceFixCommandValue(DependencyObject d, object value) => value;

		/// <summary>
		/// Can be executeed or not
		/// 是否可以执行
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		private bool CanExecuteFixCommand(object parameter) => _document != null;

		/// <summary>
		/// executive command
		/// 执行命令
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void OnExecuteFixCommand(object parameter)
		{
			bool newValue = !_document.IsFixed;
			var pane = _document.FindParent<LayoutDocumentPane>();
			int index = pane.IndexOfChild(_document);
			int unfixedIndex = -1;
			int finalIndex = pane.Children.Count - 1;
			int lastfixedIndex = -1;

			var unfixedElements = pane.Children.OfType<LayoutDocument>().FirstOrDefault(x => !x.IsFixed);
			var lastfixedElements = pane.Children.OfType<LayoutDocument>().LastOrDefault(x => x.IsFixed);
			
			if (lastfixedElements != null) { lastfixedIndex = pane.IndexOfChild(lastfixedElements); }
			if (unfixedElements != null)
			{
				//不-1时移动到元素前
				unfixedIndex = newValue ? pane.IndexOfChild(unfixedElements) : pane.IndexOfChild(unfixedElements) - 1;
				//前后索引相同不会重新排列时
				if (index == unfixedIndex)
				{
					//1.强制容器重排
					//pane.RaiseChildrenTreeChanged();
					//2.文档移动再移回
					if(index == finalIndex) finalIndex= 0;
					pane.MoveChild(index, finalIndex);
					pane.MoveChild(finalIndex, index);
				}
				else
				{
					pane.MoveChild(index, unfixedIndex);
				}
			}
			else
			{
				//全部固定则移动到最后
				lastfixedIndex = Math.Min(finalIndex, Math.Max(0, lastfixedIndex));
				if (index == lastfixedIndex)
				{
					lastfixedIndex = 0;
					pane.MoveChild(index, lastfixedIndex);
					pane.MoveChild(lastfixedIndex, index);
				}
				else
				{
					pane.MoveChild(index, lastfixedIndex);
				}
					
			}
			
			_document.IsFixed = newValue;

			////非LayoutDocument逆序排列且移到最前返回正序排列
			//var unLayoutDocumentElements = pane.Children
			//	.Where(x => !(x is LayoutDocument))
			//	.OrderByDescending(p => pane.Children.IndexOf(p))
			//	.ToArray();
			//for (int i = 0; i < unLayoutDocumentElements.Count(); i++)
			//{
			//	pane.MoveChild(pane.IndexOfChild(unLayoutDocumentElements[i]), 0);
			//}
		}

		#endregion FixCommand

		#endregion Properties

		#region Overrides

		/// <inheritdoc />
		protected override void Close()
		{
			if (_document.Root?.Manager == null) return;
			var dockingManager = _document.Root.Manager;
			dockingManager.ExecuteCloseCommand(_document);
		}

		/// <inheritdoc />
		protected override void OnVisibilityChanged()
		{
			if (_document?.Root != null)
			{
				_document.IsVisible = Visibility == Visibility.Visible;
				if (_document.Parent is LayoutDocumentPane layoutDocumentPane) layoutDocumentPane.ComputeVisibility();
			}
			base.OnVisibilityChanged();
		}

		/// <inheritdoc />
		internal override void Attach(LayoutContent model)
		{
			_document = model as LayoutDocument;
			base.Attach(model);
		}

		/// <inheritdoc />
		internal override void Detach()
		{
			_document = null;
			base.Detach();
		}

		protected override bool CanExecuteDockAsDocumentCommand()
		{
			return (LayoutElement != null && LayoutElement.FindParent<LayoutDocumentPane>() != null && LayoutElement.IsFloating);
		}


		/// <summary>
		/// 初始化命令
		/// </summary>
		protected override void InitDefaultCommands()
		{
			#region Add
			_defaultChangeTabColorCommand = new RelayCommand<object>(ExecuteChangeTabColorCommand, CanExecuteChangeTabColorCommand);
			_defaultFixCommand = new RelayCommand<object>(OnExecuteFixCommand, CanExecuteFixCommand);
			#endregion
			base.InitDefaultCommands();
		}

		/// <summary>
		/// 卸载控件时清除命令的默认绑定
		/// </summary>
		protected override void ClearDefaultBindings()
		{
			#region Add
			if (ChangeTabColorCommand == _defaultChangeTabColorCommand) BindingOperations.ClearBinding(this, ChangeTabColorCommandProperty);
			if (FixCommand == _defaultFixCommand) BindingOperations.ClearBinding(this, FixCommandProperty);
			#endregion
			base.ClearDefaultBindings();
		}

		/// <summary>
		/// 控件首次加载或需重新绑定时调用，设置命令的默认绑定
		/// </summary>
		protected override void SetDefaultBindings()
		{
			#region Add
			if (ChangeTabColorCommand == null) ChangeTabColorCommand = _defaultChangeTabColorCommand;
			if (FixCommand == null) FixCommand = _defaultFixCommand;
			#endregion
			base.SetDefaultBindings();
		}
		#endregion Overrides
	}
}
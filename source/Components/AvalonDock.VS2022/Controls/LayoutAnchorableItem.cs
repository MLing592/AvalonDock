/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using AvalonDock.Commands;
using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace AvalonDock.Controls
{
	/// <inheritdoc />
	/// <summary>
	/// This is a wrapper for around the custom anchorable content view of <see cref="LayoutElement"/>.
	/// Implements the <see cref="AvalonDock.Controls.LayoutItem" />
	///
	/// All DPs implemented here can be bound in a corresponding style to control parameters
	/// in dependency properties via binding in MVVM.
	/// </summary>
	/// <seealso cref="AvalonDock.Controls.LayoutItem" />
	public class LayoutAnchorableItem : LayoutItem
	{
		#region fields

		private LayoutAnchorable _anchorable;   // The content of this item
		private ICommand _defaultHideCommand;
		private ICommand _defaultAutoHideCommand;
		private ICommand _defaultDockCommand;
		private readonly ReentrantFlag _visibilityReentrantFlag = new ReentrantFlag();
		private readonly ReentrantFlag _anchorableVisibilityReentrantFlag = new ReentrantFlag();

		#endregion fields

		#region Constructors
		static LayoutAnchorableItem()
		{
			// #182: LayoutAnchorable initializes with CanClose == false. We therefore also override the metadata for LayoutAnchorableItem to match this.
			// Only the default value will be overriden. PropertyChangedCallbacks, etc should be unaffected.
			CanCloseProperty.OverrideMetadata(typeof(LayoutAnchorableItem), new FrameworkPropertyMetadata(false));
		}
		/// <summary>Class constructor</summary>
		internal LayoutAnchorableItem()
		{
		}

		#endregion Constructors

		#region Properties

		#region HideCommand 隐藏

		/// <summary><see cref="HideCommand"/> dependency property.</summary>
		public static readonly DependencyProperty HideCommandProperty = DependencyProperty.Register(nameof(HideCommand), typeof(ICommand), typeof(LayoutAnchorableItem),
				new FrameworkPropertyMetadata(null, OnHideCommandChanged, CoerceHideCommandValue));

		/// <summary>Gets/sets the the command to execute when an anchorable is hidden.</summary>
		[Bindable(true), Description("Gets/sets the the command to execute when an anchorable is hidden."), Category("Other")]
		public ICommand HideCommand
		{
			get => (ICommand)GetValue(HideCommandProperty);
			set => SetValue(HideCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="HideCommand"/> property.</summary>
		private static void OnHideCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutAnchorableItem)d).OnHideCommandChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="HideCommand"/> property.</summary>
		protected virtual void OnHideCommandChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		/// <summary>Coerces the <see cref="HideCommand"/> value.</summary>
		private static object CoerceHideCommandValue(DependencyObject d, object value) => value;

		private bool CanExecuteHideCommand(object parameter) => LayoutElement != null && _anchorable.CanHide;

		private void ExecuteHideCommand(object parameter) => _anchorable?.Root?.Manager?.ExecuteHideCommand(_anchorable);

		#endregion HideCommand

		#region HideCommand 隐藏

		/// <summary><see cref="AutoHideCommand"/> dependency property.</summary>
		public static readonly DependencyProperty AutoHideCommandProperty = DependencyProperty.Register(nameof(AutoHideCommand), typeof(ICommand), typeof(LayoutAnchorableItem),
				new FrameworkPropertyMetadata(null, OnAutoHideCommandChanged, CoerceAutoHideCommandValue));

		/// <summary>Gets/sets the the command to execute when an anchorable is hidden.</summary>
		[Bindable(true), Description("Gets/sets the the command to execute when an anchorable is hidden."), Category("Other")]
		public ICommand AutoHideCommand
		{
			get => (ICommand)GetValue(AutoHideCommandProperty);
			set => SetValue(AutoHideCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="AutoHideCommand"/> property.</summary>
		private static void OnAutoHideCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutAnchorableItem)d).OnAutoHideCommandChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="HideCommand"/> property.</summary>
		protected virtual void OnAutoHideCommandChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		/// <summary>Coerces the <see cref="HideCommand"/> value.</summary>
		private static object CoerceAutoHideCommandValue(DependencyObject d, object value) => value;

		private bool CanExecuteAutoHideCommand(object parameter) => LayoutElement != null && _anchorable.CanHide;

		private void ExecuteAutoHideCommand(object parameter) => _anchorable?.Root?.Manager?.ExecuteHideCommand(_anchorable);

		#endregion HideCommand

		#region ChangeTabColorCommand 改变选项卡颜色

		/// <summary><see cref="ChangeTabColorCommand"/> dependency property.</summary>
		public static readonly DependencyProperty ChangeTabColorCommandProperty = DependencyProperty.Register(nameof(ChangeTabColorCommand), typeof(ICommand), typeof(LayoutAnchorableItem),
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
		private bool CanExecuteChangeTabColorCommand(object parameter) => _anchorable != null;

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
			if (!result) SearchResourceDict(Application.Current.Resources, brush);
		}

		//"Search resource dictionary, with a default maximum search depth of 3."
		//搜索资源字典，最大搜索深度默认为3
		private bool SearchResourceDict(ResourceDictionary resourceDict, SolidColorBrush brush, int SearchDepth = 3)
		{
			Func<IEnumerable<ResourceDictionary>, int, bool> loopThroughDicts = null;
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
		public static readonly DependencyProperty FixCommandProperty = DependencyProperty.Register(nameof(FixCommand), typeof(ICommand), typeof(LayoutAnchorableItem),
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
		private bool CanExecuteFixCommand(object parameter) => _anchorable != null;

		/// <summary>
		/// executive command
		/// 执行命令
		/// </summary>
		/// <param name="parameter"></param>
		protected virtual void OnExecuteFixCommand(object parameter)
		{

		}

		#endregion FixCommand

		#region DockCommand

		/// <summary><see cref="DockCommand"/> dependency property.</summary>
		public static readonly DependencyProperty DockCommandProperty = DependencyProperty.Register(nameof(DockCommand), typeof(ICommand), typeof(LayoutAnchorableItem),
				new FrameworkPropertyMetadata(null, OnDockCommandChanged, CoerceDockCommandValue));

		/// <summary>Gets/sets the command to execute when user click the Dock button.</summary>
		/// <remarks>By default this command moves the anchorable inside the container pane which previously hosted the object.</remarks>
		[Bindable(true), Description("Gets/sets the command to execute when user click the Dock button."), Category("Other")]
		public ICommand DockCommand
		{
			get => (ICommand)GetValue(DockCommandProperty);
			set => SetValue(DockCommandProperty, value);
		}

		/// <summary>Handles changes to the <see cref="DockCommand"/> property.</summary>
		private static void OnDockCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutAnchorableItem)d).OnDockCommandChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="DockCommand"/> property.</summary>
		protected virtual void OnDockCommandChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		/// <summary>Coerces the <see cref="DockCommand"/> value.</summary>
		private static object CoerceDockCommandValue(DependencyObject d, object value) => value;

		private bool CanExecuteDockCommand(object parameter) => LayoutElement?.FindParent<LayoutAnchorableFloatingWindow>() != null;

		private void ExecuteDockCommand(object parameter) => LayoutElement.Root.Manager.ExecuteDockCommand(_anchorable);

		#endregion DockCommand

		#region CanHide

		/// <summary><see cref="CanHide"/> dependency property.</summary>
		public static readonly DependencyProperty CanHideProperty = DependencyProperty.Register(nameof(CanHide), typeof(bool), typeof(LayoutAnchorableItem), new FrameworkPropertyMetadata((bool)true,
					OnCanHideChanged));

		/// <summary>Gets/sets wether the user can hide the anchorable item.</summary>
		[Bindable(true), Description("Gets/sets wether the user can hide the anchorable item."), Category("Anchorable")]
		public bool CanHide
		{
			get => (bool)GetValue(CanHideProperty);
			set => SetValue(CanHideProperty, value);
		}

		/// <summary>Handles changes to the <see cref="CanHide"/> property.</summary>
		private static void OnCanHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutAnchorableItem)d).OnCanHideChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="CanHide"/> property.</summary>
		protected virtual void OnCanHideChanged(DependencyPropertyChangedEventArgs e)
		{
			if (_anchorable != null) _anchorable.CanHide = (bool)e.NewValue;
		}

		#endregion CanHide

		#region CanMove

		/// <summary><see cref="CanMove"/> dependency property.</summary>
		public static readonly DependencyProperty CanMoveProperty = DependencyProperty.Register(nameof(CanMove), typeof(bool), typeof(LayoutAnchorableItem), new FrameworkPropertyMetadata((bool)true,
					OnCanMoveChanged));

		/// <summary>Gets/sets wether the user can hide the anchorable item.</summary>
		[Bindable(true), Description("Gets/sets wether the user can hide the anchorable item."), Category("Anchorable")]
		public bool CanMove
		{
			get => (bool)GetValue(CanMoveProperty);
			set => SetValue(CanMoveProperty, value);
		}

		/// <summary>Handles changes to the <see cref="CanMove"/> property.</summary>
		private static void OnCanMoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((LayoutAnchorableItem)d).OnCanHideChanged(e);

		/// <summary>Provides derived classes an opportunity to handle changes to the <see cref="CanMove"/> property.</summary>
		protected virtual void OnCanMoveChanged(DependencyPropertyChangedEventArgs e)
		{
			if (_anchorable != null) _anchorable.CanMove = (bool)e.NewValue;
		}

		#endregion CanMove

		#endregion Properties

		#region Overrides

		internal override void Attach(LayoutContent model)
		{
			_anchorable = model as LayoutAnchorable;
			_anchorable.IsVisibleChanged += _anchorable_IsVisibleChanged;
			base.Attach(model);
		}

		internal override void Detach()
		{
			_anchorable.IsVisibleChanged -= _anchorable_IsVisibleChanged;
			_anchorable = null;
			base.Detach();
		}

		/// <inheritdoc />
		protected override bool CanExecuteDockAsDocumentCommand()
		{
			var canExecute = base.CanExecuteDockAsDocumentCommand();
			if (canExecute && _anchorable != null) return _anchorable.CanDockAsTabbedDocument;
			return canExecute;
		}

		/// <inheritdoc />
		protected override void Close()
		{
			if (_anchorable.Root?.Manager == null) return;
			var dockingManager = _anchorable.Root.Manager;
			dockingManager.ExecuteCloseCommand(_anchorable);
		}

		/// <inheritdoc />
		protected override void InitDefaultCommands()
		{
			_defaultHideCommand = new RelayCommand<object>(ExecuteHideCommand, CanExecuteHideCommand);
			_defaultAutoHideCommand = new RelayCommand<object>(ExecuteAutoHideCommand, CanExecuteAutoHideCommand);
			_defaultDockCommand = new RelayCommand<object>(ExecuteDockCommand, CanExecuteDockCommand);
			base.InitDefaultCommands();
		}

		/// <inheritdoc />
		protected override void ClearDefaultBindings()
		{
			if (HideCommand == _defaultHideCommand) BindingOperations.ClearBinding(this, HideCommandProperty);
			if (AutoHideCommand == _defaultAutoHideCommand) BindingOperations.ClearBinding(this, AutoHideCommandProperty);
			if (DockCommand == _defaultDockCommand) BindingOperations.ClearBinding(this, DockCommandProperty);
			base.ClearDefaultBindings();
		}

		/// <inheritdoc />
		protected override void SetDefaultBindings()
		{
			if (HideCommand == null) HideCommand = _defaultHideCommand;
			if (AutoHideCommand == null) AutoHideCommand = _defaultAutoHideCommand;
			if (DockCommand == null) DockCommand = _defaultDockCommand;
			Visibility = _anchorable.IsVisible ? Visibility.Visible : Visibility.Hidden;
			base.SetDefaultBindings();
		}

		/// <inheritdoc />
		protected override void OnVisibilityChanged()
		{
			if (_anchorable?.Root != null && _visibilityReentrantFlag.CanEnter)
			{
				using (_visibilityReentrantFlag.Enter())
				{
					switch (Visibility)
					{
						case Visibility.Hidden: case Visibility.Collapsed: _anchorable.HideAnchorable(false); break;
						case Visibility.Visible: _anchorable.Show(); break;
					}
				}
			}
			base.OnVisibilityChanged();
		}

		#endregion Overrides

		#region Private Methods

		private void _anchorable_IsVisibleChanged(object sender, EventArgs e)
		{
			if (_anchorable?.Root == null || !_anchorableVisibilityReentrantFlag.CanEnter) return;
			using (_anchorableVisibilityReentrantFlag.Enter())
			{
				Visibility = _anchorable.IsVisible ? Visibility.Visible : Visibility.Hidden;
			}
		}


		#endregion Private Methods
	}
}
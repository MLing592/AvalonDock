/************************************************************************
   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using System;
using System.Windows.Media;

namespace AvalonDock.Themes;
/// <inheritdoc/>
public class VS2022PreviewPreviewDarkTheme : Theme
{
	/// <inheritdoc/>
	public override Uri GetResourceUri()
	{
		return new Uri(
			"/AvalonDock.Themes.VS2022PreviewPreview;component/DarkTheme.xaml",
			UriKind.Relative);
	}

}

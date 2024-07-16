<a>English</a>
| Downloads                                                                                                                                               | NuGet Packages
| :------------------------------------------------------------------------------------------------------------------------------------------------------ | :--------------------------------------------------------------------------------
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.svg)](http://nuget.org/packages/Dirkster.AvalonDock)                                      | [Dirkster.AvalonDock](http://nuget.org/packages/Dirkster.AvalonDock)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Aero.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Aero)              | [Dirkster.AvalonDock.Themes.Aero](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Aero)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Expression.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Expression)  | [Dirkster.AvalonDock.Themes.Expression](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Expression)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.Metro.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Metro)            | [Dirkster.AvalonDock.Themes.Metro](http://nuget.org/packages/Dirkster.AvalonDock.Themes.Metro)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.VS2010.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2010)          | [Dirkster.AvalonDock.Themes.VS2010](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2010)
| [![NuGet](https://img.shields.io/nuget/dt/Dirkster.AvalonDock.Themes.VS2013.svg)](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2013)          | [Dirkster.AvalonDock.Themes.VS2013](http://nuget.org/packages/Dirkster.AvalonDock.Themes.VS2013) (see [Wiki](https://github.com/Dirkster99/AvalonDock/wiki/WPF-VS-2013-Dark-Light-Demo-Client) )
| [![NuGet](https://img.shields.io/nuget/dt/ML592.AvalonDock.Themes.VS2022.svg)](https://www.nuget.org/packages/ML592.AvalonDock.Themes.VS2022)          | [ML592.AvalonDock.Themes.VS2022](https://www.nuget.org/packages/ML592.AvalonDock.Themes.VS2022)

![Net4](https://badgen.net/badge/Framework/.Net&nbsp;4.8/blue) ![NetCore3](https://badgen.net/badge/NetCore/NetCore&nbsp;3.0/yellow) ![Net4](https://badgen.net/badge/NetCore/.NET&nbsp;5.0/orange)


## Theming

Using the *AvalonDock.Themes.VS2022* theme is very easy with *Dark* and *Light* and *Blue* themes.
Just load *Light* or *Dark* or *Blue* brush resources in you resource dictionary to take advantage of existing definitions.

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/DarkBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/LightBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/BlueBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

These definitions do not theme all controls used within this library. You should use a standard theming library, such as:
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro),
- [MLib](https://github.com/Dirkster99/MLib), or
- [MUI](https://github.com/firstfloorsoftware/mui)

to also theme standard elements, such as, button and textblock etc.

# easy to use for example

1. in nuget,install this package: ML592.AvalonDock.Themes.VS2022,include ML592.AvalonDock

2. in App.xaml :

 ```XAML
     <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/DarkBrushs.xaml" />
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/LightBrushs.xaml" />
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022;component/BlueBrushs.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

1. in MainWindow.xaml ：

```
    <Grid>
        <DockingManager>
            <DockingManager.Theme>
                <VS2022DarkTheme />
            </DockingManager.Theme>
            <LayoutRoot>
                <LayoutPanel Orientation="Horizontal">
                    <LayoutPanel Orientation="Vertical">
                        <LayoutDocumentPane>
                            <LayoutDocument Title="doc1.css" Content="123" ContentId="doc1" />
                            <LayoutDocument Title="doc2.css" Content="123" ContentId="doc2" />
                            <LayoutDocument Title="doc3.css" ContentId="doc3" />
                            <LayoutDocument Title="doc4.css" ContentId="doc4" />
                            <LayoutDocument Title="doc5.css" ContentId="doc5" />
                            <LayoutDocument Title="doc6.css" ContentId="doc6" />
                            <LayoutDocument Title="doc7.css" ContentId="doc7" />
                            <LayoutDocument Title="doc8.css" ContentId="doc8" />
                            <LayoutDocument Title="doc9.css" ContentId="doc9" />
                            <LayoutDocument Title="doc10.css" ContentId="doc10" />
                            <LayoutDocument Title="doc11.css" ContentId="doc11" />
                            <LayoutDocument Title="doc12.css" ContentId="doc12" />
                            <LayoutDocument Title="doc13.css" ContentId="doc13" />
                            <LayoutDocument Title="doc14.css" ContentId="doc14" />
                            <LayoutDocument Title="doc15.css" ContentId="doc15" />
                            <LayoutDocument Title="doc16.css" ContentId="doc16" />
                            <LayoutDocument Title="doc17.css" ContentId="doc17" />
                            <LayoutDocument Title="doc18.css" ContentId="doc18" />
                            <LayoutDocument Title="doc19.css" ContentId="doc19" />
                            <LayoutDocument Title="doc20.css" ContentId="doc20" />
                            <LayoutDocument Title="doc21.css" ContentId="doc21" />
                            <LayoutDocument Title="doc22.css" ContentId="doc22" />
                            <LayoutDocument Title="doc23.css" ContentId="doc23" />
                            <LayoutDocument Title="doc24.css" ContentId="doc24" />
                            <LayoutDocument Title="doc25.css" Content="doc25" ContentId="doc25" />
                        </LayoutDocumentPane>

                        <LayoutAnchorablePaneGroup DockHeight="128" Orientation="Horizontal">
                            <LayoutAnchorablePane Name="ErrorsPane" />
                            <LayoutAnchorablePane Name="OutputPane" />
                        </LayoutAnchorablePaneGroup>
                    </LayoutPanel>

                    <LayoutAnchorablePaneGroup DockWidth="256" Orientation="Vertical">
                        <LayoutAnchorablePane Name="ExplorerPane" DockHeight="2*" />
                        <LayoutAnchorablePane Name="PropertiesPane" />
                    </LayoutAnchorablePaneGroup>
                </LayoutPanel>
            </LayoutRoot>
        </DockingManager>
    </Grid>
```

4.Thanks,usage the same as [Dikster99 AvalonDock 4.72.0](https://github.com/Dirkster99/AvalonDock/tree/v4.72.0).



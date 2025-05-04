## Theming

Using the *AvalonDock.Themes.VS2022Preview* theme is very easy with *Dark* and *Light* and *Blue* themes.
Just load *Light* or *Dark* or *Blue* brush resources in you resource dictionary to take advantage of existing definitions.

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/DarkBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/LightBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/BlueBrushs.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

These definitions do not theme all controls used within this library. You should use a standard theming library, such as:
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro),
- [MLib](https://github.com/Dirkster99/MLib), or
- [MUI](https://github.com/firstfloorsoftware/mui)

to also theme standard elements, such as, button and textblock etc.

# easy to use for example

1. in nuget,install this package: ML592.AvalonDock.Themes.VS2022Preview,include ML592.AvalonDock

2. in App.xaml :

 ```XAML
     <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/DarkBrushs.xaml" />
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/LightBrushs.xaml" />
                <ResourceDictionary Source="/AvalonDock.Themes.VS2022Preview;component/BlueBrushs.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

1. in MainWindow.xaml ：

```
    <Grid>
        <DockingManager>
            <DockingManager.Theme>
                <VS2022PreviewDarkTheme />
            </DockingManager.Theme>
            <LayoutRoot>
                <LayoutPanel Orientation="Horizontal">
                    <LayoutPanel Orientation="Vertical">
                        <LayoutDocumentPane>
                            <LayoutDocument Title="doc1" Content="doc1" ContentId="doc1" />
                            <LayoutDocument Title="doc2" Content="doc2" ContentId="doc2" />
                            <LayoutDocument Title="doc3" Content="doc3" ContentId="doc3" />
                            <LayoutDocument Title="doc4" Content="doc4" ContentId="doc4" />
                            <LayoutDocument Title="doc5" Content="doc5" ContentId="doc5" />
                            <LayoutDocument Title="doc6" Content="doc6" ContentId="doc6" />
                            <LayoutDocument Title="doc7" Content="doc7" ContentId="doc7" />
                            <LayoutDocument Title="doc8" Content="doc8" ContentId="doc8" />
                            <LayoutDocument Title="doc9" Content="doc9" ContentId="doc9" />
                            <LayoutDocument Title="doc10" Content="doc10" ContentId="doc10" />
                            <LayoutDocument Title="doc11" Content="doc11" ContentId="doc11" />
                            <LayoutDocument Title="doc12" Content="doc12" ContentId="doc12" />
                            <LayoutDocument Title="doc13" Content="doc13" ContentId="doc13" />
                            <LayoutDocument Title="doc14" Content="doc14" ContentId="doc14" />
                            <LayoutDocument Title="doc15" Content="doc15" ContentId="doc15" />
                            <LayoutDocument Title="doc16" Content="doc16" ContentId="doc16" />
                            <LayoutDocument Title="doc17" Content="doc17" ContentId="doc17" />
                            <LayoutDocument Title="doc18" Content="doc18" ContentId="doc18" />
                            <LayoutDocument Title="doc19" Content="doc19" ContentId="doc19" />
                            <LayoutDocument Title="doc20" Content="doc20" ContentId="doc20" />
                            <LayoutDocument Title="doc21" Content="doc21" ContentId="doc21" />
                            <LayoutDocument Title="doc22" Content="doc22" ContentId="doc22" />
                            <LayoutDocument Title="doc23" Content="doc23" ContentId="doc23" />
                            <LayoutDocument Title="doc24" Content="doc24" ContentId="doc24" />
                            <LayoutDocument Title="doc25" Content="doc25" ContentId="doc25" />
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



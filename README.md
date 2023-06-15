# IVisualTreeElement and XAML Hot Reload.

The following example project shows you how to implement IVisualTreeElement to a custom VisualElement, allowing XAML Hot Reload to access and edit its data template.

---

[Incremental XAML Hot Reload support for Xamarin Forms and MAUI](https://learn.microsoft.com/en-us/dotnet/maui/xaml/hot-reload?tabs=vswin) operates differently from other systems, including Full Page XAML Hot Reload. Most systems replace existing pages wholesale by replacing the existing page with a new construction based on the modified XAML. Incremental XAML Hot Reload attempts to alter the property value you changed and allow the underlying platform to apply the changes through the rest of the application.

This approach has several benefits: Suppose you include UI logic in your page's constructor or OnPageAppearing. In that case, we will not invoke it, allowing you to reuse existing ViewModels and not worry about pulling down new data to see your changes. Likewise, you will stay in your place if you modify an ItemTemplate within a ListView or CollectionView. 

For this system to operate reliably, we need a consistent way to walk the MAUI Visual Tree, allowing us to modify individual elements and styles as referenced within your application. For Xamarin Forms, we would rely on LogicalChildren for this support, but this needed to be improved. Some controls, like custom controls and Forms/MAUI controls like ListView, would not reference their children in LogicalChildren, but within its internal list for each platform implementation or in a data template. We could do deep heuristics for detecting child elements, but this would be slow and fragile.

The XAML Hot Reload team introduced [IVisualTreeElement](https://github.com/dotnet/maui/blob/main/src/Core/src/Core/IVisualTreeElement.cs) to MAUI to solve this problem. IVisualTreeElement is an interface for walking the Visual Tree and is the baseline element type for what kinds of elements we can modify within the system. MAUIs' Element contains a base-level implementation, so most controls should work out of the box. MAUI controls that include children outside the default Element implementation can implement and override this implementation to add them. We could add common-level support by introducing IVisualTreeElement and implementing it on top of controls like ListView. 
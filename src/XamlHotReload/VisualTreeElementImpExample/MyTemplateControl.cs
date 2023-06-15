namespace VisualTreeElementImpExample;

/// <summary>
/// Original MyTemplateControl.
/// This is missing IVisualTreeElement implementation.
/// This WILL NOT work with XAML Hot Reload.
/// </summary>
// public class MyTemplatedControl : VisualElement
// {
//     public static readonly BindableProperty ItemTemplateProperty =
//         BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(MyTemplatedControl), null, propertyChanged: OnItemTemplateChanged);
//
//     public DataTemplate ItemTemplate {
//         get { return (DataTemplate)GetValue(ItemTemplateProperty); }
//         set { SetValue(ItemTemplateProperty, value); }
//     }
//
//     public VisualElement Item { get; set; }
//
//     static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue) {
//         Console.WriteLine($"ItemTemplate changed from {oldValue} to {newValue}!");
//     }
// }

/// <summary>
/// Updated implementation of MyTemplateControl with IVisualTreeElement support.
/// This sets the embedded DataTemplate to appear in the visual tree and allows XAML Hot Reload to access it.
/// </summary>
public class MyTemplatedControl : VisualElement, IVisualTreeElement
{
    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(MyTemplatedControl), null, propertyChanged: OnItemTemplateChanged);

    public DataTemplate ItemTemplate {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public VisualElement Item { get; set; }

    static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue) {
        Console.WriteLine($"ItemTemplate changed from {oldValue} to {newValue}!");
    }
    
    public IReadOnlyList<IVisualTreeElement> GetVisualChildren()
    {
        // On its own, this control has no visual children beyond the data template, so we send a new list
        // with only the data template as the child.
        // For other use cases, you may need to include the base implementation elements as well.
        return new List<IVisualTreeElement> { Item };
    }
}
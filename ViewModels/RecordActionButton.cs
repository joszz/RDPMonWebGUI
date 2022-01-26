namespace RDPMonWebGUI.ViewModels;

public class RecordActionButton
{
    /// <summary>
    /// The Controller to call when pressing this button. Defaults to "Home".
    /// </summary>
    public string Controller { get; set; } = "Home";

    /// <summary>
    /// The Action to call when pressing this button.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// The CSS class to apply to this button.
    /// </summary>
    public string CssClass { get; set; }

    /// <summary>
    /// Additional HTML attributes to set on the button.
    /// </summary>
    public List<string> Attributes { get; set; } = new List<string>();
}

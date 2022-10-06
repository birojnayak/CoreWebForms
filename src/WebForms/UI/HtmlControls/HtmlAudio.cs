// MIT License.

using System.ComponentModel;

namespace System.Web.UI.HtmlControls;
/// <devdoc>
///    <para>
///       The <see langword='HtmlAudio'/>
///       class defines the methods, properties, and events
///       for the HtmlAudio server control.
///       This class provides programmatic access on the server to
///       the HTML 5 &lt;audio&gt; element.
///    </para>
/// </devdoc>
public class HtmlAudio : HtmlContainerControl
{

    /// <devdoc>
    /// <para>Initializes a new instance of the <see cref='System.Web.UI.HtmlControls.HtmlAudio'/> class.</para>
    /// </devdoc>
    public HtmlAudio()
        : base("audio")
    {
    }

    /// <devdoc>
    ///    <para>
    ///       Gets or sets the name of and path to the video file to be displayed. This can be an absolute or relative path.
    ///    </para>
    /// </devdoc>
    [
    WebCategory("Behavior"),
    DefaultValue(""),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
    UrlProperty()
    ]
    public string Src
    {
        get
        {
            string s = Attributes["src"];
            return s ?? String.Empty;
        }
        set
        {
            Attributes["src"] = MapStringAttributeToString(value);
        }
    }

    /*
     * Override to process src attribute
     */
    protected override void RenderAttributes(HtmlTextWriter writer)
    {
        PreProcessRelativeReferenceAttribute(writer, "src");
        base.RenderAttributes(writer);
    }

}
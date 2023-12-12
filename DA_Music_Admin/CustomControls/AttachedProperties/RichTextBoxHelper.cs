using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CustomControls.AttachedProperties
{
    internal class RichTextBoxHelper : DependencyObject
    {
        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentXamlProperty, value);
        }

        public static readonly DependencyProperty DocumentXamlProperty =
            DependencyProperty.RegisterAttached(
                "DocumentXaml",
                typeof(string),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = (obj, e) =>
                    {
                        var richTextBox = (RichTextBox)obj;

                        FlowDocument document = new FlowDocument();

                        // Create a paragraph and add it to the document
                        Paragraph paragraph = new Paragraph(new Run(GetDocumentXaml(richTextBox)));
                        document.Blocks.Add(paragraph);

                        // Set the document as the content of the RichTextBox
                        richTextBox.Document = document;

                        // When the document changes update the source
                       
                    }
                });
    }
}

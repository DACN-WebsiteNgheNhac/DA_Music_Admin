using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CustomControls.Controls
{
    public class RichTextBoxHelper : DependencyObject
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
                        
                        // Parse the XAML to a document (or use XamlReader.Parse())
                        var xaml = GetDocumentXaml(richTextBox);
                        var doc = new FlowDocument();
                        var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        if (xaml is string)
                        {
                            range.Text = xaml;
                            doc = RichTextBoxHelper.StringToDocument(xaml);
                        }
                        else
                        {
                            range.Load(new MemoryStream(Encoding.UTF8.GetBytes(xaml)),
                                  DataFormats.Xaml);
                        }
                        // Set the document
                        richTextBox.Document = doc;
                        richTextBox.TextChanged += RichTextBox_TextChanged;
                        // When the document changes update the source
                        range.Changed += (obj2, e2) =>
                        {
                            if (richTextBox.Document == doc)
                            {
                                MemoryStream buffer = new MemoryStream();
                                range.Save(buffer, DataFormats.Xaml);
                                SetDocumentXaml(richTextBox,
                                    Encoding.UTF8.GetString(buffer.ToArray()));
                            }
                        };
                    }
                });

        private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var target = sender as RichTextBox;
            if(target != null)
            {
                SetDocumentXaml(target, DocumentToString(target.Document));
            }
        }

        public static FlowDocument StringToDocument(string content)
        {
            FlowDocument document = new FlowDocument();
            System.Windows.Documents.Paragraph paragraph = new System.Windows.Documents.Paragraph(new Run(content));
            document.Blocks.Add(paragraph);
            return document;
        }

        public static string DocumentToString(FlowDocument document)
        {
            var text = new TextRange(document.ContentStart, document.ContentEnd).Text;
            text = text.Replace("\r", "");
            return text;
        }
    }

  
}

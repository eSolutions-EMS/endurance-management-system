using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace EnduranceJudge.Gateways.Desktop.Services;

public class Printer : IPrinter
{
    private readonly IFileService fileService;
    private readonly string previewWindowXaml =
        @"<Window xmlns ='http://schemas.microsoft.com/netfx/2007/xaml/presentation'
                  xmlns:x ='http://schemas.microsoft.com/winfx/2006/xaml'
                  Title ='Print Preview - @@TITLE'
                  WindowState='Maximized'>
            <DocumentViewer Name='dv1'/>
        </Window>";

    public Printer(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public void Print(Visual visual)
    {
        var filePath = Path.GetRandomFileName();

        try
        {
            // write the XPS document
            using var writeDoc = new XpsDocument(filePath, FileAccess.ReadWrite);
            var writer = XpsDocument.CreateXpsDocumentWriter(writeDoc);
            writer.Write(visual);

            // Read the XPS document into a dynamically generated
            // preview Window
            // using var readDoc = new XpsDocument(filePath, FileAccess.Read);
            var fds = writeDoc.GetFixedDocumentSequence();

            var previewXaml = this.previewWindowXaml;
            previewXaml = previewXaml.Replace("@@TITLE", "Endurance Judge");

            using var stringReader = new StringReader(previewXaml);
            using var xmlReader = new XmlTextReader(stringReader);

            var previewWindow = (Window)XamlReader.Load(xmlReader);
            var dv1 = (DocumentViewer) LogicalTreeHelper.FindLogicalNode(previewWindow, "dv1");
            dv1!.Document = fds;

            previewWindow.ShowDialog();
        }
        finally
        {
            this.fileService.Delete(filePath);
        }
    }
}

public interface IPrinter : IService
{
    void Print(Visual visual);
}

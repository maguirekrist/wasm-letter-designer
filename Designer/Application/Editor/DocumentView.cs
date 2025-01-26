using Topten.RichTextKit.Editor;

namespace WASMApp.Application.Editor;

public class DocumentView : ITextDocumentView
{
    public void OnReset()
    {
        Console.WriteLine("OnReset");
        //throw new NotImplementedException();
    }

    public void OnRedraw()
    {
        Console.WriteLine("OnRedraw");
        //throw new NotImplementedException();
    }

    public void OnDocumentWillChange(ITextDocumentView view)
    {
        Console.WriteLine("OnDocumentWillChange");
        //throw new NotImplementedException();
    }

    public void OnDocumentChange(ITextDocumentView view, DocumentChangeInfo info)
    {
        Console.WriteLine("OnDocumentChange");
        //throw new NotImplementedException();
    }

    public void OnDocumentDidChange(ITextDocumentView view)
    {
        Console.WriteLine("OnDocumentDidChange");
        //throw new NotImplementedException();
    }
}
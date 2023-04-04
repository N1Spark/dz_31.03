using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dz_31._03
{
    public class TextEditor
    {
        private string text;
        private Caretaker history;
        public TextEditor()
        {
            text = "";
            history = new Caretaker();
        }
        public void Write(string text)
        {
            history.Push(new TextEditorMemento(this.text));
            this.text += text;
        }
        public void Undo()
        {
            TextEditorMemento memento = history.Undo();
            if (memento != null)
                text = memento.GetText();
        }
        public void Redo()
        {
            TextEditorMemento memento = history.Redo();
            if (memento != null)
            {
                text = memento.GetText();
                history.Push(new TextEditorMemento(text));
            }
        }
        public string GetText()
        {
            return text;
        }
    }
    public class TextEditorMemento
    {
        private string text;
        public TextEditorMemento(string text) => this.text = text;
        public string GetText()
        {
            return text;
        }
    }
    public class Caretaker
    {
        private Stack<TextEditorMemento> mementos;
        private int limit;
        private Stack<TextEditorMemento> redoMementos;
        public Caretaker(int limit = 256)
        {
            mementos = new Stack<TextEditorMemento>();
            redoMementos = new Stack<TextEditorMemento>();
            this.limit = limit;
        }
        public void Push(TextEditorMemento memento)
        {
            mementos.Push(memento);
            if (mementos.Count > limit)
                mementos = new Stack<TextEditorMemento>(mementos.Take(limit));
            redoMementos.Clear();
        }
        public TextEditorMemento Undo()
        {
            if (mementos.Count > 1)
            {
                TextEditorMemento memento = mementos.Pop();
                redoMementos.Push(memento);
                return mementos.Peek();
            }
            return null;
        }
        public TextEditorMemento Redo()
        {
            if (redoMementos.Count > 0)
            {
                TextEditorMemento memento = redoMementos.Pop();
                mementos.Push(memento);
                return memento;
            }
            return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.Undoing\n2.Redoing");
            TextEditor texteditor = new TextEditor();
            Console.WriteLine("Enter the text:");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "1")
                    texteditor.Undo();
                else if (input == "2")
                    texteditor.Redo();
                else
                    texteditor.Write(input);
                Console.WriteLine("Text: " + texteditor.GetText());
            }
        }
    }
}

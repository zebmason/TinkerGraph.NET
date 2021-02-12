namespace Translator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var traverser = new Traverser(args[0], args[1], new Stub());
            traverser.Traverse();
        }
    }
}

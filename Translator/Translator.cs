namespace Translator
{
    using System.IO;

    internal static class Translator
    {
        public static void Translate(string java, string csharp)
        {
            var direc = Path.GetDirectoryName(csharp);
            Directory.CreateDirectory(direc);

            File.WriteAllText(csharp, string.Empty);
        }
    }
}

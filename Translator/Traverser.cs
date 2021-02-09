namespace Translator
{
    using System.IO;

    internal class Traverser
    {
        private readonly string _java;

        private readonly string _csharp;

        public Traverser(string java, string csharp)
        {
            _java = java;
            _csharp = csharp;
        }

        private string CSharp(string java)
        {
            var csharp = _csharp + java.Substring(_java.Length);
            for (int i = _csharp.Length; i < csharp.Length; ++i)
            {
                if (csharp[i] != '\\')
                    continue;

                csharp = csharp.Substring(0, i + 1) + char.ToUpper(csharp[i + 1]) + csharp.Substring(i + 2);
            }

            return csharp.Substring(0, csharp.Length - 4) + "cs";
        }

        private void Traverse(string local)
        {
            var csharp = _csharp + local.Substring(_java.Length);

            foreach (var dir in Directory.GetDirectories(local))
            {
                Traverse(dir);
            }

            foreach (var filePath in Directory.GetFiles(local))
            {
                if (Path.GetExtension(filePath) != ".java")
                    continue;

                Translator.Translate(filePath, CSharp(filePath));
            }
        }

        public void Traverse()
        {
            Traverse(_java);
        }
    }
}

namespace Translator
{
    using System.IO;

    internal class Traverser
    {
        private readonly string _java;

        private readonly string _csharp;

        private readonly ITranslator _translator;

        public Traverser(string java, string csharp, ITranslator translator)
        {
            _java = java;
            _csharp = csharp;
            _translator= translator;
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

        private string NameSpace(string direc)
        {
            var name = direc.Substring(_csharp.Length + 1);
            return  $"TinkerGraph.NET.{name.Replace("\\", ".")}";
        }

        private void Traverse(string local)
        {
            foreach (var dir in Directory.GetDirectories(local))
            {
                Traverse(dir);
            }

            foreach (var filePath in Directory.GetFiles(local))
            {
                if (Path.GetExtension(filePath) != ".java")
                    continue;

                var csharp = CSharp(filePath);
                var direc = Path.GetDirectoryName(csharp);
                Directory.CreateDirectory(direc);

                var nameSpace = NameSpace(direc);
                var lines = File.ReadAllText(filePath);
                lines = _translator.Translate(lines, nameSpace);
                File.WriteAllText(csharp, lines);

            }
        }

        public void Traverse()
        {
            Traverse(_java);
        }
    }
}

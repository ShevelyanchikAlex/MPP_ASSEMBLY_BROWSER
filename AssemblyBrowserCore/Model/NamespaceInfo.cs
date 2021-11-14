using System.Collections.Generic;

namespace AssemblyBrowserCore.Model
{
    public class NamespaceInfo
    {
        private List<ClassInfo> _classesList;
        public string Name { get; set; }
        public List<ClassInfo> ClassesList
        {
            get => _classesList;
            set => _classesList = value;
        }

        public NamespaceInfo(string name)
        {
            Name = name;
            ClassesList = new List<ClassInfo>();
        }
    }
}
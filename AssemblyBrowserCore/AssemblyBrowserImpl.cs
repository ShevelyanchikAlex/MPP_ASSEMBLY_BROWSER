using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyBrowserCore.Model;

namespace AssemblyBrowserCore
{
    public static class AssemblyBrowserCoreImpl
    {
        private const string EmptyNamespace = "Namespace is absent";

        public static List<NamespaceInfo> GetAssemblyData(string assemblyPath)
        {
            var namespaces = new Dictionary<string, NamespaceInfo>();
            var assembly = Assembly.LoadFrom(assemblyPath);
            var assemblyTypes = assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                if (namespaces.TryGetValue(type.Namespace ?? EmptyNamespace, out var namespaceInfo))
                {
                    namespaceInfo.ClassesList.Add(ClassInfo.AsClassInfo(type));
                }
                else
                {
                    var currentNamespace = type.Namespace ?? EmptyNamespace;
                    var newNamespace = new NamespaceInfo(currentNamespace);
                    newNamespace.ClassesList.Add(ClassInfo.AsClassInfo(type));
                    namespaces.Add(currentNamespace, newNamespace);
                }
            }

            return namespaces.Values.ToList();
        }
    }
}
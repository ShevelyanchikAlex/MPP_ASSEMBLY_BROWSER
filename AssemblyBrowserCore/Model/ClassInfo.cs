using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserCore.Model
{
    public class ClassInfo
    {
        private List<ClassMemberInfo> _members;

        private const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Static | BindingFlags.Public |
                                                     BindingFlags.FlattenHierarchy;

        public string Name { get; set; }

        public List<ClassMemberInfo> Members
        {
            get => _members;
            set => _members = value;
        }

        public ClassInfo(string name)
        {
            Name = name;
            Members = new List<ClassMemberInfo>();
        }

        public static ClassInfo AsClassInfo(Type type)
        {
            var classInfo = new ClassInfo(GetClassSignature(type));

            var fields = type.GetFields(BindingFlagsAll);
            foreach (var fieldInfo in fields)
            {
                classInfo.Members.Add(ClassMemberInfo.FieldAsClassMemberInfo(fieldInfo));
            }
            
            var constructors = type.GetConstructors(BindingFlagsAll);
            foreach (var constructorInfo in constructors)
            {
                classInfo.Members.Add(ClassMemberInfo.ConstructorAsClassMemberInfo(constructorInfo));
            }
            
            var methods = type.GetMethods(BindingFlagsAll);
            foreach (var methodInfo in methods)
            {
                classInfo.Members.Add(ClassMemberInfo.MethodAsClassMemberInfo(methodInfo));
            }
            
            var properties = type.GetProperties(BindingFlagsAll);
            foreach (var propertyInfo in properties)
            {
                classInfo.Members.Add(ClassMemberInfo.PropertyAsClassMemberInfo(propertyInfo));
            }

            return classInfo;
        }

        private static string GetClassSignature(Type type)
        {
            var classSignatureBuilder = new StringBuilder("[CLASS] ");
            classSignatureBuilder.Append(type.IsNotPublic ? "internal " : "public ");
            if (type.IsAbstract) classSignatureBuilder.Append("abstract ");
            if (type.IsSealed) classSignatureBuilder.Append("sealed ");
            if (type.BaseType != null && type.IsClass && type.BaseType.Name == "MulticastDelegate")
                classSignatureBuilder.Append("delegate");
            if (type.IsClass) classSignatureBuilder.Append("class");
            if (type.IsInterface) classSignatureBuilder.Append("interface");
            if (type.IsEnum) classSignatureBuilder.Append("enum");
            if (type.IsValueType && !type.IsPrimitive) classSignatureBuilder.Append("struct");
            classSignatureBuilder.Append(' ');
            classSignatureBuilder.Append(type.Name);
            return classSignatureBuilder.ToString();
        }
    }
}
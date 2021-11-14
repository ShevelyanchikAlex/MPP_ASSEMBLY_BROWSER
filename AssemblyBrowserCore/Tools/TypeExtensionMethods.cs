using System;

namespace AssemblyBrowserCore.Tools
{
    public static class TypeExtensionMethods
    {
        public static bool IsNullable(this Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null;
        }

        public static bool IsGeneric(this Type type)
        {
            return type.IsGenericType
                   && type.Name.Contains("`");
        }

        public static string GetQualifiedTypeName(Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return "string";
                case "Int32":
                    return "int";
                case "Decimal":
                    return "decimal";
                case "Object":
                    return "object";
                case "Void":
                    return "void";
                case "Boolean":
                    return "bool";
            }

            var signature = string.IsNullOrWhiteSpace(type.FullName)
                ? type.Name
                : type.FullName;

            if (IsGeneric(type))
                signature = RemoveGenericTypeNameArgumentCount(signature);

            return signature;
        }


        private static string RemoveGenericTypeNameArgumentCount(string genericTypeSignature)
        {
            return genericTypeSignature[..genericTypeSignature.IndexOf('`')];
        }
    }
}
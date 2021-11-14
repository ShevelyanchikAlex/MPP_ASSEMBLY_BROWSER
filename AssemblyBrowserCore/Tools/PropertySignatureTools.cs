using System.Reflection;
using System.Text;

namespace AssemblyBrowserCore.Tools
{
    public static class PropertySignatureTools
    {
        public static string GetSignature(this PropertyInfo propertyInfo)
        {
            var signatureBuilder = new StringBuilder(propertyInfo.GetPropertyAccessModifierSignature());
            signatureBuilder.Append(' ');

            signatureBuilder.Append(propertyInfo.GetPropertyModifierSignature());
            signatureBuilder.Append(' ');

            signatureBuilder.Append(propertyInfo.PropertyType.GetSignature());
            signatureBuilder.Append(' ');

            signatureBuilder.Append(propertyInfo.Name);
            signatureBuilder.Append(' ');

            signatureBuilder.Append(propertyInfo.GetPropertyAccessorsSignature());
            return signatureBuilder.ToString();
        }

        private static string GetPropertyAccessModifierSignature(this PropertyInfo propertyInfo)
        {
            var accessor = propertyInfo.GetAccessors(true)[0];
            if (accessor.IsPrivate) return "private";
            if (accessor.IsPublic) return "public";
            if (accessor.IsAssembly) return "internal";
            if (accessor.IsFamilyAndAssembly) return "private protected";
            return "protected internal";
        }
        
        private static string GetPropertyModifierSignature(this PropertyInfo propertyInfo)
        {
            var accessor = propertyInfo.GetAccessors(true)[0];
            var sb = new StringBuilder();
            if (accessor.IsAbstract) sb.Append("abstract ");
            else if (accessor.IsVirtual) sb.Append("virtual ");
            if (accessor.IsStatic) sb.Append("static ");
            return sb.ToString();
        }

        private static string GetPropertyAccessorsSignature(this PropertyInfo propertyInfo)
        {
            var sb = new StringBuilder();
            sb.Append(" { ");
            string accessors = string.Empty;
            foreach (var accessor in propertyInfo.GetAccessors())
            {
                if (accessor.IsSpecialName)
                {
                    if (accessor.IsPrivate) accessors += "private ";
                    accessors += accessor.Name;
                    accessors += ", ";
                }
            }

            if (accessors.Length > 0) accessors = accessors.Remove(accessors.Length - 2, 2);
            sb.Append(accessors);
            sb.Append(" } ");
            return sb.ToString();
        }
    }
}
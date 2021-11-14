using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserCore.Tools
{
    public static class ConstructorSignatureTools
    {
        public static string GetSignature(this ConstructorInfo constructorInfo)
        {
            var signatureBuilder = new StringBuilder(constructorInfo.GetConstructorAccessorSignature());
            signatureBuilder.Append(' ');

            signatureBuilder.Append(constructorInfo.Name);
            signatureBuilder.Append(constructorInfo.GetConstructorArgumentsSignature());

            return signatureBuilder.ToString();
        }

        private static string GetConstructorAccessorSignature(this ConstructorInfo constructor)
        {
            string signature = null;

            if (constructor.IsAssembly)
            {
                signature = "internal ";

                if (constructor.IsFamily)
                    signature += "protected ";
            }
            else if (constructor.IsPublic)
            {
                signature = "public ";
            }
            else if (constructor.IsPrivate)
            {
                signature = "private ";
            }
            else if (constructor.IsFamily)
            {
                signature = "protected ";
            }

            if (constructor.IsStatic)
                signature += "static ";

            return signature;
        }

        private static string GetConstructorArgumentsSignature(this ConstructorInfo constructor)
        {
            var methodParameters = constructor.GetParameters().AsEnumerable();

            var methodParameterSignatures = methodParameters.Select(param =>
            {
                var signature = string.Empty;

                signature += TypeSignatureTools.GetSignature(param.ParameterType) + " ";

                signature += param.Name;

                return signature;
            });

            var methodParameterString = "(" + string.Join(", ", methodParameterSignatures) + ")";

            return methodParameterString;
        }
    }
}
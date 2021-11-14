using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserCore.Tools
{
    public static class MethodSignatureTools
    {
        public static string GetSignature(this MethodInfo method, bool invokable)
        {
            var signatureBuilder = new StringBuilder();

            if (!invokable)
            {
                signatureBuilder.Append(GetMethodAccessorSignature(method));
                signatureBuilder.Append(' ');
            }

            signatureBuilder.Append(method.Name);

            if (method.IsGenericMethod)
            {
                signatureBuilder.Append(GetGenericSignature(method));
            }

            signatureBuilder.Append(GetMethodArgumentsSignature(method, invokable));

            return signatureBuilder.ToString();
        }

        private static string GetMethodAccessorSignature(this MethodInfo method)
        {
            string signature = null;

            if (method.IsAssembly)
            {
                signature = "internal ";

                if (method.IsFamily)
                    signature += "protected ";
            }
            else if (method.IsPublic)
            {
                signature = "public ";
            }
            else if (method.IsPrivate)
            {
                signature = "private ";
            }
            else if (method.IsFamily)
            {
                signature = "protected ";
            }

            if (method.IsStatic)
                signature += "static ";

            signature += method.ReturnType.GetSignature();

            return signature;
        }

        private static string GetGenericSignature(this MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsGenericMethod) throw new ArgumentException($"{method.Name} is not generic.");

            return TypeSignatureTools.BuildGenericSignature(method.GetGenericArguments());
        }

        private static string GetMethodArgumentsSignature(this MethodInfo method, bool invokable)
        {
            var isExtensionMethod = method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false);
            var methodParameters = method.GetParameters().AsEnumerable();

            if (isExtensionMethod && invokable)
            {
                //If this signature is designed to be invoked and it's
                //an extension method skip the first argument
                methodParameters = methodParameters.Skip(1);
            }

            var methodParameterSignatures = methodParameters.Select(param =>
            {
                var signature = string.Empty;

                if (param.ParameterType.IsByRef)
                    signature = "ref ";
                else if (param.IsOut)
                    signature = "out ";
                else if (isExtensionMethod && param.Position == 0)
                    signature = "this ";

                if (!invokable)
                {
                    signature += param.ParameterType.GetSignature() + " ";
                }

                signature += param.Name;

                return signature;
            });

            var methodParameterString = "(" + string.Join(", ", methodParameterSignatures) + ")";

            return methodParameterString;
        }
    }
}
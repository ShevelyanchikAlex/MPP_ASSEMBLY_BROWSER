using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyBrowserCore.Tools
{
    public static class TypeSignatureTools
    {
        public static string GetSignature(this Type type)
        {
            var isNullableType = type.IsNullable(out var underlyingNullableType);

            var signatureType = isNullableType
                ? underlyingNullableType
                : type;

            var isGenericType = signatureType.IsGeneric();

            var signature = TypeExtensionMethods.GetQualifiedTypeName(signatureType);

            if (isGenericType)
            {
                signature += BuildGenericSignature(signatureType.GetGenericArguments());
            }

            if (isNullableType)
            {
                signature += "?";
            }

            return signature;
        }

        public static string BuildGenericSignature(IEnumerable<Type> genericArgumentTypes)
        {
            var argumentSignatures = genericArgumentTypes.Select(GetSignature);

            return "<" + string.Join(", ", argumentSignatures) + ">";
        }
    }
}
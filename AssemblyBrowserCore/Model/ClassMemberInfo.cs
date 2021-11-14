using System.Reflection;
using AssemblyBrowserCore.Tools;

namespace AssemblyBrowserCore.Model
{
    public class ClassMemberInfo
    {
        public string StringRepresentation { get; set; }

        public ClassMemberInfo(string stringRepresentation)
        {
            StringRepresentation = stringRepresentation;
        }

        public static ClassMemberInfo FieldAsClassMemberInfo(FieldInfo fieldInfo)
        {
            return new ClassMemberInfo("[FIELD] " + fieldInfo.GetSignature());
        }

        public static ClassMemberInfo ConstructorAsClassMemberInfo(ConstructorInfo constructorInfo)
        {
            return new ClassMemberInfo("[CONSTRUCTOR] " + constructorInfo.GetSignature());
        }
        
        public static ClassMemberInfo MethodAsClassMemberInfo(MethodInfo methodInfo)
        {
            return new ClassMemberInfo("[METHOD] " + methodInfo.GetSignature(false));
        }
        
        public static ClassMemberInfo PropertyAsClassMemberInfo(PropertyInfo propertyInfo)
        {
            return new ClassMemberInfo("[PROPERTY] " + propertyInfo.GetSignature());
        }
    }
}
using System;
using System.Reflection;
using System.Text;

namespace DocumentationProject
{
    public static class clsDocumentation
    {
        private static Type _type { get; set; }
        private static StringBuilder _document { get; set; }
        private static void _GetMethodsInfo()
        {
            MemberInfo[] methods = _type.GetMethods();
            _document.AppendLine($"the class contain {methods.Length} {_HandlePluralString("Method", methods.Length, "Methods")}");
            foreach (MethodInfo method in methods)
            {
                _document.AppendLine($"\nMethod name: {method.Name}");
                _document.Append(_GetDocumentationAttribute(method));
                ParameterInfo[] properties = method.GetParameters();
                _document.AppendLine($"this method has {properties.Length} {_HandlePluralString("parameter", properties.Length, "parameters")}");
                foreach (ParameterInfo parameter in properties)
                {
                    _document.AppendLine($"Parameter name: {parameter.Name}, parameter type: {parameter.ParameterType}");
                }
            }
        }
        private static string _HandlePluralString(string Name, int Length, string PluralName)
        {
            if (Length > 1) return PluralName;
            else return Name;
        }
        private static void _GetClassPropertiesInfo()
        {
            PropertyInfo[] properties = _type.GetProperties();
            _document.AppendLine($"\nThe class takes {properties.Length} {_HandlePluralString("property", properties.Length, "properties")}");
            foreach (PropertyInfo property in properties)
            {
                _document.Append(_GetDocumentationAttribute(property));
                _document.AppendLine($"Property name: {property.Name}, property type: {property.PropertyType}");
            }
        }
        private static void _GetConstructorsInfo()
        {
            ConstructorInfo[] constructors = _type.GetConstructors();
            _document.AppendLine($"\nThis class takes {constructors.Length} {_HandlePluralString("Constructor", constructors.Length, "Constructors")}.");
            int Counter = 0;
            foreach (ConstructorInfo constructor in constructors)
            {
                Counter++;
                _document.Append($"Constrocture number {Counter}. ");
                _document.Append(_GetDocumentationAttribute(constructor));
                ParameterInfo[] parameters = constructor.GetParameters();
                _document.Append($"\ntakes {parameters.Length} parameter{(parameters.Length > 1 ? "s" : null)}");
                foreach (ParameterInfo parameter in parameters)
                {

                    _document.Append($"\nParameter name: {parameter.Name}, Parameter Type {parameter.ParameterType}");

                }
                _document.AppendLine("\n");
            }

        }
        private static string _GetDocumentationAttribute(MemberInfo type)
        {
            var documentationAttribute = (DocumentationAttribute)Attribute.GetCustomAttribute(type, typeof(DocumentationAttribute));
            if (documentationAttribute == null)
                return null;
            return $"[ {documentationAttribute.Documentation.Trim()} ] ";
        }
        public static StringBuilder Create(Assembly assembly)
        {
            _document = new StringBuilder();
            int Counter = 0;
            foreach (Type CurrentType in assembly.GetTypes())
            {
                _type = CurrentType;
                if (!_type.IsClass)
                    continue;
                Counter++;
                _document.AppendLine($"[{Counter}] {_type.Name}");

                _document.AppendLine(_GetDocumentationAttribute(_type));



                _GetClassPropertiesInfo();
                _GetConstructorsInfo();
                _GetMethodsInfo();
            }
            return _document;
        }

    }
}

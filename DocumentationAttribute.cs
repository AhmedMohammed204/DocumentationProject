using System;

namespace DocumentationProject
{
    public class DocumentationAttribute : Attribute
    {
        public string Documentation { get; set; }
        public DocumentationAttribute(string documentation)
        {
            Documentation = documentation;
        }
    }
}

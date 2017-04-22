using System;

namespace MES.Common.Helper
{
    public class ReferenceDetails
    {
        public string ArticlePart { get; set; }
        public string ReferencePart { get; set; }
        public string UniqueIdPart { get; set; }
    }
    public class ProductReference
    {
        private const int UniqueLength = 5;
        public ProductReference(string fullName)
        {
            fullName =fullName.ToUpper();
            Parsed = ParseReferenceDetails(fullName);
            FullName = fullName;
        }

        public ReferenceDetails Parsed { get; }
        public string FullName { get; }

        public static ReferenceDetails ParseReferenceDetails(string fullnameReference)
        {
            
            var len = fullnameReference.Length;
            var s = fullnameReference.Substring(0, 12);
            var t = fullnameReference.Substring(12, len - 12  - UniqueLength);
            var u = fullnameReference.Substring(len- UniqueLength, UniqueLength);
            return new ReferenceDetails
            {
                ArticlePart = s,
                ReferencePart = t,
                UniqueIdPart = u
            };
        }
    }
}

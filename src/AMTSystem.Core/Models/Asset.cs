using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public int Category_Id { get; set; }
        public string Category_String { get; set; }
        public int Condition_Id { get; set; }
        public string Condition_String { get; set; }
        public int Status_Id { get; set; }
        public string Status_String { get; set; }
        public string? Components { get; set; } = string.Empty;
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBaseLib
{
    using System;
    using System.Collections.Generic;
    
    public partial class logs
    {
        public long id { get; set; }
        public string code { get; set; }
        public string enlicense { get; set; }
        public string exlicense { get; set; }
        public System.DateTime enter { get; set; }
        public Nullable<System.DateTime> exit { get; set; }
        public Nullable<int> cost { get; set; }
        public byte[] enpicture { get; set; }
        public byte[] expicture { get; set; }
        public string type { get; set; }
        public Nullable<long> enuser { get; set; }
        public Nullable<long> exuser { get; set; }
    }
}

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
    
    public partial class driver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public driver()
        {
            this.car = new HashSet<car>();
        }
    
        public long id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string orgname { get; set; }
        public string orgval { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public byte[] picture { get; set; }
        public System.DateTime register { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<car> car { get; set; }
    }
}

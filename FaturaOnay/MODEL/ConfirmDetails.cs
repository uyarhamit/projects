//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MODEL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConfirmDetails
    {
        public int Id { get; set; }
        public Nullable<int> ConfirmingUserId { get; set; }
        public string ConfirmingUserName { get; set; }
        public Nullable<System.DateTime> ConfirmingDate { get; set; }
        public Nullable<System.TimeSpan> ConfirmingTime { get; set; }
        public string ConfirmingDetail { get; set; }
        public Nullable<int> InvoiceId { get; set; }
    }
}

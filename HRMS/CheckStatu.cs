//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class CheckStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckStatu()
        {
            this.LeaveApplications = new HashSet<LeaveApplication>();
            this.Travel_Expense_Application = new HashSet<Travel_Expense_Application>();
        }
    
        public int CheckStatusID { get; set; }
        public string CheckStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveApplication> LeaveApplications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Travel_Expense_Application> Travel_Expense_Application { get; set; }
    }
}
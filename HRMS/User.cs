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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Absences = new HashSet<Absence>();
            this.LeaveApplications = new HashSet<LeaveApplication>();
            this.Travel_Expense_Application = new HashSet<Travel_Expense_Application>();
            this.User1 = new HashSet<User>();
        }
    
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEnglishName { get; set; }
        public string PassWord { get; set; }
        public Nullable<System.DateTime> OnBoardDay { get; set; }
        public Nullable<System.DateTime> ByeByeDay { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<int> Department { get; set; }
        public Nullable<int> JobTitle { get; set; }
        public Nullable<int> Supervisor { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Phone { get; set; }
        public byte[] Photo { get; set; }
        public string EmergencyPerson { get; set; }
        public string EmergencyContact { get; set; }
        public int OnBoardState { get; set; }
        public byte AccountEnable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Absence> Absences { get; set; }
        public virtual Department Department1 { get; set; }
        public virtual JobTitle JobTitle1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveApplication> LeaveApplications { get; set; }
        public virtual OnBoardStatu OnBoardStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Travel_Expense_Application> Travel_Expense_Application { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}

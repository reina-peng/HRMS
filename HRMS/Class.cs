using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS
{
    public class UserInfo
    {
        public int ID;//員工ID
        public string Name;//員工姓名
        public string EnglishName;//員工英文名
        public int Dept;//員工部門
        public int JobTitle;//員工職稱

        public UserInfo(int userID)
        {
            MyHREntities hrEntities = new MyHREntities();
            var q = (hrEntities.Users.Where(o => o.EmployeeID == userID).Select(o => new { o.EmployeeName, o.EmployeeEnglishName, o.Department, o.JobTitle })).ToList();//抓員工資料         
            ID = userID;
            Name = q[0].EmployeeName;
            EnglishName = q[0].EmployeeEnglishName;
            Dept = (int)q[0].Department;
            JobTitle = (int)q[0].JobTitle;
        }
    }
}

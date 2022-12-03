using System;
using System.ComponentModel.DataAnnotations;
namespace TWP_API_Payroll.ViewModels
{
    public class DepartmentsBaseModel
    {

        [Required]
        public string DepartmentName { get; set; }
        
    }
    public class DepartmentsViewModel : DepartmentsBaseModel
    {
        [Required]
        public Guid Id { get; set; }

    }

}
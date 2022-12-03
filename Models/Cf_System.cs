using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWP_API_Payroll.Models
{
    [Table("Cf_System")]
    public partial class CfSystem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        public Guid PayrollAdminAdvanceAccount { get; set; }
        public Guid PayrollManufacturingAdvanceAccount { get; set; }

        public Guid PayrollAdminLoanAccount { get; set; }
        public Guid PayrollManufacturingLoanAccount { get; set; }


        public Guid PayrollAdminInsuranceAccount { get; set; }
        public Guid PayrollManufacturingInsuranceAccount { get; set; }



        public Guid PayrollAdminIncomeTaxAccount { get; set; }
        public Guid PayrollManufacturingIncomeTaxAccount { get; set; }


        public Guid PayrollAdminSalaryExpenseAccount { get; set; }

        public Guid PayrollAdminSalaryPayableAccount { get; set; }


        public Guid PayrollManufacturingExpenseAccount { get; set; }

        public Guid PayrollManufacturingPayableAccount { get; set; }

        public Guid PayrollManufacturingOverTimeExpenseAccount { get; set; }

        public Guid PayrollManufacturingOverTimePayableAccount { get; set; }

        public Guid PayrollAdminOverTimeExpenseAccount { get; set; }

        public Guid PayrollAdminOverTimePayableAccount { get; set; }



        [Required]
        public Guid CompanyId { get; set; }

        public string UserNameInsert { get; set; }

        public DateTime? InsertDate { get; set; }



    }


}

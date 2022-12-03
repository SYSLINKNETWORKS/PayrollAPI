using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class EmployeeProfileDocumentBaseModel {

    }
    public class EmployeeProfileDocumentFoundationModel : EmployeeProfileDocumentBaseModel {

        // [Required]
        // public Guid EmployeeId { get; set; }

        // [Required]
        // public Boolean ImageProfileCheck { get; set; }

        // public IFormFile ProfileImage { get; set; }
    }
    //    public class EmployeeProfileDocumentAddModel : EmployeeProfileDocumentFoundationModel { }

    public class EmployeeProfileDocumentViewByIdModel : EmployeeProfileDocumentFoundationModel {
      
        public List<EmployeeProfileDocumentList> EmployeeProfileDocumentLists { get; set; }
    }

    public class EmployeeProfileDocumentAddModel : EmployeeProfileDocumentFoundationModel {

        public List<EmployeeProfileDocumentList> EmployeeProfileDocumentLists { get; set; }

    }

    public class EmployeeProfileDocumentList {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public string ImageProfileCheck { get; set; }
        public string ImageName { get; set; }
        public string ImageBytes { get; set; }
        public string ImageExtension { get; set; }
        public Guid Menu_Id { get; set; }

    }

}
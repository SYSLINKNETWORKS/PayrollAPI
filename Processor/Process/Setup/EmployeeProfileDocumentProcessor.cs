using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Processor.Process.Payroll {
    public class EmployeeProfileDocumentProcessor : IProcessor<EmployeeProfileDocumentBaseModel> {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper ();
        private CompressImage _CompressImage = new CompressImage ();

        public EmployeeProfileDocumentProcessor (App_Data.DataContext context) {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass (Enums.ClassName.EmployeeProfileDocument, _context);
        }
        public Task<ApiResponse> ProcessGet (Guid _MenuId, ClaimsPrincipal _User) {
            return null;
        }
        public Task<ApiResponse> ProcessGetById (Guid _Id, Guid _MenuId, ClaimsPrincipal _User) {
            // ApiResponse apiResponse = new ApiResponse ();

            // apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            // apiResponse.message = "Invalid Class";
            // return apiResponse;
            return null;

        }
        public async Task<ApiResponse> ProcessPost (object request, ClaimsPrincipal _User) {
            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                var _request = (EmployeeProfileDocumentAddModel) request;
                List<EmployeeImage> _EmployeeImage = new List<EmployeeImage> ();

                foreach (var _requestRecord in _request.EmployeeProfileDocumentLists) {

                    byte[] newBytes = Convert.FromBase64String (_requestRecord.ImageBytes);
                    byte[] imageBytes = _CompressImage.GetCompressImage (newBytes);
                    string base64String = "";
                    base64String = Convert.ToBase64String (imageBytes);

                    Boolean _ImageProfileCheck = false;
                    if (_requestRecord.ImageProfileCheck == "1") { _ImageProfileCheck = true; }
                    _EmployeeImage.Add (new EmployeeImage {
                        EmployeeId = _requestRecord.EmployeeId,
                            ImageProfileCheck = _ImageProfileCheck,
                            ImageName = _requestRecord.ImageName,
                            ImageExtension = _requestRecord.ImageExtension,
                            ImageBytes = base64String,
                    });

                }
                return await _AbsBusiness.AddAsync (_EmployeeImage, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public Task<ApiResponse> ProcessPut (object request, ClaimsPrincipal _User) {

            return null;

        }
        public Task<ApiResponse> ProcessDelete (object request, ClaimsPrincipal _User) {
            return null;
        }

    }
}
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TWP_API_Payroll.Helpers;

namespace TWP_API_Payroll.Generic {
    public class AbsBusiness : IFacade<object> {
        public virtual Task<ApiResponse> GetDataAsync (ClaimsPrincipal _User) {
            throw new NotImplementedException ();
        }
        public virtual Task<ApiResponse> GetDataByIdAsync (Guid _Id, ClaimsPrincipal _User) {
            throw new NotImplementedException ();
        }

        public virtual Task<ApiResponse> AddAsync (object model, ClaimsPrincipal _User) {
            throw new NotImplementedException ();
        }
        public virtual Task<ApiResponse> UpdateAsync (object model, ClaimsPrincipal _User) {
            throw new NotImplementedException ();
        }
        public virtual Task<ApiResponse> DeleteAsync (Guid _Id, ClaimsPrincipal _User) {
            throw new NotImplementedException ();
        }

    }
}
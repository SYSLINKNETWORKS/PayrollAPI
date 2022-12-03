namespace TWP_API_Payroll.Generic
{
    public class Enums
    {
        public enum ClassName
        {

            UserGroup,
            UserMenu,
            UserMenuModule,
            UserMenuCategory,
            UserMenuSubCategory,
            GetMenu,
            RolePermission,
            AuthClaim,
            Role,
            UserClaim,
            User,
            UserRole,
            Company,
            Branch,
            FinancialYear,
            ListOfValue,

            //Payroll Start
            //Setup
            Designation,
            Department,
            Roster,
            RosterGroup,
            IncomeTaxSlabEmployee,
            EmployeeCategory,
            MachineCompany,
            AttendanceMachineGroup,
            AttendanceMachine,
            AnnualLeaves,
            InOutCategory,
            EmployeeProfile,
            EmployeeProfileDocument,
            InOutEditor,
            LoanCategory,
            Allowance,
            //Transaction
            Holiday,
            //Dashboard 
            PayrollDashboard,
            Advance,
            LoanIssue,
            LoanReceive,
            SalaryAdditionDeduction,
            Salary,

            //Payroll End

            //Production Start 
            //Configuration
            DieCutting,
            Eyelet,
            FoilingHotStamping,
            FoilingHotStampingArea,
            LaminationCoating,
            LaminationCoatingArea,
            PackingNature,
            Pasting,
            Sublet,
            PrintingColor,
            PrintingOpv,
            PrintingWaterbase,
            UvCoating,
            UvCoatingarea,
            UvHoloGraphicCoating,
            UvHoloGraphicCoatingArea,
            Machine,
            ProductionDepartment,
            //Transaction
            JobOrder,
            JobOrderSequence,
            ProductionRequisition,
            DieCuttingMaster,
            EyeletMaster,
            FoilingMaster,
            LaminationMaster,
            PackingMaster,
            PastingMaster,
            PrintingMaster,
            SubletMaster,
            UVMaster,

            //Item Start 
            //Setup
            Scale,
            ItemCategory,
            ItemSubCategoryMaster,
            ItemSubCategory,
            Brand,
            Item,

            //Sales
            //Setup
            Country,
            City,
            Zone,
            CustomerCategory,
            CustomerSubCategory,
            CustomerBank,
            Customer,

            //Transaction
            Quotation,
            QuotationRate,
            SalesOrder,

        }
        public enum Operations
        {
            A,
            E,
            D,
            S,
            U,
            I,
            O,
            M,
            N

        }
        public enum Policies
        {
            View,
            ViewById,
            Insert,
            Update,
            Delete,
            Checked,
            Approved

        }
        public enum Roles
        {
            Role,
            SuperAdmin

        }

        public enum Misc
        {
            UserId,
            UserName,

            CompanyId,
            CompanyName,
            BranchId,
            BranchName,

            YearId,
            YearName,
            Email,
            Key

        }
        public enum Payroll
        {
            Present,
            Absent,
            A,
            AnnualLeave,
            C,
            CasualLeave,
            S,
            SickLeave,
            Worker,
            Staff,
            I,
            Inn,
            O,
            Out,
            Office,
            Resign,
        }
        public enum Days
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday

        }
        public enum ErrorType
        {
            Error,
            Information,
            Warning,

        }
        public enum Status
        {
            Complete,
            InProcess,

        }
        public enum NotificationMessageCategory
        {
            AttendanceMachine,
            Attendance,
            Accounts,
            Advance,
            Loan,
            Employee,
            NightOverTime,
        }
        public enum ColumnType
        {
            S,
            U,
            System,
            User,
        }
        public enum NotificationStatus
        {
            Closed,
            Cancelled,
            Checked,
            Approved,

        }
    }
}
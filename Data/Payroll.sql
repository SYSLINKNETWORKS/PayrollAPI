use TWP_DB_Payroll
go




--SELECT
--      QUOTENAME(SCHEMA_NAME(sOBJ.schema_id)) + '.' + QUOTENAME(sOBJ.name) AS [TableName]
--      , SUM(sPTN.Rows) AS [RowCount]
--FROM 
--      sys.objects AS sOBJ
--      INNER JOIN sys.partitions AS sPTN
--            ON sOBJ.object_id = sPTN.object_id
--WHERE
--      sOBJ.type = 'U'
--      AND sOBJ.is_ms_shipped = 0x0
--      AND index_id < 2 -- 0:Heap, 1:Clustered
--GROUP BY 
--      sOBJ.schema_id
--      , sOBJ.name
--ORDER BY [TableName]
--GO


--insert into Cf_Allowance (Id,Name,Amount,Fix,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Amount,Fix,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate from twp_db_new.dbo.Cf_Allowance


--insert into Cf_AnnualLeaves(Id,Date,Name,AnnualLeaveAllow,AnnualLeaveDays,SickLeaveAllow,SickLeaveDays,CasualLeaveAllow,CasualLeaveDays,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,Name,AnnualLeaveAllow,AnnualLeaveDays,SickLeaveAllow,SickLeaveDays,CasualLeaveAllow,CasualLeaveDays,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_AnnualLeaves


--insert into Cf_Department(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_Department


--insert into Cf_Designation(Id,Name,Director,Salesman,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Director,Salesman,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_Designation


--insert into Cf_EmployeeCategory(Id,Name,Type,SalaryAccountNo,BonusAccountNo,EidAccountNo,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,SalaryAccountNo,BonusAccountNo,EidAccountNo,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_EmployeeCategory

--insert into Cf_Roster(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_Roster

--insert into Cf_RosterGroup(Id,Date,OverTime,WorkingHours,Late,EarlyGoing,EarlyOvertime,MorningWorkingHours,EveningWorkingHours,
--MondayCheck,MondayInn,MondayOut,MondayWorkingHours,
--TuesdayCheck,TuesdayInn,TuesdayOut,TuesdayWorkingHours,
--WednesdayCheck,WednesdayInn,WednesdayOut,WednesdayWorkingHours,
--ThursdayCheck,ThursdayInn,ThursdayOut,ThursdayWorkingHours,
--FridayCheck,FridayInn,FridayOut,FridayWorkingHours,
--SaturdayCheck,SaturdayInn,SaturdayOut,SaturdayWorkingHours,
--SundayCheck,SundayInn,SundayOut,SundayWorkingHours,
--RosterId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,OverTime,WorkingHours,Late,EarlyGoing,EarlyOvertime,MorningWorkingHours,EveningWorkingHours,
--MondayCheck,MondayInn,MondayOut,MondayWorkingHours,
--TuesdayCheck,TuesdayInn,TuesdayOut,TuesdayWorkingHours,
--WednesdayCheck,WednesdayInn,WednesdayOut,WednesdayWorkingHours,
--ThursdayCheck,ThursdayInn,ThursdayOut,ThursdayWorkingHours,
--FridayCheck,FridayInn,FridayOut,FridayWorkingHours,
--SaturdayCheck,SaturdayInn,SaturdayOut,SaturdayWorkingHours,
--SundayCheck,SundayInn,SundayOut,SundayWorkingHours,
--RosterId,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_RosterGroup




--insert into cf_employee(Id,MachineId,Name,FatherName,Address,AddressPermanent,DateofJoin,Gender,Married,DateofBirth,CNIC,CNICExpire,NTN,Phone,Mobile,Email,CompanyExpirence,CompanyExpirenceDescription,CompanyExpirenceFrom,CompanyExpirenceTo,
--CompanyExpirenceRemarks,QualificationInstitute,Qualification,QualificationYear,QualificationRemarks,Gratuity,EOBI,EOBIRegistrationDate,EOBIRegistrationNo,SESSI,SESSIRegistrationDate,SESSIRegistrationNo,
--StopPayment,ResignationCheck,ResignationDate,ResignationRemarks,ModeOfPayment,SalaryAccount,OverTime,OverTimeHoliday,OverTimeRate,OvertimeSaturday,OverTimeFactory,
--LateDeduction,AttendanceAllowance,AttendanceExempt,OverTimeRateCheck,DocumentAuthorize,TemporaryPermanent,OverTimeSpecialRate,IncomeTax,
--ProvisionPeriod,DateofParmanent,EmergencyContactOne,EmergencyContactTwo,Remarks,TakafulRate,OfficeWorker,ReferenceOne,ReferenceCNICOne,ReferenceAddressOne,ReferenceContactOne,
--ReferenceTwo,ReferenceCNICTwo,ReferenceAddressTwo,ReferenceContactTwo,BranchId,EmployeeCategoryId,DesignationId,DepartmentId,AnnualLeavesId,RosterId,Type,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,MachineId,Name,FatherName,Address,AddressPermanent,DateofJoin,Gender,Married,DateofBirth,CNIC,CNICExpire,NTN,Phone,Mobile,Email,CompanyExpirence,CompanyExpirenceDescription,CompanyExpirenceFrom,CompanyExpirenceTo,
--CompanyExpirenceRemarks,QualificationInstitute,Qualification,QualificationYear,QualificationRemarks,Gratuity,EOBI,EOBIRegistrationDate,EOBIRegistrationNo,SESSI,SESSIRegistrationDate,SESSIRegistrationNo,
--StopPayment,ResignationCheck,ResignationDate,ResignationRemarks,ModeOfPayment,SalaryAccount,OverTime,OverTimeHoliday,OverTimeRate,OvertimeSaturday,OverTimeFactory,
--LateDeduction,AttendanceAllowance,AttendanceExempt,OverTimeRateCheck,DocumentAuthorize,TemporaryPermanent,OverTimeSpecialRate,IncomeTax,
--ProvisionPeriod,DateofParmanent,EmergencyContactOne,EmergencyContactTwo,Remarks,TakafulRate,OfficeWorker,ReferenceOne,ReferenceCNICOne,ReferenceAddressOne,ReferenceContactOne,
--ReferenceTwo,ReferenceCNICTwo,ReferenceAddressTwo,ReferenceContactTwo,BranchId,EmployeeCategoryId,DesignationId,DepartmentId,AnnualLeavesId,RosterId,Type,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.cf_employee

--insert into Cf_EmployeeImage(Id,ImageProfileCheck,ImageName,ImageBytes,ImageExtension,EmployeeId)
--select Id,ImageProfileCheck,ImageName,ImageBytes,ImageExtension,EmployeeId from twp_db_new.dbo.Cf_EmployeeImage

--insert into Cf_EmployeeAllowances(Id,EmployeeId,AllowanceId)
--select Id,EmployeeId,AllowanceId from twp_db_new.dbo.Cf_EmployeeAllowances


--insert into Cf_InOutCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_InOutCategory

--insert into Cf_IncomeTaxSlabEmployee(Id,Date,SlabFrom,SlabTo,Percentage,Amount,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,SlabFrom,SlabTo,Percentage,Amount,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_IncomeTaxSlabEmployee

--insert into Cf_AttendanceMachineCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_AttendanceMachineCategory


--insert into Cf_AttendanceMachine(Id,Name,Ip,Port,Category,AttendanceMachineCategoryId,Type,BranchId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Ip,Port,Category,AttendanceMachineCategoryId,Type,BranchId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_AttendanceMachine

--insert into Cf_LoanCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Name,Type,CompanyId,Active,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.Cf_LoanCategory

--insert into Cf_DailyDate(Date,HolidayCheck,Remarks)
--select Date,HolidayCheck,Remarks  from twp_db_new.dbo.Cf_DailyDate

--insert into T_ABSENT(Id,Date,Approved,ApprovedAdjust,ApprovedAdjustType,EmployeeId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,Approved,ApprovedAdjust,ApprovedAdjustType,EmployeeId,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_ABSENT

--insert into T_AnnualLeaveAdjustment(Id,Date,LeaveAdjust,ApprovedAdjustType,EmployeeId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,LeaveAdjust,ApprovedAdjustType,EmployeeId,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_AnnualLeaveAdjustment

--insert into T_CheckAttendance(EmployeeId,Date,Inn,Out,Minutes,Approved,AttendanceMachineIdInn,AttendanceMachineIdOut,CompanyId,UserNameInsert,InsertDate)
--select EmployeeId,Date,Inn,Out,Minutes,Approved,AttendanceMachineIdInn,AttendanceMachineIdOut,CompanyId,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate  from twp_db_new.dbo.T_CheckAttendance

--insert into T_CheckInOut(MachineId,CheckTime,Date,CheckType,VerifyCode,SensorId,Status,Type,Approved,Remarks,Latitude,Longitude,Address,AttendanceMachineId,
--InOutCategoryId,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select MachineId,CheckTime,Date,CheckType,VerifyCode,SensorId,Status,Type,Approved,Remarks,Latitude,Longitude,Address,AttendanceMachineId,
--InOutCategoryId,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_CheckInOut

--insert into T_Holiday(Id,Date,HolidayCheck,FactoryOverTimeCheck,Remarks,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,HolidayCheck,FactoryOverTimeCheck,Remarks,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_Holiday


--insert into T_LoanIssue(Id,Date,Amount,NoOfInstalment,InstalmentAmount,LoanCategoryId,LoanStatus,EmployeeId,CheaqueCash,Remarks,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,Amount,NoOfInstalment,InstalmentAmount,LoanCategoryId,LoanStatus,EmployeeId,CheaqueCash,Remarks,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_LoanIssue

--insert into T_LoanReceive(Id,Date,Amount,LoanIssueId,CheaqueCash,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,Amount,LoanIssueId,CheaqueCash,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_LoanReceive

--insert into T_Salary(Id,Date,PreviousAmount,IncreamentPercentage,IncreamentAmount,CurrentAmount,EmployeeId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,PreviousAmount,IncreamentPercentage,IncreamentAmount,CurrentAmount,EmployeeId,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_Salary

--insert into T_SalaryAdditionDeduction(Id,Date,AdditionAmount,DeductionAmount,EmployeeId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select Id,Date,AdditionAmount,DeductionAmount,EmployeeId,Type,CompanyId,Action,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdUpdate) as UserNameUpdate, UpdateDate,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdDelete) as UserNameDelete,DeleteDate  from twp_db_new.dbo.T_SalaryAdditionDeduction

--insert into T_StaffSalary(EmployeeId,Date,CheckIncomeTax,Takaful,DateOfJoining,DateOfResign,AttendanceExempted,CheckAttendanceAllowance,CheckOvertime,SalaryAmount,SalaryAllowanceAmount,SalaryGrossAmount,NoOfDaysMonth,WorkingDays,ResignDays,JoinDays,WorkingHours,SalaryPerDay,SalaryPerHour,SalaryPerminute,SalaryGrossPerDay,SalaryGrossPerHour,SalaryGrossPerMinute,PresentDays,AbsentDays,AbsentDaysApproval,
--TotalAbsentDays,AdjustedDays,AdvanceAmount,LoanAmount,IncomeTaxAmount,AdditionAmount,DeductionAmount,AttendanceAllowanceAmount,OvertimeMinutes,OvertimeHours,
--OvertimeDays,LateMinutes,LateHours,LateDays,LateDaysTotal,AbsentMinutes,AbsentHours,OvertimeRate,OvertimeActual,OvertimeActualAmount,AbsentDaysActual,AbsentDaysActualAmount,
--LateDaysActual,LateDaysActualAmount,Amount,GrossAmount,PayableAmount,AllowanceAbsent,VoucherPostCk,VoucherNo,CompanyId,UserNameInsert,InsertDate)
--select EmployeeId,Date,CheckIncomeTax,Takaful,DateOfJoining,DateOfResign,AttendanceExempted,CheckAttendanceAllowance,CheckOvertime,SalaryAmount,SalaryAllowanceAmount,SalaryGrossAmount,NoOfDaysMonth,WorkingDays,ResignDays,JoinDays,WorkingHours,SalaryPerDay,SalaryPerHour,SalaryPerminute,SalaryGrossPerDay,SalaryGrossPerHour,SalaryGrossPerMinute,PresentDays,AbsentDays,AbsentDaysApproval,
--TotalAbsentDays,AdjustedDays,AdvanceAmount,LoanAmount,IncomeTaxAmount,AdditionAmount,DeductionAmount,AttendanceAllowanceAmount,OvertimeMinutes,OvertimeHours,
--OvertimeDays,LateMinutes,LateHours,LateDays,LateDaysTotal,AbsentMinutes,AbsentHours,OvertimeRate,OvertimeActual,OvertimeActualAmount,AbsentDaysActual,AbsentDaysActualAmount,
--LateDaysActual,LateDaysActualAmount,Amount,GrossAmount,PayableAmount,AllowanceAbsent,VoucherPostCk,VoucherNo,CompanyId,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate
--from twp_db_new.dbo.T_StaffSalary

--insert into T_WorkerSalary(EmployeeId,Date,CheckIncomeTax,Takaful,DateOfJoining,DateOfResign,AttendanceExempted,CheckAttendanceAllowance,CheckOvertime,SalaryAmount,SalaryAllowanceAmount,
--SalaryGrossAmount,NoOfDaysMonth,WorkingDays,ResignDays,JoinDays,WorkingHours,SalaryPerDay,SalaryPerHour,
--SalaryPerminute,SalaryGrossPerDay,SalaryGrossPerHour,SalaryGrossPerMinute,PresentDays,AbsentDays,AbsentDaysApproval,TotalAbsentDays,AdjustedDays,AdvanceAmount,
--LoanAmount,IncomeTaxAmount,AdditionAmount,DeductionAmount,AttendanceAllowanceAmount,OvertimeMinutes,OvertimeHours,OvertimeDays,LateMinutes,LateHours,LateDays,LateDaysTotal,
--AbsentMinutes,AbsentHours,OvertimeRate,OvertimeActual,OvertimeActualAmount,AbsentDaysActual,AbsentDaysActualAmount,LateDaysActual,LateDaysActualAmount,
--Amount,GrossAmount,PayableAmount,AllowanceAbsent,VoucherPostCk,VoucherNo,CompanyId,UserNameInsert,InsertDate)
--select EmployeeId,Date,CheckIncomeTax,Takaful,DateOfJoining,DateOfResign,AttendanceExempted,CheckAttendanceAllowance,CheckOvertime,SalaryAmount,SalaryAllowanceAmount,
--SalaryGrossAmount,NoOfDaysMonth,WorkingDays,ResignDays,JoinDays,WorkingHours,SalaryPerDay,SalaryPerHour,
--SalaryPerminute,SalaryGrossPerDay,SalaryGrossPerHour,SalaryGrossPerMinute,PresentDays,AbsentDays,AbsentDaysApproval,TotalAbsentDays,AdjustedDays,AdvanceAmount,
--LoanAmount,IncomeTaxAmount,AdditionAmount,DeductionAmount,AttendanceAllowanceAmount,OvertimeMinutes,OvertimeHours,OvertimeDays,LateMinutes,LateHours,LateDays,LateDaysTotal,
--AbsentMinutes,AbsentHours,OvertimeRate,OvertimeActual,OvertimeActualAmount,AbsentDaysActual,AbsentDaysActualAmount,LateDaysActual,LateDaysActualAmount,
--Amount,GrossAmount,PayableAmount,AllowanceAbsent,VoucherPostCk,VoucherNo,CompanyId,(select NormalizedUserName from twp_db_new.dbo.users where users.id=UserIdInsert) as UserNameInsert,InsertDate
--from twp_db_new.dbo.T_WorkerSalary

--sp_help T_WorkerSalary








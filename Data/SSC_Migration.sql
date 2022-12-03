--insert into Cf_Allowance (Id,Name,Amount,Fix,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),all_nam,all_amt,all_fix,all_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_all

--insert into Cf_AnnualLeaves (Id,Date,Name,AnnualLeaveAllow,AnnualLeaveDays,SickLeaveAllow,SickLeaveDays,CasualLeaveAllow,CasualLeaveDays,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),empanl_dat,empanl_name,empanl_ck_al,empanl_al,empanl_ck_sl,empanl_sl,empanl_ck_cl,empanl_cl,empanl_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_empanl


--insert into Cf_Department(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),dpt_nam,dpt_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_dpt where dpt_nam not in (select name from cf_department)


--insert into cf_designation(Id,Name,Director,Salesman,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),memp_sub_nam,memp_sub_salman,0,memp_sub_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_emp_sub

--insert into Cf_EmployeeCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),mempcat_nam,mempcat_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_empcat

--insert into Cf_InOutCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),inoutcat_nam,inoutcat_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_inoutcat

--insert into Cf_LoanCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),mloan_nam,mloan_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_loan

--insert into Cf_AttendanceMachineCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),mac_com_nam,mac_com_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_mac_com

--insert into Cf_Roster(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),ros_nam,ros_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_ros

--insert into Cf_RosterGroup(Id,Date,OverTime,WorkingHours,Late,EarlyGoing,EarlyOvertime,MorningWorkingHours,EveningWorkingHours,
--MondayCheck,MondayInn,MondayOut,MondayWorkingHours,
--TuesdayCheck,TuesdayInn,TuesdayOut,TuesdayWorkingHours,
--WednesdayCheck,WednesdayInn,WednesdayOut,WednesdayWorkingHours,
--ThursdayCheck,ThursdayInn,ThursdayOut,ThursdayWorkingHours,
--FridayCheck,FridayInn,FridayOut,FridayWorkingHours,
--SaturdayCheck,SaturdayInn,SaturdayOut,SaturdayWorkingHours,
--SundayCheck,SundayInn,SundayOut,SundayWorkingHours,
--RosterId,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),rosgp_dat,rosgp_ota,rosgp_wh,rosgp_lat,rosgp_ear,isnull(rosgp_earot,0),rosgp_mwh,rosgp_ewh,
--rosgp_ck_mon,rosgp_in_mon,rosgp_out_mon,rosgp_mon_wh,
--rosgp_ck_tue,rosgp_in_tue,rosgp_out_tue,rosgp_tue_wh,
--rosgp_ck_wed,rosgp_in_wed,rosgp_out_wed,rosgp_wed_wh,
--rosgp_ck_thu,rosgp_in_thu,rosgp_out_thu,rosgp_thu_wh,
--rosgp_ck_fri,rosgp_in_fri,rosgp_out_fri,rosgp_fri_wh,
--rosgp_ck_sat,rosgp_in_sat,rosgp_out_sat,rosgp_sat_wh,
--rosgp_ck_sun,rosgp_in_sun,rosgp_out_sun,rosgp_sun_wh,
--(select cf_roster.id from Cf_Roster where [name]=ros_nam),
--rosgp_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4','A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_rosgp.usr_id_ins),''),isnull(m_rosgp.ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_rosgp.usr_id_upd),''),isnull(m_rosgp.upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_rosgp
--inner join SSC_DB.dbo.m_ros on m_rosgp.ros_id=m_ros.ros_id
--where emppro_macid not in (select MachineId from Cf_Employee)



--insert into cf_employee(Id,MachineId,Name,FatherName,Address,AddressPermanent,DateofJoin,Gender,Married,DateofBirth,CNIC,CNICExpire,NTN,Phone,Mobile,Email,
--CompanyExpirence,CompanyExpirenceDescription,CompanyExpirenceFrom,CompanyExpirenceTo,CompanyExpirenceRemarks,
--QualificationInstitute,Qualification,QualificationYear,QualificationRemarks,Gratuity,
--EOBI,EOBIRegistrationDate,EOBIRegistrationNo,
--SESSI,SESSIRegistrationDate,SESSIRegistrationNo,
--StopPayment,ResignationCheck,ResignationDate,ResignationRemarks,
--ModeOfPayment,SalaryAccount,
--OverTime,OverTimeHoliday,OverTimeRate,OvertimeSaturday,OverTimeFactory,LateDeduction,
--AttendanceAllowance,AttendanceExempt,OverTimeRateCheck,
--DocumentAuthorize,TemporaryPermanent,OverTimeSpecialRate,IncomeTax,ProvisionPeriod,
--DateofParmanent,EmergencyContactOne,EmergencyContactTwo,Remarks,TakafulRate,OfficeWorker,
--ReferenceOne,ReferenceCNICOne,ReferenceAddressOne,ReferenceContactOne,
--ReferenceTwo,ReferenceCNICTwo,ReferenceAddressTwo,ReferenceContactTwo,
--BranchId,EmployeeCategoryId,DesignationId,DepartmentId,AnnualLeavesId,RosterId,
--Type,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),emppro_macid,emppro_nam,emppro_fnam,emppro_add,emppro_add,emppro_doj,
--case emppro_gen when 'M' then 'Male' when 'F' then 'Female' end,
--case emppro_mar when 'M' then 'Married' when 'U' then 'Unmarried' end,emppro_dob,emppro_cnic,getdate(),emppro_ntn,emppro_pho,emppro_mob,emppro_eml,
--emppro_expcom,emppro_expdes,emppro_expyrfrm,emppro_expyrto,emppro_exprmk,
--emppro_quains,emppro_quaqua,emppro_quayr,emppro_quarmk,
--emppro_salgra,emppro_saleobi,emppro_saleobi_dor,emppro_saleobi_reg,emppro_salsessi,emppro_dat_sessi,emppro_sessi_no,
--emppro_salsp,emppro_reg,emppro_reg_dat,emppro_reg_rmk,'Cash',emppro_salpay_acc,
--emppro_ot,emppro_ho,emppro_rat,emppro_sot,emppro_fot,emppro_lde,emppro_att,emppro_attexp,emppro_ckrat,0,'Permanent',0,0,0,emppro_doj,emppro_Ref,'','',0,'Office',emppro_ref,'','','','','','','',
--'79BB8FE0-BEFA-4DB5-ACC4-08D9659D19D9',(select Cf_EmployeeCategory.id from Cf_EmployeeCategory where Cf_EmployeeCategory.name=m_empcat.mempcat_nam),
--(select top 1 Cf_Designation.id from Cf_Designation where Cf_Designation.name=m_emp_Sub.memp_sub_nam),
--(select Cf_Department.id from Cf_Department where Cf_Department.name=m_dpt.dpt_nam),(select top 1 Cf_AnnualLeaves.id from Cf_AnnualLeaves),
--(select Cf_Roster.id from Cf_Roster where Cf_Roster.name=m_ros.ros_nam),
--emppro_typ,1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_emppro.usr_id_ins),''),isnull(m_Emppro.ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_Emppro.usr_id_upd),''),isnull(m_emppro.upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_emppro
--inner join ssc_db.dbo.m_empcat on m_emppro.emppro_cat=m_empcat.mempcat_id
--inner join ssc_db.dbo.m_emp_sub on m_emppro.memp_sub_id=m_emp_sub.memp_sub_id
--inner join ssc_db.dbo.m_dpt on m_emppro.dpt_id=m_dpt.dpt_id
--inner join ssc_db.dbo.m_ros on m_emppro.ros_id=m_ros.ros_id

--insert into Cf_AttendanceMachineCategory(Id,Name,Type,CompanyId,Active,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),mac_com_nam,mac_com_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',1,'A',isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_ins),''),isnull(ins_dat,getdate()),isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=usr_id_upd),''),isnull(upd_dat,getdate()),'',getdate() from ssc_db.dbo.m_mac_com



--insert into T_CheckInOut(MachineId,CheckTime,Date,CheckType,VerifyCode,SensorId,Status,Type,Approved,Remarks,Latitude,Longitude,Address,AttendanceMachineId,InOutCategoryId,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select userid,CHECKTIME,checkdate,CHECKTYPE,VERIFYCODE,null,ckinout_st,check_typ,check_app,
--'',0,0,'',
--(select Cf_AttendanceMachine.id from Cf_AttendanceMachine where name=m_mac.mac_nam ) as [mac],
--(select Cf_InOutCategory.id from Cf_InOutCategory where name=m_inoutcat.inoutcat_nam ) as inoutcat_name,
--'5e0a69cd-6b2e-4281-97e9-08d9659d19c4','A',
--isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=CHECKINOUT.usr_id_ins),''),isnull(CHECKINOUT.ins_dat,getdate()),
--isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=CHECKINOUT.usr_id_upd),''),isnull(CHECKINOUT.upd_dat,getdate()),
--'',GETDATE()
--from ssc_db.dbo.CHECKINOUT
--left join ssc_db.dbo.m_inoutcat on CHECKINOUT.inoutcat_id=m_inoutcat.inoutcat_id
--left join ssc_db.dbo.m_mac on CHECKINOUT.mac_id=m_mac.mac_id

--insert into T_Holiday(Id,Date,HolidayCheck,FactoryOverTimeCheck,Remarks,Type,CompanyId,Action,UserNameInsert,InsertDate,UserNameUpdate,UpdateDate,UserNameDelete,DeleteDate)
--select NEWID(),mholi_dat,mholi_dayact,mholi_fovertime,mholi_rmks,mholi_typ,'5e0a69cd-6b2e-4281-97e9-08d9659d19c4',
--log_act,isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_holi.usr_id_ins),''),isnull(m_holi.ins_dat,getdate()),
--isnull((select usr_nam from ssc_db.dbo.new_usr where usr_id=m_holi.usr_id_upd),''),isnull(m_holi.upd_dat,getdate()),'',GETDATE()
--from SSC_DB.dbo.m_holi
select * from ssc_db.dbo.CHECKINOUT where USERID=116 and checkdate='2021-10-01'
select * from SSC_DB_Payroll.dbo.T_CheckInOut where MachineId=116 and Date='2021-10-01'


--insert into Cf_DailyDate(Date,HolidayCheck,Remarks)
--	select tbldat_dat,tbldat_hol,tbldat_rmk from SSC_DB.dbo.tbl_dat

--select * from ssc_dbo.dbo.m_empcat



--sp_help cf_employee


--sp_help Cf_AttendanceMachine



--select * from ssc_db.dbo.m_empanl



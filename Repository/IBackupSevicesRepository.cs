using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWP_API.ViewModels;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;

namespace TWP_API_Payroll.Repository
{
    public interface IBackupSevicesRepository
    {

        Task<ApiResponse> CreateDBBackupAsync(ClaimsPrincipal _User);
        Task<ApiResponse> DBBackupShowAsync(ClaimsPrincipal _User);
        Task<ApiResponse> DBBackupDeleteAsync(ClaimsPrincipal _User, string _FileName);
        FileStreamResult DBBackupDownloadAsync(ClaimsPrincipal _User, string _FileName);

    }
    public class BackupSevicesRepository : IBackupSevicesRepository
    {
        private readonly DataContext _context = null;

        private ErrorLog _ErrorLog = new ErrorLog();
        string _connectionString = "", _UserId = "", BackupFolderName = "db_backup";

        public BackupSevicesRepository(DataContext context)
        {
            _context = context;
            var configuration = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", false)
                          .Build();
            _connectionString = configuration.GetConnectionString("Connection").ToString();
        }

        public async Task<ApiResponse> CreateDBBackupAsync(ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            try
            {

                string strdata, strsrv, name1;

                apiResponse = await _ErrorLog.ServiceName();
                if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
                {
                    return apiResponse;
                }
                var _ServiceTable = (LovServicesViewModel)apiResponse.data;

                string filepath = Directory.GetCurrentDirectory() + "\\" + BackupFolderName;
                //Check Folder
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                    //Directory.CreateDirectory(Server.MapPath("~\\db_backup"));
                }
                string[] _str = _connectionString.Split(';');
                int SRV_index = _str[0].IndexOf('=') + 1;
                int DB_index = _str[1].IndexOf('=') + 1;

                strsrv = _str[0].Substring(SRV_index, _str[0].Length - SRV_index);
                strdata = _str[1].Substring(DB_index, _str[1].Length - DB_index);

                name1 = _ServiceTable.Name.Trim() + "_" + DateTime.Now.Year + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") + "_" + DateTime.Now.Hour.ToString("d2") + DateTime.Now.Minute.ToString("d2") + DateTime.Now.Second + DateTime.Now.Millisecond;

                string name = name1 + ".BAK";

                Microsoft.SqlServer.Management.Common.ServerConnection srv = new Microsoft.SqlServer.Management.Common.ServerConnection();
                srv.ConnectionString = _connectionString;

                Server op_srv = new Server(srv);
                DatabaseCollection dbcollection = op_srv.Databases;

                Database db = default(Database);
                db = op_srv.Databases[strdata];

                Backup bkp = new Backup();

                bkp.Devices.AddDevice(filepath + "\\" + name, DeviceType.File);
                bkp.Database = strdata;
                bkp.Action = BackupActionType.Database;
                bkp.Initialize = true;
                bkp.PercentCompleteNotification = 1;
                bkp.SqlBackup(op_srv);

                //Zip
                string _FileToZip = filepath + "\\" + name;
                byte[] bt = File.ReadAllBytes(_FileToZip);
                using (FileStream zipToOpen = new FileStream(filepath + "\\" + name1 + ".ZIP", FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(name);

                        using (Stream writer = readmeEntry.Open())
                        {

                            writer.Write(bt, 0, bt.Length);
                        }
                    }
                }


                //Delete BAK
                FileInfo fi = new FileInfo(filepath + "\\" + name1 + ".BAK");
                fi.Delete();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = "Backup Complete";
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;

            }
            //return await _DBBackup.CreateDBBackupAsync(_User);
        }

        public async Task<ApiResponse> DBBackupShowAsync(ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            List<DBBackupViewModel> _DBBackupViewModel = new List<DBBackupViewModel>();
            try
            {
                string filepath = Directory.GetCurrentDirectory() + "\\" + BackupFolderName; // System.Web.Hosting.HostingEnvironment.MapPath("~\\db_backup");
                string[] filePaths = Directory.GetFiles(filepath);
                if (filePaths.Length > 0)
                {
                    for (int i = 0; i < filePaths.Length; i++)
                    {

                        DateTime _filedate = new DateTime();
                        string _FileName = Path.GetFileName(filePaths[i]);

                        FileInfo _finfo = new FileInfo(filepath + "\\" + _FileName);
                        _filedate = System.IO.File.GetLastWriteTime(filepath + "\\" + Path.GetFileName(filePaths[i]));
                        double filesize = Math.Round(Convert.ToDouble((_finfo.Length / 1024)) / 1024, 4);
                        //   string _Url = "http://" + _FileName;
                        _DBBackupViewModel.Add(
                            new DBBackupViewModel
                            {
                                Date = _filedate.ToString("dd-MMM-yyyy"),
                                Time = _filedate.ToShortTimeString(),
                                FileName = _FileName,
                                Size = filesize,
                                // Url = _Url,
                            });
                    }
                    apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    apiResponse.data = _DBBackupViewModel;
                }
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;

            }
        }

        public async Task<ApiResponse> DBBackupDeleteAsync(ClaimsPrincipal _User, string _FileName)
        {
            ApiResponse apiResponse = new ApiResponse();
            _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            try
            {
                if (_FileName.Trim() == "")
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "File not found";
                    return apiResponse;
                }
                else
                {
                    string filepath = Directory.GetCurrentDirectory() + "\\" + BackupFolderName; // System.Web.Hosting.HostingEnvironment.MapPath("~\\db_backup");
                    string FileName = filepath + "//" + _FileName;
                    if (File.Exists(FileName))
                    {
                        System.IO.FileInfo fi = new FileInfo(FileName);
                        fi.Delete();
                        apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                        apiResponse.message = "Backup Delete";
                        return apiResponse;
                    }
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "File not found";
                    return apiResponse;

                }
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;

            }
        }

        public FileStreamResult DBBackupDownloadAsync(ClaimsPrincipal _User, string _FileName)
        {
            // ApiResponse apiResponse = new ApiResponse();
            // DBBackupDownloadViewModel _DBBackupDownloadViewModel = new DBBackupDownloadViewModel();

            // _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            // try
            // {
            // if (_FileName.Trim() == "")
            // {
            //     apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
            //     apiResponse.message = "File not found";
            //     return apiResponse;
            // }
            // else
            // {
            //     string _hostingEnvironment = Directory.GetCurrentDirectory() + "\\" + BackupFolderName;
            //     var path = Path.Combine(_hostingEnvironment, _FileName);
            //     var fs = new FileStream(path, FileMode.Open);

            //     // Return the file. A byte array can also be used instead of a stream
            //     //  return File (fs, "application/octet-stream", _FileName);
            //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            //     apiResponse.data = _DBBackupDownloadViewModel;
            //     // return apiResponse;
            //     return null;
            // }
            string _hostingEnvironment = Directory.GetCurrentDirectory() + "\\" + BackupFolderName;
            var path = Path.Combine(_hostingEnvironment, _FileName);
            if (File.Exists(path))
            {
                var fs = new FileStream(path, FileMode.Open);

                // Return the file. A byte array can also be used instead of a stream
                Microsoft.AspNetCore.Mvc.FileStreamResult _File = new Microsoft.AspNetCore.Mvc.FileStreamResult(fs, "application/octet-stream");
                _File.FileDownloadName = _FileName;
                return _File;
            }
            return null;
            // return Ok();
            //                return Ok();
            //                return Microsoft.AspNetCore.Mvc.FileStreamResult File(fs, "application/octet-stream", _FileName);

            // }
            // catch (Exception e)
            // {

            //     string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
            //     //string _ErrorId = _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
            //     // apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            //     // apiResponse.message = _ErrorId;
            //     //return apiResponse;
            //     return null;
            // }
        }
    }
}
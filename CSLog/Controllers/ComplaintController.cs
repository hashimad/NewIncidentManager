using CSLog.Domain.Entities;
using CSLog.Domain.Repositories;
using CSLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CSLog.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ComplaintController : Controller
    {
        private readonly ComplaintRepository _complaint;
        private readonly vComplaintRepository _vComplaint;
        private readonly ComplaintActivityRepository _complaintActivity;
        private readonly vComplaintActivityRepository _vComplaintActivity;
        private readonly ComplaintTypeRepository _complaintType;
        private readonly SolutionStatusRepository _slnStatus;
   
        private readonly ASPNetUserRepository _user;
        private readonly ComplaintStatusRepository _complaintStatus;
        private readonly Helper _help;

        public ComplaintController()
        {
            _complaint = new ComplaintRepository();
            _vComplaint = new vComplaintRepository();
            _complaintActivity = new ComplaintActivityRepository();
            _vComplaintActivity = new vComplaintActivityRepository();
            _complaintType = new ComplaintTypeRepository();
            _slnStatus = new SolutionStatusRepository();
       
            _user = new ASPNetUserRepository();
            _complaintStatus = new ComplaintStatusRepository();
            _help = new Helper();
        }

        // GET: Complaint
        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now;
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = 0;
            ViewBag.ComplaintStatus = _complaintStatus.GetAll();
            ViewBag.currentStatus = 0;
            ViewBag.SolutionStatus = _slnStatus.GetAll();
            ViewBag.currentSolutionStatus = 0;
            ViewBag.Agents = _user.GetAllComplaintUsers();//.GetAll();
            ViewBag.currentAgent = 0;
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            List<vComplaint> complaints = new List<vComplaint>();
            if (User.IsInRole("Admin"))
            {
               
                complaints = _vComplaint.GetTodayComplaint().Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();//GetTodayComplaint(user.UserId).Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();
            }
            else if(User.IsInRole("User"))
            {
                var user = _user.GetUser(User.Identity.Name);
                complaints = _vComplaint.GetTodayComplaint(user.UserId).Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();//.GetTodayComplaint().Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();
            }
            else
            {
                var user = _user.GetUser(User.Identity.Name);
                complaints = _vComplaint.GetTodayComplaintBySupportUser(user.UserId).Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();//.GetTodayComplaint().Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();
            }

            ViewBag.Msg = TempData["Msg"];
            return View(complaints);
        }

        [HttpPost]
        public ActionResult Index(DateTime date, int complaintStatus, int complaintType, int solnStatus, int agent)
        {
            ViewBag.Date = date;
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = complaintType;
            ViewBag.ComplaintStatus = _complaintStatus.GetAll();
            ViewBag.currentStatus = complaintStatus;
            ViewBag.SolutionStatus = _slnStatus.GetAll();
            ViewBag.currentSolutionStatus = solnStatus;
            ViewBag.Agents = _user.GetAll();
            ViewBag.currentAgent = agent;
            var complaints = _vComplaint.FilterComplaint(date, date, complaintType, complaintStatus, solnStatus, agent).Where(c => c.ComplaintStatus.ToUpper() != "CLOSED").ToList();
            return View(complaints);
        }

        public ActionResult Resolved()
        {
            ViewBag.sDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.eDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = 0;
          
            int UserId = 0;
            if (Session["UserId"] == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else
            {
                UserId = (int)Session["UserId"];
            }
        
            if (User.IsInRole("Admin"))
            {
              var  complaints = _vComplaint.GetTodayComplaint().Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList();
              return View(complaints);
            }
            else
            {
               var complaints = _vComplaint.GetTodayComplaint(UserId).Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList();
               return View(complaints);
            }
            
           
        }

        [HttpPost]
        public ActionResult Resolved(DateTime sDate, DateTime eDate, int typeId)
        {
            ViewBag.sDate = sDate.ToString("dd/MM/yyyy");
            ViewBag.eDate = eDate.ToString("dd/MM/yyyy");
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = typeId;
            int UserId = 0;
            if (Session["UserId"] == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else
            {
                UserId = (int)Session["UserId"];
            }

            if (User.IsInRole("Admin"))
            {
                var complaints = _vComplaint.FilterComplaint(sDate, eDate, typeId, 0)
                    .Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList();
                return View(complaints);
            }
            else
            {
                var complaints = _vComplaint.FilterComplaint(sDate, eDate, typeId,0,0,UserId)
                    .Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList();
                return View(complaints);
            }

           // var complaints = _vComplaint.FilterComplaint(sDate, eDate,typeId).Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList();
          
        }

        public ActionResult Report()
        {
            ViewBag.sDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.eDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SolutionStatus = _slnStatus.GetAll().Where(c => c.Name != "RESOLVED FULLY").ToList();
            ViewBag.currentStatus = 0;
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = 0;

            int UserId = 0;
            if (Session["UserId"] == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else
            {
                UserId = (int)Session["UserId"];
            }

            if (User.IsInRole("Admin"))
            {
                var complaints = _vComplaint.GetTodayComplaint()
                    .Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList();
                return View(complaints);
            }
            else
            {
                var complaints = _vComplaint.GetTodayComplaint(UserId)
                    .Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList();
                return View(complaints);
            }
            //var complaints = _vComplaint.GetTodayComplaint().Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList(); ;
            //return View(complaints);
        }

        [HttpPost]
        public ActionResult Report(DateTime sDate, DateTime eDate, int statusId, int typeId)
        {
            ViewBag.sDate = sDate.ToString("dd/MM/yyyy");
            ViewBag.eDate = eDate.ToString("dd/MM/yyyy");
            ViewBag.SolutionStatus = _slnStatus.GetAll().Where(c => c.Name != "RESOLVED FULLY").ToList();
            ViewBag.currentStatus = statusId;
            ViewBag.ComplaintType = _complaintType.GetAll();
            ViewBag.currentType = typeId;

            int UserId = 0;
            if (Session["UserId"] == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else
            {
                UserId = (int)Session["UserId"];
            }

            if (User.IsInRole("Admin"))
            {
                var complaints = _vComplaint.FilterComplaint(sDate, eDate, typeId, statusId).Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList(); 
                return View(complaints);
            }
            else
            {
                var complaints = _vComplaint.FilterComplaint(sDate, eDate, typeId, statusId,UserId).Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList();
                return View(complaints);
            }

            //var complaints = _vComplaint.FilterComplaint(sDate, eDate,typeId,statusId).Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList(); ;
            //return View(complaints);
        }

        public ActionResult NewComplaint()
        {
            ViewBag.ComplaintType = _complaintType.GetAll();
          //  ViewBag.SolutionType = _slnType.GetAll();
         //   ViewBag.SolutionStatus = _slnStatus.GetAll();
            var user = _user.GetUser(User.Identity.Name);
            ViewBag.Complaints = _vComplaint.FilterComplaint(DateTime.Now.Date, DateTime.Now.Date, 0, 0, 0, user.UserId);
            return View(new NewComplaint());
        }

        [HttpPost]
        public ActionResult NewComplaint(NewComplaint model)
        {
            try
            {
                ViewBag.ComplaintType = _complaintType.GetAll();
                //ViewBag.SolutionType = _slnType.GetAll();
                //ViewBag.SolutionStatus = _slnStatus.GetAll();
                var user = _user.GetUser(User.Identity.Name);
                ViewBag.Complaints = _vComplaint.FilterComplaint(DateTime.Now.Date, DateTime.Now.Date, 0, 0, 0, user.UserId);

                if (model.Details!="" && model.ComplaintTypeId>0)
                {
                    if (model.Title.Trim().Contains(" "))
                    {
                        ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), "Empty space is not allowed in Title, you can replace empty space with character like - or _");
                        return View(model);
                    }
                    //if (!string.IsNullOrEmpty(model.ComplaintOwnerEmail))
                    //{
                    //    if (!_help.IsValidEmail(model.ComplaintOwnerEmail))
                    //    {
                    //        ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), "Invalid email address!");
                    //        return View(model);
                    //    }
                    //}
                    //if (!_help.IsValidPhone(model.MobileNo))
                    //{
                    //    ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), "Invalid phone number!");
                    //    return View(model);
                    //}
                    var compType = _complaintType.GetRecordById(model.ComplaintTypeId);
                    model.Title = compType.Name.Replace(" ", "_") + "_" + model.Title + "_" + model.MobileNo;
                    var cmp = _complaint.GetComplaintByTitle(model.Title);
                    if (cmp == null)
                    {
                        var soln = _slnStatus.GetRecordById(1);
                        if (soln.Name.ToUpper() == "RESOLVED FULLY")
                        {
                            model.StatusId = 4;
                        }
                        else
                        {
                            if (soln.Name.ToUpper() == "REQUIRED TECHNICAL SKILL")
                            {
                                model.StatusId = 3;
                            }
                            else
                            {
                                model.StatusId = 2;
                            }

                        }

                        Complaint nc = new Complaint();
                        nc.Code = model.Code;
                        nc.ComplaintOwnerEmail = null;
                        nc.ComplaintOwnerName = null;
                        nc.ComplaintTypeId = model.ComplaintTypeId;
                        nc.Date = DateTime.Now;
                        nc.Details = model.Details;
                        nc.Location = null;
                        nc.MobileNo = "08066666666";
                        nc.RegisteredBy = user.UserId;
                        nc.SolutionStatusId = 1;
                        nc.StatusId = model.StatusId;
                        nc.Title = model.Title;

                        using (var cmd = new TransactionScope())
                        {
                            try
                            {
                                _complaint.Create(nc);
                                //ComplaintActivity ca = new ComplaintActivity();
                                //ca.ComplaintId = nc.ComplaintId;
                                //ca.Date = DateTime.Now;
                                //ca.RecordedBy = -1;
                                //ca.SolutionDetails = "No Solution Yet";
                                //ca.SolutionStatusId = 1;
                                ////ca.SolutionTypeId = model.SolutionTypeId;
                                //_complaintActivity.Create(ca);
                                cmd.Complete();
                                ViewBag.Msg = _help.getMsg(AlertType.success.ToString(), "Complaint added successful!");
                                return View(new NewComplaint());

                            }
                            catch (Exception er)
                            {
                                cmd.Dispose();
                                ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), er.Message);
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), "Complaint title already exist!");
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), "All fields with * are required!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = _help.getMsg(AlertType.danger.ToString(), ex.Message);
                return View(model);
            }
        }


        public ActionResult AddComplaint()
        {
            ViewBag.ComplaintType = _complaintType.GetAll();
            return PartialView(new Complaint());
        }

        [HttpPost]
        public int GetCode(int ctID)
        {
            return _complaintType.GetRecordById(ctID).Code;
        }

        [HttpPost]
        public ActionResult AddComplaint(Complaint model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Title.Trim().Contains(" "))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Empty space is not allowed in Title, you can replace empty space with character like - or _" });
                    }
                    if (!string.IsNullOrEmpty(model.ComplaintOwnerEmail))
                    {
                        if (!_help.IsValidEmail(model.ComplaintOwnerEmail))
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid email address!" });
                        }
                    }
                    if (!_help.IsValidPhone(model.MobileNo))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid phone number!" });
                    }
                    var compType = _complaintType.GetRecordById(model.ComplaintTypeId);
                    model.Title = compType.Name.Replace(" ","_") + "_" + model.Title + "_" + model.MobileNo;
                    var cmp = _complaint.GetComplaintByTitle(model.Title);
                    if(cmp == null)
                    {
                        var user = _user.GetUser(User.Identity.Name);
                        model.Date = DateTime.Now;
                        model.RegisteredBy = user.UserId;
                        _complaint.Create(model);
                        TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Added successfully!");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Complaint title already exist!" });
                    }
                }
                else
                {
                    //TempData["Msg"] = _help.getMsg(AlertType.warning.ToString(), "Please fill the form correctly!");
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }

        }

        public ActionResult Details(int id)
        {
            var dt = _vComplaint.GetComplaint(id);
            //ViewBag.ComplaintActivity = _vComplaintActivity.GetSolutionsByComplaintId(id);
            return PartialView(dt);
        }

        public ActionResult AssignTo(int id)
        {
            ViewBag.SupportUsers = _user.GetAllSupportUsers();//.GetAll();//.GetAllSupportUser();
            ViewBag.currentUser = 0;
            var dt = _vComplaint.GetComplaint(id);
            //ViewBag.ComplaintActivity = _vComplaintActivity.GetSolutionsByComplaintId(id);
            return PartialView(dt);
        }

        [HttpPost]

        public ActionResult AssignTo(Complaint model)
        {
            //var dt = _vComplaint.GetComplaint(model.ComplaintId);

            ////ViewBag.ComplaintActivity = _vComplaintActivity.GetSolutionsByComplaintId(id);
            //return PartialView(dt);
            try
            {
                if (model.ResolvedBy!=null)
                {
                    _complaint.AssignSupport(model);
                    TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Support User added successfully!");
                    return RedirectToAction("Index");
                }
                else
                {
                    return Json(new
                        {IsAuthenticated = true, IsSuccessful = false, Error = "Please assign Support User!"});
                }
            }
            catch (Exception ex)
            {
                return Json(new {IsAuthenticated = true, IsSuccessful = false, Error = ex.Message});
            }
        }

        public ActionResult ResolutionDetails(int id)
        {
            var ComplaintActivity = _vComplaintActivity.GetSolutionsByComplaintId(id);
            return PartialView(ComplaintActivity);
        }

        public ActionResult AddSolution(int id)
        {
            var obj = new ComplaintActivity();
            obj.ComplaintId = id;
            //ViewBag.SolutionType = _slnType.GetAll();
            
            ViewBag.SolutionStatus = _slnStatus.GetAll();
            ViewBag.Complaint = _vComplaint.GetComplaint(id);
            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult AddSolution(ComplaintActivity model)
        {
            using (var cmd = new TransactionScope())
            {
                try
                {
                   // model.SolutionTypeId = 2;
                    if (ModelState.IsValid)
                    {
                        var user = _user.GetUser(User.Identity.Name);
                        model.Date = DateTime.Now;
                        model.RecordedBy = user.UserId;
                        _complaintActivity.Create(model);
                        int statusId;
                        //var compObj = _vComplaint.GetComplaint(model.ComplaintId);
                        var soln = _slnStatus.GetRecordById(model.SolutionStatusId);
                        if(soln.Name.ToUpper() == "RESOLVED FULLY")
                        {
                            statusId = 4;
                        }
                        else
                        {
                            if (soln.Name.ToUpper() == "REQUIRED TECHNICAL SKILL")
                            {
                                statusId = 3;
                            }
                            else
                            {
                                statusId = 2;
                            }
                                
                        }
                        _complaint.UpdateStatus(model.SolutionStatusId, model.ComplaintId,statusId);
                        cmd.Complete();
                        TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Resolution added successfully!");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        cmd.Dispose();
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
                    }
                }
                catch (Exception ex)
                {
                    cmd.Dispose();
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
                }
            }
        }

        public ActionResult Edit(int id)
        {
            var obj = _vComplaint.GetComplaint(id);
            ViewBag.ComplaintType = _complaintType.GetAll();
            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult Edit(Complaint model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _complaint.Update(model);
                    TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
                    return RedirectToAction("Index");
                }
                else
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult Suspend(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_complaint.SuspendComplaint(id))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = true });
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "An error accurred while suspending this record!" });
                    }
                }
                else
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "This record does not exist!" });

            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult Close(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_complaint.CloseComplaint(id))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = true });
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "An error accurred while closing this record!" });
                    }
                }
                else
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "This record does not exist!" });

            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }

        }
    }
}
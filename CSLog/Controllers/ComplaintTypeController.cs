using CSLog.Domain.Entities;
using CSLog.Domain.Repositories;
using CSLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSLog.Content
{
    [Authorize(Roles = "Admin,User,Support")]
    public class ComplaintTypeController : Controller
    {
        private readonly ComplaintTypeRepository _complaintType;
        private readonly Helper _help;

        public ComplaintTypeController()
        {
            _complaintType = new ComplaintTypeRepository();
            _help = new Helper();
        }

        // GET: ComplaintType
        public ActionResult Index()
        {
            var dt = _complaintType.GeActiveAndNonActive();
            ViewBag.Msg = TempData["Msg"];
            return View(dt);
        }

        public ActionResult Add()
        {
            return PartialView(new ComplaintType());
        }

        //[HttpPost]
        //public JsonResult Add(ComplaintType model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _complaintType.Create(model);
        //            return Json(new { IsAuthenticated = true, IsSuccessful = true });
        //        }
        //        else
        //        {
        //            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please enter Complaint Type!" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
        //    }

        //}

        [HttpPost]
        public ActionResult Add(ComplaintType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Code > 0)
                    {
                        var chk = _complaintType.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
                        if (chk == null)
                        {
                            _complaintType.Create(model);
                            TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Added successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "The record already exist. Please check the records and try again!" });
                        }
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Code!" });
                    }
                    
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

        public ActionResult Edit(int id)
        {
            var obj = _complaintType.GetRecordById(id);
            return PartialView(obj);
        }

        //[HttpPost]
        //public JsonResult Edit(ComplaintType model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _complaintType.UpdateComplaintType(model);
        //            return Json(new { IsAuthenticated = true, IsSuccessful = true });
        //        }
        //        else
        //        {
        //            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
        //    }

        //}

        [HttpPost]
        public ActionResult Edit(ComplaintType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Code > 0)
                    {
                        var chk = _complaintType.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
                        if (chk == null)
                        {
                            _complaintType.UpdateComplaintType(model);
                            TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (chk.ComplaintTypeId == model.ComplaintTypeId)
                            {
                                _complaintType.UpdateComplaintType(model);
                                TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "A record with the same name or code already exist. Please check the records and try again!" });
                            }

                        }
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Code!" });
                    }
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
        public JsonResult Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_complaintType.DeleteComplaintType(id))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = true });
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "An error accurred while deleting this record!" });
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
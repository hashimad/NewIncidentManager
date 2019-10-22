using CSLog.Domain.Entities;
using CSLog.Domain.Repositories;
using CSLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSLog.Controllers
{
    [Authorize(Roles = "Admin,User,Support")]
    public class SolutionStatusController : Controller
    {
        private readonly SolutionStatusRepository _slnStatus;
        private readonly Helper _help;

        public SolutionStatusController()
        {
            _slnStatus = new SolutionStatusRepository();
            _help = new Helper();
        }

        // GET: SolutionStatus
        public ActionResult Index()
        {
            var dt = _slnStatus.GeActiveAndNonActive();
            ViewBag.Msg = TempData["Msg"];
            return View(dt);
        }


        public ActionResult Add()
        {
            return PartialView(new SolutionStatus());
        }

        //[HttpPost]
        //public JsonResult Add(SolutionStatus model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _slnStatus.Create(model);
        //            return Json(new { IsAuthenticated = true, IsSuccessful = true });
        //        }
        //        else
        //        {
        //            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please enter Solution Status!" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
        //    }

        //}

        [HttpPost]
        public ActionResult Add(SolutionStatus model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.Code > 0)
                    {
                        var chk = _slnStatus.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
                        if (chk == null)
                        {
                            _slnStatus.Create(model);
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
            var obj = _slnStatus.GetRecordById(id);
            return PartialView(obj);
        }

        //[HttpPost]
        //public JsonResult Edit(SolutionStatus model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _slnStatus.UpdateSolutionStatus(model);
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
        public ActionResult Edit(SolutionStatus model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Code> 0)
                    {
                        var chk = _slnStatus.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
                        if (chk == null)
                        {
                            _slnStatus.UpdateSolutionStatus(model);
                            TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (chk.SolutionStatusId == model.SolutionStatusId)
                            {
                                _slnStatus.UpdateSolutionStatus(model);
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
                    if (_slnStatus.DeleteSolutionStatus(id))
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
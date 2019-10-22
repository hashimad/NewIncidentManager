//using CSLog.Domain.Entities;
//using CSLog.Domain.Repositories;
//using CSLog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace CSLog.Controllers
//{
//    [Authorize(Roles = "Admin,User,Support")]
//    public class SolutionTypeController : Controller
//    {
//        private readonly SolutionTypeRepository _slnType;
//        private readonly Helper _help;

//        public SolutionTypeController()
//        {
//            _slnType = new SolutionTypeRepository();
//            _help = new Helper();
//        }

//        // GET: SolutionType
//        //[OutputCache(Duration = 60)]
//        public ActionResult Index()
//        {
//            var dt = _slnType.GeActiveAndNonActive();
//            ViewBag.Msg = TempData["Msg"];
//            return View(dt);
//        }

//        public ActionResult Add()
//        {
//            return PartialView(new SolutionType());
//        }
        

//        [HttpPost]
//        public ActionResult Add(SolutionType model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (model.Code > 0)
//                    {
//                        var chk = _slnType.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
//                        if (chk == null)
//                        {
//                            _slnType.Create(model);
//                            TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Added successfully!");
//                            return RedirectToAction("Index");
//                        }
//                        else
//                        {
//                            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "The record already exist. Please check the records and try again!" });
//                        }
//                    }
//                    else
//                    {
//                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Code!" });
//                    }
//                }
//                else
//                {
//                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
//                        //TempData["Msg"] = _help.getMsg(AlertType.warning.ToString(), "Please fill the form correctly!");
//                }
//            }
//            catch (Exception ex)
//            {
//                //TempData["Msg"] = _help.getMsg(AlertType.warning.ToString(), ex.Message);
//                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
//            }
            

//        }

//        public ActionResult Edit(int id)
//        {
//            var obj = _slnType.GetRecordById(id);
//            return PartialView(obj);
//        }

//        //[HttpPost]
//        //public JsonResult Edit(SolutionType model)
//        //{
//        //    try
//        //    {
//        //        if (ModelState.IsValid)
//        //        {
//        //            _slnType.UpdateSolutionType(model);
//        //            return Json(new { IsAuthenticated = true, IsSuccessful = true });
//        //        }
//        //        else
//        //        {
//        //            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
//        //    }

//        //}

//        [HttpPost]
//        public ActionResult Edit(SolutionType model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (model.Code > 0)
//                    {
//                        var chk = _slnType.GetAll().Where(c => c.Name.ToUpper() == model.Name.ToUpper() || c.Code == model.Code).FirstOrDefault();
//                        if (chk == null)
//                        {
//                            _slnType.UpdateSolutionType(model);
//                            TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
//                            return RedirectToAction("Index");
//                        }
//                        else
//                        {
//                            if (chk.SolutionTypeId == model.SolutionTypeId)
//                            {
//                                _slnType.UpdateSolutionType(model);
//                                TempData["Msg"] = _help.getMsg(AlertType.success.ToString(), "Updated successfully!");
//                                return RedirectToAction("Index");
//                            }
//                            else
//                            {
//                                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "A record with the same name or code already exist. Please check the records and try again!" });
//                            }
//                        }
//                    }
//                    else
//                    {
//                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Code!" });
//                    }
                    
//                }
//                else
//                {
//                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please fill the form correctly!" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
//            }
//        }

//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            try
//            {
//                if (id > 0)
//                {
//                    if (_slnType.DeleteSolutionType(id))
//                    {
//                        return Json(new { IsAuthenticated = true, IsSuccessful = true });
//                    }
//                    else
//                    {
//                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "An error accurred while deleting this record!" });
//                    }
//                }
//                else
//                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "This record does not exist!" });

//            }
//            catch (Exception ex)
//            {
//                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
//            }

//        }
//    }
//}
using CSLog.Domain.Repositories;
using CSLog.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSLog.Controllers
{
    [Authorize(Roles = "Admin,User,Support")]
    public class DashboardController : Controller
    {
        private readonly vComplaintRepository _vComplaint;
        private readonly Helper _app;
        private readonly ComplaintTypeRepository _complaintType;
        private readonly ASPNetUserRepository _user;

        public DashboardController()
        {
            _vComplaint = new vComplaintRepository();
            _complaintType = new ComplaintTypeRepository();
            _user = new ASPNetUserRepository();
            _app = new Helper();
        }
        // GET: Dashboard
        public ActionResult Index()
        {

            ViewBag.sDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.eDate = DateTime.Now.ToString("dd/MM/yyyy");
            var dt = _vComplaint.GetTodayComplaint();
            DashboardViewModel obj = new DashboardViewModel();
            obj.TotalUnresolved = dt.Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList().Count();
            obj.TotalClosed = dt.Where(c => c.ComplaintStatus.ToUpper() == "CLOSED").ToList().Count();
            obj.TotalComplaint = dt.Count();
            obj.TotalResolved = dt.Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList().Count();
            var ct = GetChartData(DateTime.Now, DateTime.Now);
            ViewBag.Chart = _app.GetChart(ChartTypes.Column, "Complaints Chart", ct.data.ToArray(), ct.series.ToArray(), Color.White, Color.White, $"Complaints from {DateTime.Now.ToString("MMM dd, yyyy")} To {DateTime.Now.ToString("MMM dd, yyyy")}", "Complaint Code","No.of Complaints");
            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(DateTime sDate, DateTime eDate)
        {
            ViewBag.sDate = sDate.ToString("dd/MM/yyyy");
            ViewBag.eDate = eDate.ToString("dd/MM/yyyy");
            var dt = _vComplaint.FilterComplaint(sDate, eDate);
            DashboardViewModel obj = new DashboardViewModel();
            obj.TotalUnresolved = dt.Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList().Count();
            obj.TotalClosed = dt.Where(c => c.ComplaintStatus.ToUpper() == "CLOSED").ToList().Count();
            obj.TotalComplaint = dt.Count();
            obj.TotalResolved = dt.Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList().Count();
            var ct = GetChartData(sDate, eDate);
            ViewBag.Chart = _app.GetChart(ChartTypes.Column,  "Complaints Chart", ct.data.ToArray(), ct.series.ToArray(), Color.White, Color.White, $"Complaints from {sDate.ToString("MMM dd, yyyy")} To {eDate.ToString("MMM dd, yyyy")}", "Complaint Code", "No.of Complaints");
            return View(obj);
        }


        public ActionResult Agent()
        {
            ViewBag.sDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.eDate = DateTime.Now.ToString("dd/MM/yyyy");
            var agents = _user.GetAll();
            ViewBag.Agents = agents;
            if (agents.Count() > 0)
            {
                ViewBag.currentAgent = agents[0].UserId;
                ViewBag.currentAgentName = agents[0].DisplayName;
            } 
            else
            {
                ViewBag.currentAgent = 0;
                ViewBag.currentAgentName = "Agent Dashbord";
            }

            var dt = _vComplaint.FilterComplaint(DateTime.Now.Date, DateTime.Now.Date, 0, 0, 0, agents[0].UserId);
            ViewBag.dts = dt;
            DashboardViewModel obj = new DashboardViewModel();
            obj.TotalUnresolved = dt.Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList().Count();
            obj.TotalClosed = dt.Where(c => c.ComplaintStatus.ToUpper() == "CLOSED").ToList().Count();
            obj.TotalComplaint = dt.Count();
            obj.TotalResolved = dt.Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList().Count();
            var ct = GetAgentChartData(DateTime.Now, DateTime.Now, (int)ViewBag.currentAgent);
            if (ct.series.Count() > 0)
            {
                ViewBag.Chart = _app.GetChart(ChartTypes.Column, "Complaints Chart", ct.data.ToArray(), ct.series.ToArray(), Color.White, Color.White, $"Complaints from {DateTime.Now.ToString("MMM dd, yyyy")} To {DateTime.Now.ToString("MMM dd, yyyy")}", "Complaint Code", "No.of Complaints");
            }
            else
            {
                ViewBag.Chart = new Highcharts("columnchart");
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Agent(DateTime sDate, DateTime eDate, int agent)
        {
            ViewBag.sDate = sDate.ToString("dd/MM/yyyy");
            ViewBag.eDate = eDate.ToString("dd/MM/yyyy");
            var agents = _user.GetAll();
            ViewBag.Agents = agents;
            ViewBag.currentAgent = agent;

            if (agent > 0)
            {
                ViewBag.currentAgentName = agents.Where(c => c.UserId == agent).Select(a => a.DisplayName).FirstOrDefault();
            }
            else
            {
                ViewBag.currentAgentName = "Agent Dashbord";
            }

            var dt = _vComplaint.FilterComplaint(sDate, eDate, 0, 0, 0, agent);
            ViewBag.dts = dt;
            DashboardViewModel obj = new DashboardViewModel();
            obj.TotalUnresolved = dt.Where(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY").ToList().Count();
            obj.TotalClosed = dt.Where(c => c.ComplaintStatus.ToUpper() == "CLOSED").ToList().Count();
            obj.TotalComplaint = dt.Count();
            obj.TotalResolved = dt.Where(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY").ToList().Count();
            var ct = GetAgentChartData(sDate, eDate,agent);
            if (ct.series.Count() > 0)
            {
                ViewBag.Chart = _app.GetChart(ChartTypes.Column, "Complaints Chart", ct.data.ToArray(), ct.series.ToArray(), Color.White, Color.White, $"Complaints from {sDate.ToString("MMM dd, yyyy")} To {eDate.ToString("MMM dd, yyyy")}", "Complaint Code", "No.of Complaints");
            }
            else
            {
                ViewBag.Chart = new Highcharts("columnchart");
            }
            
            return View(obj);
        }



        private ChartModel GetChartData(DateTime startDate, DateTime endDate)
        {
            ChartModel chart = new ChartModel();
            List<string> complaintTypes = new List<string>();
            List<Series> series = new List<Series>();
            List<object> sdata = new List<object>();
            List<object> ddata = new List<object>();
            var cts = _complaintType.GetAll();
            foreach (var item in cts)
            {
                complaintTypes.Add(item.Code.ToString());
                var complaints = _vComplaint.FilterComplaint(startDate, endDate).Where(c => c.ComplaintType.ToUpper() == item.Name.ToUpper()).ToList();
                if (complaints.Count > 0)
                {
                    var sdt = complaints.Count(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY");
                    sdata.Add(sdt);
                    var ddt = complaints.Count(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY");
                    ddata.Add(ddt);
                }
                else
                {
                    sdata.Add(0);
                    ddata.Add(0);
                }
            }
            Series s = new Series { Name = "Unresolved", Data = new Data(sdata.ToArray()) };
            Series d = new Series { Name = "Resolved Fully", Data = new Data(ddata.ToArray()) };
            series.Add(s);
            series.Add(d);
            chart.data = complaintTypes;
            chart.series = series;
            return chart;
        }

        private ChartModel GetAgentChartData(DateTime startDate, DateTime endDate, int agent)
        {
            ChartModel chart = new ChartModel();
            List<string> complaintTypes = new List<string>();
            List<Series> series = new List<Series>();
            List<object> sdata = new List<object>();
            List<object> ddata = new List<object>();
            var cts = _complaintType.GetAll();
            foreach (var item in cts)
            {
                complaintTypes.Add(item.Code.ToString());
                var complaints = _vComplaint.FilterComplaint(startDate, endDate, 0, 0, 0, agent).Where(c => c.ComplaintType.ToUpper() == item.Name.ToUpper()).ToList();
                if (complaints.Count > 0)
                {
                    var sdt = complaints.Count(c => c.SolutionStatus.ToUpper() != "RESOLVED FULLY");
                    sdata.Add(sdt);
                    var ddt = complaints.Count(c => c.SolutionStatus.ToUpper() == "RESOLVED FULLY");
                    ddata.Add(ddt);
                }
                else
                {
                    sdata.Add(0);
                    ddata.Add(0);
                }
            }
            Series s = new Series { Name = "Unresolved", Data = new Data(sdata.ToArray()) };
            Series d = new Series { Name = "Resolved Fully", Data = new Data(ddata.ToArray()) };
            series.Add(s);
            series.Add(d);
            chart.data = complaintTypes;
            chart.series = series;
            return chart;
        }

    }
}
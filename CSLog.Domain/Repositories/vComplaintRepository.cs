using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class vComplaintRepository
    {
        private readonly EFDbContext _context;

        public vComplaintRepository()
        {
            _context = new EFDbContext();
        }

        public List<vComplaint> GetAll()
        {
            return _context.vComplaints.OrderByDescending(a=>a.ComplaintId).ToList();
        }
        
        public List<vComplaint> GetTodayComplaint(int userId = 0)
        {
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow;
            if(userId > 0)
            {
                return _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate) && c.RegisteredBy == userId).OrderByDescending(a => a.ComplaintId).ToList();
            }
            else
            {
                return _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate)).OrderByDescending(a => a.ComplaintId).ToList();
            }
        }

        public List<vComplaint> GetTodayComplaintBySupportUser(int userId = 0)
        {
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow;
            if (userId > 0)
            {
                return _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate) && c.ResolvedBy == userId).OrderByDescending(a => a.ComplaintId).ToList();
            }
            else
            {
                return _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate)).OrderByDescending(a => a.ComplaintId).ToList();
            }
        }

     
        public List<vComplaint> GetUnResolvedComplaintBySupportUser(int userId = 0)
        {
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow;
            if (userId > 0)
            {
                return _context.vComplaints.Where(c =>c.SolutionStatusId !=2 && c.ResolvedBy == userId).OrderByDescending(a => a.ComplaintId).ToList();
            }
            else
            {
                return _context.vComplaints.Where(c => c.SolutionStatusId != 2).OrderByDescending(a => a.ComplaintId).ToList();
                //  return _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate)).OrderByDescending(a => a.ComplaintId).ToList();
            }
        }

        public List<vComplaint> FilterComplaint(DateTime sDate, DateTime eDate, int complaintType = 0, int complaintStatus = 0, int solnStatus = 0, int agent = 0)
        {
            var dt = _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(sDate)
                 && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(eDate)); //& (c.ComplaintTypeId == complaintTypeId | c.SolutionStatusId == statusId)).ToList();
            if (complaintStatus > 0)
            {
                dt = dt.Where(c => c.StatusId == complaintStatus);
            }
            if (complaintType > 0)
            {
                dt = dt.Where(c => c.ComplaintTypeId == complaintType);
            }
            if (solnStatus > 0)
            {
                dt = dt.Where(c => c.SolutionStatusId == solnStatus);
            }
            if (agent > 0)
            {
                dt = dt.Where(c => c.RegisteredBy == agent);
            }
            return dt.OrderByDescending(a=>a.ComplaintId).ToList();
        }

        public List<vComplaint> FilterComplaintBySupportUser(DateTime sDate, DateTime eDate, int complaintType = 0, int complaintStatus = 0, int solnStatus = 0, int supportUser = 0, int agent = 0)
        {
            var dt = _context.vComplaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(sDate)
                                                     && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(eDate)); //& (c.ComplaintTypeId == complaintTypeId | c.SolutionStatusId == statusId)).ToList();
            if (complaintStatus > 0)
            {
                dt = dt.Where(c => c.StatusId == complaintStatus);
            }
            if (complaintType > 0)
            {
                dt = dt.Where(c => c.ComplaintTypeId == complaintType);
            }
            if (solnStatus > 0)
            {
                dt = dt.Where(c => c.SolutionStatusId == solnStatus);
            }
            if (supportUser > 0)
            {
                dt = dt.Where(c => c.ResolvedBy == supportUser);
            }
            if (agent > 0)
            {
                dt = dt.Where(c => c.RegisteredBy == agent);
            }
            return dt.OrderByDescending(a => a.ComplaintId).ToList();
        }

        public vComplaint GetComplaint(int id)
        {
            var complaint = _context.vComplaints.Where(c => c.ComplaintId == id).FirstOrDefault();
            return complaint;
        }
    }
}

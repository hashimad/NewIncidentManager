using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class vComplaintActivityRepository
    {
        private readonly EFDbContext _context;

        public vComplaintActivityRepository()
        {
            _context = new EFDbContext();
        }

        public List<vComplaintActivity> GetSolutionsByComplaintId(int complaintId)
        {
            if (!_context.vComplaintActivities.Select(x=>x).Any())
            {
              return  new List<vComplaintActivity>().ToList();
            }
           // List<vComplaintActivity> solnByComplaintId= _context.vComplaintActivities.Where(c => c.ComplaintId == complaintId).ToList();
            return _context.vComplaintActivities.Select(x => x).ToList().Exists(c => c.ComplaintId == complaintId)
                ? _context.vComplaintActivities.Where(c => c.ComplaintId == complaintId).ToList()
                : new List<vComplaintActivity>().ToList();
        }
    }
}

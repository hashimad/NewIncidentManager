using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class ComplaintRepository
    {
        private readonly EFDbContext _context;

        public ComplaintRepository()
        {
            _context = new EFDbContext();
        }

        public List<Complaint> GetAll()
        {
            return _context.Complaints.ToList();
        }

        public List<Complaint> GetLast7DaysComplaint()
        {
            var startDate = DateTime.UtcNow.AddDays(-6);
            var endDate = DateTime.UtcNow;
            return _context.Complaints.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(endDate)).ToList();
        }

        public List<Complaint> FilterComplaint(DateTime sDate, DateTime eDate, int complaintTypeId, int statusId)
        {
            var dt =_context.Complaints.Where(c => (DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(sDate) 
                && DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(eDate)) & (c.ComplaintTypeId == complaintTypeId | c.SolutionStatusId == statusId)).ToList();
            return dt;
        }

        public void Create(Complaint model)
        {
            try
            {
                _context.Complaints.Add(model);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Complaint model)
        {
            try
            {
                var dt = _context.Complaints.Find(model.ComplaintId);
                if (dt != null)
                {
                    dt.MobileNo = model.MobileNo;
                    dt.ComplaintTypeId = model.ComplaintTypeId;
                    dt.Details = model.Details;
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void AssignSupport(Complaint model)
        {
            try
            {
                var dt = _context.Complaints.Find(model.ComplaintId);
                if (dt != null)
                {
                    //dt.MobileNo = model.MobileNo;
                    dt.ComplaintTypeId = model.ComplaintTypeId;
                    dt.Details = model.Details;
                    dt.ResolvedBy = model.ResolvedBy;
                    dt.ResolvedDate=DateTime.Now;
                    
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void UpdateStatus(int solnStatusId, int id, int statusId)
        {
            try
            {
                var dt = _context.Complaints.Find(id);
                if (dt != null)
                {
                    dt.SolutionStatusId = solnStatusId;
                    dt.StatusId = statusId;
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Complaint GetComplaint(int id)
        {
            var complaint = _context.Complaints.Where(c => c.ComplaintId == id).FirstOrDefault();
            return complaint;
        }
        public Complaint GetComplaintByTitle(string title)
        {
            var complaint = _context.Complaints.Where(c => c.Title == title).FirstOrDefault();
            return complaint;
        }

        public bool SuspendComplaint(int id)
        {
            try
            {
                var dt = _context.Complaints.Find(id);
                if (dt != null)
                {
                    dt.StatusId = 5;
                    dt.SolutionStatusId = 5;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool CloseComplaint(int id)
        {
            try
            {
                var dt = _context.Complaints.Find(id);
                if (dt != null)
                {
                    dt.StatusId = 5;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

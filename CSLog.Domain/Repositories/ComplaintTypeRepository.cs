using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class ComplaintTypeRepository
    {
        private readonly EFDbContext _context;

        public ComplaintTypeRepository()
        {
            _context = new EFDbContext();
        }

        public List<ComplaintType> GetAll()
        {
            return _context.ComplaintTypes.ToList();
        }
        public List<ComplaintType> GeActiveAndNonActive()
        {
            return _context.ComplaintTypes.Where(c => c.Status >= 0).ToList();
        }

        public ComplaintType GetRecordById(int Id)
        {
            return _context.ComplaintTypes.Find(Id);
        }

        public void Create(ComplaintType model)
        {
            try
            {
                _context.ComplaintTypes.Add(model);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void UpdateComplaintType(ComplaintType obj)
        {
            try
            {
                var dt = _context.ComplaintTypes.Find(obj.ComplaintTypeId);
                if (dt != null)
                {
                    dt.Name = obj.Name;
                    dt.Code = obj.Code;
                    dt.Status = obj.Status;
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool DeleteComplaintType(int id)
        {
            try
            {
                var dt = _context.ComplaintTypes.Find(id);
                if (dt != null)
                {
                    dt.Status = -1;
                    //_context.ComplaintTypes.Remove(dt);
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

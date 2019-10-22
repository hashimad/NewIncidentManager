using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class SolutionStatusRepository
    {
        private readonly EFDbContext _context;

        public SolutionStatusRepository()
        {
            _context = new EFDbContext();
        }

        public List<SolutionStatus> GetAll()
        {
            return _context.SolutionStatus.ToList();
        }

        public List<SolutionStatus> GeActiveAndNonActive()
        {
            return _context.SolutionStatus.Where(c => c.Status >= 0).ToList();
        }

        public SolutionStatus GetRecordById(int Id)
        {
            return _context.SolutionStatus.Find(Id);
        }
        public List<SolutionStatus> GetActive()
        {
            return _context.SolutionStatus.Where(c => c.Status > 0).ToList();
        }
        public void Create(SolutionStatus model)
        {
            try
            {
                _context.SolutionStatus.Add(model);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void UpdateSolutionStatus(SolutionStatus obj)
        {
            try
            {
                var dt = _context.SolutionStatus.Find(obj.SolutionStatusId);
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

        public bool DeleteSolutionStatus(int id)
        {
            try
            {
                var dt = _context.SolutionStatus.Find(id);
                if (dt != null)
                {
                    dt.Status = -1;
                    //_context.SolutionStatus.Remove(dt);
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

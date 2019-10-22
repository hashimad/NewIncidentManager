//using CSLog.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CSLog.Domain.Repositories
//{
//    public class SolutionTypeRepository
//    {
//        private readonly EFDbContext _context;

//        public SolutionTypeRepository()
//        {
//            _context = new EFDbContext();
//        }

//        public List<SolutionType> GetAll()
//        {
//            return _context.SolutionTypes.ToList();
//        }

//        public List<SolutionType> GeActiveAndNonActive()
//        {
//            return _context.SolutionTypes.Where(c => c.Status >= 0).ToList();
//        }

//        public SolutionType GetRecordById(int Id)
//        {
//            return _context.SolutionTypes.Find(Id);
//        }
//        public void Create(SolutionType model)
//        {
//            try
//            {
//                _context.SolutionTypes.Add(model);
//                _context.SaveChanges();
//            }
//            catch (Exception e)
//            {
//                throw;
//            }
//        }

//        public void UpdateSolutionType(SolutionType obj)
//        {
//            try
//            {
//                var dt = _context.SolutionTypes.Find(obj.SolutionTypeId);
//                if (dt != null)
//                {
//                    dt.Name = obj.Name;
//                    dt.Code = obj.Code;
//                    dt.Status = obj.Status;
//                    _context.SaveChanges();
//                }

//            }
//            catch (Exception e)
//            {
//                throw;
//            }
//        }

//        public bool DeleteSolutionType(int id)
//        {
//            try
//            {
//                var dt = _context.SolutionTypes.Find(id);
//                if (dt != null)
//                {
//                    dt.Status = -1;
//                    //_context.SolutionTypes.Remove(dt);
//                    _context.SaveChanges();
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception e)
//            {
//                throw;
//            }
//        }
//    }
//}

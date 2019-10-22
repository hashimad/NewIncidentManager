using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class ComplaintActivityRepository
    {
        private readonly EFDbContext _context;

        public ComplaintActivityRepository()
        {
            _context = new EFDbContext();
        }

        public List<ComplaintActivity> GetSolutionsByComplaintId(int complaintId)
        {
            return _context.ComplaintActivities.Where(c => c.ComplaintId == complaintId).ToList();
        }

        public void Create(ComplaintActivity model)
        {
            try
            {
                _context.ComplaintActivities.Add(model);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Core.UpdateException e)
            {

            }

            catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
            {
                Console.WriteLine(ex.InnerException);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                throw;
            }
            //    catch (Exception e)
            //    {
            //        throw;
            //    }
            }
        }
}

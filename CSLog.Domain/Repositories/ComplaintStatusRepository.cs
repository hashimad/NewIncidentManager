using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class ComplaintStatusRepository
    {
        private readonly EFDbContext _context;

        public ComplaintStatusRepository()
        {
            _context = new EFDbContext();
        }

        public List<ComplaintStatus> GetAll()
        {
            return _context.ComplaintStatus.ToList();
        }

        public ComplaintStatus GetRecordById(int Id)
        {
            return _context.ComplaintStatus.Find(Id);
        }
    }
}

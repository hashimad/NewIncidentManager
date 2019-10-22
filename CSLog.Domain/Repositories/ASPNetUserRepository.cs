using CSLog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLog.Domain.Repositories
{
    public class ASPNetUserRepository
    {
        private readonly EFDbContext _context;

        public ASPNetUserRepository()
        {
            _context = new EFDbContext();
        }

        public List<AspNetUser> GetAll()
        {
            return _context.AspNetUsers.ToList();
        }
        public List<AspNetUser> GetActiveUsers()
        {
            return _context.AspNetUsers.Where(c=>c.Status).ToList();
        }

        public AspNetUser GetUser(string username)
        {
            return _context.AspNetUsers.Where(c => c.Email == username).FirstOrDefault();
        }
        public List<AspNetUser> GetAllSupportUsers()
        {
            
            return _context.AspNetUsers.Where(u => u.AspNetRoles.Any(r=>r.Name=="Support")).ToList();
        }
        public List<AspNetUser> GetAllComplaintUsers()
        {

            return _context.AspNetUsers.Where(u => u.AspNetRoles.Any(r => r.Name == "User")).ToList();
        }

        public AspNetUser GetUserById(string Id)
        {
            return _context.AspNetUsers.Where(c => c.Id == Id).FirstOrDefault();
        }

        public void Update(string userId, string currentUser)
        {
            try
            {
                var dt = _context.AspNetUsers.Find(userId);
                if (dt != null)
                {
                    //if (currentUser.ToLower() == "admin@xplugng.com")
                    //    dt.MustChangePassword = true;
                    //else
                    //    dt.MustChangePassword = false;

                    dt.MustChangePassword = false;
                    
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
     
        public bool UpdateComplaint(string Id, string displayName,bool userStatus)
        {
            try
            {
                var dt = _context.AspNetUsers.Find(Id);
                if (dt != null)
                {
                    dt.DisplayName = displayName;
                    dt.Status = userStatus;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

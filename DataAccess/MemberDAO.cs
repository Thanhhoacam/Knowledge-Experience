using BusinessObject;
using BusinessObject.Object;

namespace DataAccess
{
    public class MemberDAO
    {
       
        //using singleton pattern
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public Member getMemberByEmail(string email)
        {
            Member member;
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                member = context.Members.FirstOrDefault(m => m.Email == email);
                if (member == null)
                {
                    throw new Exception("No member found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void DeleteMember(Member member)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                var memberToDelete = context.Members.Find(member.MemberId);
                context.Members.Remove(memberToDelete);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public List<Member> GetMembers()
        {
            List<Member> members;
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                members = context.Members.ToList();
                if (members == null)
                {
                    throw new Exception("No members found");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            return members;
        }

        public void InsertMember(Member member)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Members.Add(member);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                using ApplicationDbContext context = new ApplicationDbContext();
                context.Entry<Member>(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AuthenticateMember(string email, string password)
        {
            Member member = null;
            bool isAuthenticated = false;
            try {
                using ApplicationDbContext context = new ApplicationDbContext();

                 member = context.Members.FirstOrDefault(m => m.Email == email && m.Password == password);
                if (member != null)
                {
                    isAuthenticated = true;
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }


            return isAuthenticated;
           
           
        }



    }
}

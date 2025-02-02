using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.RegisterDTO;
using BusinessObject.Object;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IMemberRepository
    {
        //IEnumerable<Member> GetMembers();
        //Member getMemberByEmail(string email);
        //void InsertMember(Member member);
        //void DeleteMember(Member member);
        //void UpdateMember(Member member);
        //bool AuthenticateMember(string email, string password);

        //---------v2 async----------//
        Task<List<Member>> GetAllAsync(Expression<Func<Member, bool>>? filter = null, string? includeProperties = null,
          int pageSize = 0, int pageNumber = 1);
        Task<Member> GetAsync(Expression<Func<Member, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(Member entity);
        Task RemoveAsync(Member entity);
        Task<Member> UpdateAsync(Member entity);
        Task<Member> Register(RegisterationRequestDTO entity);
        Task<LoginResponseDTO> Login(LoginRequestDTO entity);
        bool IsUniqueUser(string email);
    }
}

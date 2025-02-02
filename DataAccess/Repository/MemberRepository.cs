using BusinessObject.models.Dto.LoginDTO;
using BusinessObject.models.Dto.RegisterDTO;
using BusinessObject.Object;
using DataAccess.Dao;
using DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDAOv2 memberDAOv2;

        public MemberRepository(MemberDAOv2 memberDAOv2)
        {
            this.memberDAOv2 = memberDAOv2;
        }

        //---------v2 DAOv2----------//
        public async Task CreateAsync(Member entity)
        {
            await memberDAOv2.CreateAsync(entity);
        }


        public async Task<List<Member>> GetAllAsync(Expression<Func<Member, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            return await memberDAOv2.GetAllAsync(filter: filter, pageSize: pageSize, pageNumber: pageNumber, includeProperties: includeProperties);
        }


        public async Task RemoveAsync(Member entity)
        {
            await memberDAOv2.RemoveAsync(entity);
        }

        public async Task UpdateAsync(Member entity)
        {
            await memberDAOv2.UpdateAsync(entity);
        }

        public async Task<Member> GetAsync(Expression<Func<Member, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            return await memberDAOv2.GetAsync(filter, tracked, includeProperties);
        }

        async Task<Member> IMemberRepository.UpdateAsync(Member entity)
        {
            return await memberDAOv2.UpdateAsync(entity);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO entity)
        {
            return await memberDAOv2.Login(entity);
        }

        public async Task<Member> Register(RegisterationRequestDTO entity)
        {
            return await memberDAOv2.Register(entity);
        }

        public bool IsUniqueUser(string email)
        {
            return memberDAOv2.IsUniqueUser(email);
        }

        //public Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

using UserSignUp.Data;

namespace UserSignUp.Interfaces
{
    public interface IDbInitializer
    {
        void Initialize(SqlDbContext context);
    }
}

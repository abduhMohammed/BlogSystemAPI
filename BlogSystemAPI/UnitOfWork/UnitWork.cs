using BlogSystemAPI.Models;
using BlogSystemAPI.Repository;

namespace BlogSystemAPI.UnitOfWork
{
    public class UnitWork
    {
        AppDbContext db;
        GenaricRepository<BlogPost> PostRepo;

        public UnitWork(AppDbContext db)
        {
            this.db = db;
        }

        public GenaricRepository<BlogPost> PostRepository
        {
            get
            {
                if (PostRepo == null)
                {
                    PostRepo = new GenaricRepository<BlogPost>(db);
                }
                return PostRepo;
            }
        }
    }
}
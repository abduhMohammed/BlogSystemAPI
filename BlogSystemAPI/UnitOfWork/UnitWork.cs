using BlogSystemAPI.Models;
using BlogSystemAPI.Repository;

namespace BlogSystemAPI.UnitOfWork
{
    public class UnitWork
    {
        AppDbContext db;
        GenaricRepository<BlogPost> PostRepo;
        GenaricRepository<Category> CategRopo;
        GenaricRepository<Comment> CommRepo;

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

        public GenaricRepository<Category> CategoryRepository
        {
            get
            {
                if (CategRopo == null)
                {
                    CategRopo = new GenaricRepository<Category>(db);
                }
                return CategRopo;
            }
        }

        public GenaricRepository<Comment> CommentRepository
        {
            get
            {
                if (CommRepo == null)
                {
                    CommRepo = new GenaricRepository<Comment>(db);
                }
                return CommRepo;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
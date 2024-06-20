namespace Boren.MiniAPI.WebApplication.Data.Service
{
    public class StudentService : IStudentService
    {
        private readonly TodoDb _dbContext;
        public StudentService(TodoDb dbContext)
        {
            _dbContext = dbContext;
        }

        public string Create(Student student)
        {
            return "Create Success";
        }

        public string ReadAll()
        {
            return "So many students";
        }

        public string Read(int id)
        {
            return "Amy";
        }
    }
}

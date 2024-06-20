namespace Boren.MiniAPI.WebApplication.Data
{
    public interface IStudentService
    {
        public string Create(Student student);

        public string ReadAll();

        public string Read(int id);
    }
}

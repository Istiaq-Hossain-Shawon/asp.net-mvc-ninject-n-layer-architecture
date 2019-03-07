using System.Data;

namespace MVC.DAL
{
    public class Result
    {
        public bool ExecutionState { get; set; }
        public string Error { get; set; }
        public DataTable Data;
    }
}

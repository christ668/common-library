using Microsoft.AspNetCore.Mvc;

namespace common_library.Base.Model
{
    public class CustomProblem : ProblemDetails
    {
        public IDictionary<string, string[]> Errors = new Dictionary<string, string[]>();
    }
}

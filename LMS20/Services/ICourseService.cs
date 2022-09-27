
using System.Security.Claims;

namespace lms20.web.services
{
    public interface ICourseService
    {
        Task<string> getCourseName(ClaimsPrincipal User);
        Task<string> getCourseId(ClaimsPrincipal User);
    }
}
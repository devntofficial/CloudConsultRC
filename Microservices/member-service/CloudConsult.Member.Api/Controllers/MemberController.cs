using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Member.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class MemberController : ControllerBase
    {

        [HttpGet(Routes.Member.GetMemberById)]
        public IActionResult GetMemberById([FromRoute] string MemberId)
        {
            throw new NotImplementedException();
        }
    }
}

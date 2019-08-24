using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storytel.Repository.Interface;

namespace Storytel.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MessageController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public MessageController(IRepositoryWrapper repoWrapper) => _repoWrapper = repoWrapper;

    }
}
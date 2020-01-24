using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace APITest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly APITestContext _apiTestContext;

        public ProdutoController(APITestContext apiTestContext)
        {
            _apiTestContext = apiTestContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
        {
            return await _apiTestContext.Produto.ToListAsync();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(long id)
        {
            var produto = await _apiTestContext.Produto.FindAsync(id);

            // https://www.youtube.com/watch?v=ccVmPgxNE6c ---- 38:23
            return produto != null ? Ok(produto) : NotFound("Produto não encontrado");
        }
    }
}
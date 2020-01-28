using Domain.DTO;
using Domain.RDTO;
using Utils.Authorize;
using Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("getAll")]
        [ClaimsAuthorize("Produto", "Consultar")]
        public ActionResult<Produto> GetAllProduto()
        {
            return Ok(_produtoRepository.GetAll());
        }

        [HttpGet("get/{id}")]
        [ClaimsAuthorize("Produto", "Consultar")]
        public ActionResult<Produto> GetProduto(long id)
        {
            var produto = _produtoRepository.GetbyId(id);

            if (produto != null)
            {
                return Ok(produto);
            }
            else
            {
                return NotFound("Produto não encontrado");
            }
        }

        [HttpPost]
        [ClaimsAuthorize("Produto", "Incluir")]
        public ActionResult<Produto> Create(RequestProduto rProduto)
        {
            decimal valor = 0;
            decimal.TryParse(rProduto.Valor, out valor);

            Produto produto = new Produto
            {
                Nome = rProduto.Nome,
                Valor = valor
            };

            _produtoRepository.Incluid(produto);

            return Ok("Produto incluído com sucesso");
        }

        [HttpPut("edit/{id}")]
        [ClaimsAuthorize("Produto", "Editar")]
        public IActionResult Edit(long id)
        {
            var produto = _produtoRepository.GetbyId(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            try
            {
                _produtoRepository.Update(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("Produto não encontrado");
            }

            return Ok("Produto modificado com sucesso");
        }

        [HttpDelete("delete/{id}")]
        [ClaimsAuthorize("Produto", "Excluir")]
        public ActionResult<Produto> Delete(long id)
        {
            var produto = _produtoRepository.GetbyId(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            _produtoRepository.Remove(produto);

            return produto;
        }
    }
}
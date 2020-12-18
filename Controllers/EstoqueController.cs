using EVendas.Mapper;
using EVendas.Service.Interfaces;
using EVendas.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EVendasEstoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogger<EstoqueController> _logger;
        public EstoqueController(IProdutoService produtoService, ILogger<EstoqueController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AdicionarProduto")]        
        public IActionResult AdicionarProduto([FromBody] ProdutoResponse produtoResponse)
        {
            try
            {
                var produto = _produtoService.AdicionarProduto(ProdutoMapper.ToEntity(produtoResponse));
                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("EditarProduto")]        
        public IActionResult EditarProduto([FromBody] ProdutoResponse produtoResponse)
        {
            try
            {
                _produtoService.EditarProduto(ProdutoMapper.ToEntity(produtoResponse));
                return Ok(produtoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("ListarProdutos")]        
        public IActionResult ListarProdutos()
        {
            try
            {
                IEnumerable<ProdutoResponse> produtos = _produtoService.ListarProdutos().Select(x => x.ToProdutoResponse());
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                return BadRequest(new { Error = "Api unavailable" });
            }

        }
    }
}

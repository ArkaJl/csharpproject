using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// контроллер для управления трнзакциями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        /// <summary>
        /// получить все транзакции
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAll();
            return Ok(transactions);
        }
        /// <summary>
        /// получить транзакцию по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound($"Transaction with id {id} not found");
            }
            return Ok(transaction);
        }
        /// <summary>
        /// получить транзакции пользователя
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var transactions = await _transactionService.GetByUserId(userId);
            return Ok(transactions);
        }
        /// <summary>
        /// получить транзакции с предметом по id
        /// </summary>

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetByItemId(string itemId)
        {
            var transactions = await _transactionService.GetByItemId(itemId);
            return Ok(transactions);
        }
        /// <summary>
        /// получить транзакции типа
        /// </summary>

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var transactions = await _transactionService.GetByType(type);
            return Ok(transactions);
        }
        /// <summary>
        /// получить транзакции по дате
        /// </summary>

        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _transactionService.GetByDateRange(startDate, endDate);
            return Ok(transactions);
        }
        /// <summary>
        /// получить баланс пользователя
        /// </summary>

        [HttpGet("user/{userId}/balance")]
        public async Task<IActionResult> GetUserBalance(string userId)
        {
            var balance = await _transactionService.GetUserBalance(userId);
            return Ok(new { Balance = balance });
        }
        /// <summary>
        /// создать транзакцию
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            var createdTransaction = await _transactionService.Create(transaction);
            return CreatedAtAction(nameof(GetById), new { id = createdTransaction.Id }, createdTransaction);
        }
        /// <summary>
        /// изменить транзакцию
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Transaction transaction)
        {
            if (id != transaction.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedTransaction = await _transactionService.Update(transaction);
            return Ok(updatedTransaction);
        }
        /// <summary>
        /// удалить транзакцию
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _transactionService.Delete(id);
            return NoContent();
        }
    }
}
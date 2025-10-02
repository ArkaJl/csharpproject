using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Transaction;

namespace BackendApi.Controllers
{
    /// <summary>
    /// контроллер для управления транзакциями
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
            var response = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                ItemId = t.ItemId,
                CreatedAt = t.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить транзакцию по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetById(id.ToString());
            if (transaction == null)
            {
                return NotFound($"Transaction with id {id} not found");
            }

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                ItemId = transaction.ItemId,
                CreatedAt = transaction.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить транзакции пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var transactions = await _transactionService.GetByUserId(userId.ToString());
            var response = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                ItemId = t.ItemId,
                CreatedAt = t.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить транзакции с предметом по id
        /// </summary>
        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetByItemId(Guid itemId)
        {
            var transactions = await _transactionService.GetByItemId(itemId.ToString());
            var response = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                ItemId = t.ItemId,
                CreatedAt = t.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить транзакции по дате
        /// </summary>
        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date");
            }

            // Ограничение на диапазон дат (максимум 1 год)
            if ((endDate - startDate).TotalDays > 365)
            {
                return BadRequest("Date range cannot exceed 1 year");
            }

            var transactions = await _transactionService.GetByDateRange(startDate, endDate);
            var response = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                ItemId = t.ItemId,
                CreatedAt = t.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить баланс пользователя
        /// </summary>
        [HttpGet("user/{userId}/balance")]
        public async Task<IActionResult> GetUserBalance(Guid userId)
        {
            var balance = await _transactionService.GetUserBalance(userId.ToString());
            return Ok(new { Balance = balance });
        }

        /// <summary>
        /// создать транзакцию
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {

            // Валидация суммы
            if (request.Amount == 0)
            {
                return BadRequest("Amount cannot be zero");
            }

            var transaction = new Transaction
            {
                UserId = request.UserId,
                Amount = request.Amount,
                ItemId = request.ItemId,
                CreatedAt = DateTime.UtcNow
            };

            var createdTransaction = await _transactionService.Create(transaction);

            var response = new TransactionResponse
            {
                Id = createdTransaction.Id,
                UserId = createdTransaction.UserId,
                Amount = createdTransaction.Amount,
                ItemId = createdTransaction.ItemId,
                CreatedAt = createdTransaction.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить транзакцию
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateTransactionRequest request)
        {
            var existingTransaction = await _transactionService.GetById(id.ToString());
            if (existingTransaction == null)
            {
                return NotFound($"Transaction with id {id} not found");
            }


            // Валидация суммы
            if (request.Amount != 0)
            {
                existingTransaction.Amount = request.Amount;
            }

            // Обновляем только переданные поля
            if (request.UserId != Guid.Empty)
                existingTransaction.UserId = request.UserId;

            if (request.ItemId.HasValue)
                existingTransaction.ItemId = request.ItemId;

            var updatedTransaction = await _transactionService.Update(existingTransaction);

            var response = new TransactionResponse
            {
                Id = updatedTransaction.Id,
                UserId = updatedTransaction.UserId,
                Amount = updatedTransaction.Amount,
                ItemId = updatedTransaction.ItemId,
                CreatedAt = updatedTransaction.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить транзакцию
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingTransaction = await _transactionService.GetById(id.ToString());
            if (existingTransaction == null)
            {
                return NotFound($"Transaction with id {id} not found");
            }

            await _transactionService.Delete(id.ToString());
            return NoContent();
        }
    }
}
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions.Include(x => x.Category).Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();

            int totalIncome = SelectedTransactions.Where(i => i.Category.Type == "Income").Sum(i => i.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0");

            int totalExpense = SelectedTransactions.Where(i => i.Category.Type == "Expense").Sum(i => i.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("C0");

            int balance =  totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0");

            return View();
        }
    }
}

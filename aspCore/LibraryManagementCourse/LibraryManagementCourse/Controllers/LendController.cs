using LibraryManagementCourse.Data.Interfaces;
using LibraryManagementCourse.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementCourse.Controllers
{
    public class LendController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICustomerRepository _customerRePository;

        public LendController(IBookRepository bookRepository, ICustomerRepository customerRepository)
        {
            _bookRepository = bookRepository;
            _customerRePository = customerRepository;
        }

        public IActionResult List()
        {
            // Load all varieable books
            var availableBooks = _bookRepository.FindWithAuthor(x => x.BorrowerId == 0);

            // Check collection
            if (availableBooks.Count() == 0)
            {
                return View("Empty");
            }
            else
            {
                return View(availableBooks);
            }
        }

        public IActionResult LendBook(int bookId)
        {
            // Load current book and all customers
            var lendVM = new LendViewModel()
            {
                Book = _bookRepository.GetById(bookId),
                Customers = _customerRePository.GetAll()
            };
            // Send data to the lend view
            return View(lendVM);
        }
        [HttpPost]
        public IActionResult LendBook(LendViewModel lendViewModel)
        {
            // Update the database
            var book = _bookRepository.GetById(lendViewModel.Book.BookId);
            var customer = _customerRePository.GetById(lendViewModel.Book.BorrowerId);
            book.Borrower = customer;
            _bookRepository.Update(book);

            // Redirect to the list view
            return RedirectToAction("List");
        }
    }
}

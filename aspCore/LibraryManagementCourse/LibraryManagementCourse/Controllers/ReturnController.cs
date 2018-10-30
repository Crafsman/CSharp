using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementCourse.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementCourse.Controllers
{
    public class ReturnController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICustomerRepository _customerRepository;

        public ReturnController(IBookRepository bookRepository, ICustomerRepository customerRepository)
        {
            _bookRepository = bookRepository;
            _customerRepository = customerRepository;
        }

        public IActionResult List()
        {
            // Load all borrowed books
            var borrowedBooks = _bookRepository.FindWithAuthorAndBorrower(x => x.BorrowerId != 0);
            // Check the books collection 
            if (borrowedBooks == null || borrowedBooks.ToList().Count() == 0)
            {
                return View("Empty");
            }
            return View(borrowedBooks);
        }

        public IActionResult ReturnBook(int bookId)
        {
            // Load the currrent book
            var book = _bookRepository.GetById(bookId);
            // Remove borrower
            book.Borrower = null;

            book.BorrowerId = 0;
            // Update database
            _bookRepository.Update(book);
            // Redirect to list method
            return RedirectToAction("List");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // These static lists keep data available between requests
        // while the application is running. Data resets when the app restarts.
        public static List<Author> Authors = new List<Author>()
        {
            new Author { Id = 1, Name = "Robert C. Martin", Country = "USA" },
            new Author { Id = 2, Name = "James Clear", Country = "USA" }
        };

        public static List<Book> Books = new List<Book>()
        {
            new Book { Id = 1, Title = "Clean Code", Price = 2500, AuthorId = 1 },
            new Book { Id = 2, Title = "Atomic Habits", Price = 3000, AuthorId = 2 },
            new Book { Id = 3, Title = "The Clean Coder", Price = 2200, AuthorId = 1 }
        };

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Books = new List<BookCardViewModel>();
            model.Authors = Authors;

            foreach (var book in Books)
            {
                string authorName = "";

                foreach (var author in Authors)
                {
                    if (book.AuthorId == author.Id)
                    {
                        authorName = author.Name;
                        break;
                    }
                }

                BookCardViewModel card = new BookCardViewModel();
                card.Id = book.Id;
                card.Title = book.Title;
                card.Price = book.Price;
                card.AuthorName = authorName;

                model.Books.Add(card);
            }

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return RedirectToAction("Index");
            }

            bool authorExists = false;

            foreach (var author in Authors)
            {
                if (author.Id == authorId)
                {
                    authorExists = true;
                    break;
                }
            }

            if (!authorExists)
            {
                return RedirectToAction("Index");
            }

            int newId = 1;

            foreach (var book in Books)
            {
                if (book.Id >= newId)
                {
                    newId = book.Id + 1;
                }
            }

            Book newBook = new Book();
            newBook.Id = newId;
            newBook.Title = title;
            newBook.Price = price;
            newBook.AuthorId = authorId;

            Books.Add(newBook);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }

            int newId = 1;

            foreach (var author in Authors)
            {
                if (author.Id >= newId)
                {
                    newId = author.Id + 1;
                }
            }

            Author newAuthor = new Author();
            newAuthor.Id = newId;
            newAuthor.Name = name;
            newAuthor.Country = country;

            Authors.Add(newAuthor);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            Book bookToRemove = null;

            foreach (var book in Books)
            {
                if (book.Id == id)
                {
                    bookToRemove = book;
                    break;
                }
            }

            if (bookToRemove != null)
            {
                Books.Remove(bookToRemove);
            }

            return RedirectToAction("Index");
        }
    }
}

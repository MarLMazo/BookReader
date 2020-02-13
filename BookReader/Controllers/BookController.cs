using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookReader.Data;
using BookReader.Models;
using System.Data.SqlClient;
using BookReader.Models.ViewModels;
using PagedList.Mvc;
using PagedList;

using System.Diagnostics;

namespace BookReader.Controllers
{
    public class BookController : Controller
    {
        private BookReaderContext db = new BookReaderContext();

        // GET: Book
        public ActionResult List(string search, string sortOrder)
        {
            //ViewBag.CurrentSort = sortOrder;
            //List<Book> books = db.Books.SqlQuery("Select * from books").ToList();
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            //return View(books.ToPagedList(pageNumber, pageSize));
            //return View(books);
           
           //Debug.WriteLine(query);
        
            string order;

            switch (sortOrder)
            {
                
                case "publish":
                    order = " ORDER BY BookPublish";
                    break;
                case "author":
                    order = " ORDER BY BookAuthor";
                    break;
                default:
                    order = " ORDER BY BookTitle";
                    break;
            }

            List<Book> books;
            if (!string.IsNullOrEmpty(search))
            {
                //readers = db.Readers.ToList();

                books = db.Books.SqlQuery("Select * from books where BookTitle LIKE '%'+ @search + '%'" + order, new SqlParameter("@search", search)).ToList();
                //Debug.WriteLine(query);
                //readers = db.Readers.Where(s => s.ReaderFname.Contains(search)|| s.ReaderLname.Contains(search)).ToList();
            }
            else
            {
                books = db.Books.SqlQuery("Select * from books" + order).ToList();;
            }



            return View(books);

        }
        

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Book selectedbook = db.Books.SqlQuery("Select * from books where BookId = @id", new SqlParameter("@id", id)).First();           
           
            if (selectedbook == null)
            {
                return HttpNotFound();
            }
            List<Reader> readers = db.Readers.SqlQuery("Select Readers.* from Books INNER JOIN BookReaders ON Books.bookId = BookReaders.bookId INNER JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId where Books.BookId = @id;", new SqlParameter("@id", id)).ToList();
            //List<Reader> notreaders = db.Readers.SqlQuery("Select Readers.* from Books RIGHT JOIN BookReaders ON Books.bookId = BookReaders.bookId RIGHT JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId WHERE Books.BookId IS NULL OR Books.BookId != @id;", new SqlParameter("@id", id)).ToList();
            List<Reader> allreaders = db.Readers.SqlQuery("Select * from Readers").ToList();
            //Subtract values from allreaders - readers
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-find-the-set-difference-between-two-lists-linq
            //Microsoft Documentation July 20,2015- How to find the set difference between two lists
            IEnumerable<Reader> notreaders = allreaders.Except(readers);
            DetailsBook detailsbook = new DetailsBook();

            detailsbook.NotReaders = notreaders.ToList();
            detailsbook.Book = selectedbook;
            detailsbook.Readers = readers;

            return View(detailsbook);


        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]       
        public ActionResult Create(string BookTitle, string BookDescrp, string BookAuthor, DateTime? BookPublish)
        {
            
            string query = "insert into Books (BookTitle, BookDescrp, BookAuthor, BookPublish) values (@BookTitle,@BookDescrp,@BookAuthor,@BookPublish)";
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@BookTitle", BookTitle);
            sqlparams[1] = new SqlParameter("@BookDescrp", BookDescrp);
            sqlparams[2] = new SqlParameter("@BookAuthor", BookAuthor);
            sqlparams[3] = new SqlParameter("@BookPublish", BookPublish);

            Debug.WriteLine(query);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }

        // GET: Book/Edit/5
        public ActionResult Update(int? id)
        {
            // if id is null return Bad Request
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //id has value so create the selected books
            Book selectedbooks = db.Books.SqlQuery("Select * from books where BookId = @id", new SqlParameter("@id", id)).First();
            //if selected book is null or no value return Page Not found
            if (selectedbooks == null)
            {
                return HttpNotFound();
            }
            
            return View(selectedbooks);

        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Update(int id, string BookTitle, string BookDescrp, string BookAuthor, DateTime BookPublish)
        {

            string query = "update Books set BookTitle = @BookTitle, BookDescrp = @BookDescrp, BookAuthor = @BookAuthor, BookPublish = @BookPublish where BookId = @id ";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@BookTitle", BookTitle);
            sqlparams[1] = new SqlParameter("@BookDescrp", BookDescrp);
            sqlparams[2] = new SqlParameter("@BookAuthor", BookAuthor);
            sqlparams[3] = new SqlParameter("@BookPublish", BookPublish);
            sqlparams[4] = new SqlParameter("@id", id);

            //Debug.WriteLine(query);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book selectedbooks = db.Books.SqlQuery("Select * from books where BookID = @id", new SqlParameter("@id", id)).First();
            
            if (selectedbooks == null)
            {
                return HttpNotFound();
            }
            return View(selectedbooks);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            string query = "delete from books where BookId= @id";
            string query2 = "delete from BookReaders where BookId = @id";
            //Debug.WriteLine(query);
            SqlParameter sqlparams = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            db.Database.ExecuteSqlCommand(query2, sqlparams);
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult deleteReader(int id, int ReaderId)
        {
            //List<Reader> readers = db.Readers.SqlQuery("Select * from Books INNER JOIN BookReaders ON Books.bookId = BookReaders.bookId INNER JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId where Books.BookId = @id;", new SqlParameter("@id", id)).ToList();
            string query = "delete from BookReaders where BookReaders.ReaderId= @ReaderId AND BookReaders.BookId = @id";

            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ReaderId", ReaderId);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //var sample = "Select * from BookReaders INNER JOIN Books ON BookReaders.BookId = Books.BookId WHERE BookReaders.Reader = 1";

            //Debug.WriteLine("BookId = " +id + "ReaderId = "+ ReaderId);
            return RedirectToAction("Details", new { id });
        }


        [HttpPost]
        public ActionResult addReader(int id, int ReaderId)
        {
            //List<Reader> readers = db.Readers.SqlQuery("Select * from Books INNER JOIN BookReaders ON Books.bookId = BookReaders.bookId INNER JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId where Books.BookId = @id;", new SqlParameter("@id", id)).ToList();
         
            string query = "insert into BookReaders (BookId, ReaderId) values (@id, @ReaderId)";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ReaderId", ReaderId);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //var sample = "Select * from BookReaders INNER JOIN Books ON BookReaders.BookId = Books.BookId WHERE BookReaders.Reader = 1";

            //Debug.WriteLine("BookId = " +id + "ReaderId = "+ ReaderId);
            //Return to the same page
            return RedirectToAction("Details", new { id });
        }

    }
}

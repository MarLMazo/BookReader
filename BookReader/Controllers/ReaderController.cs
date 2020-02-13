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

namespace BookReader.Controllers
{
   
    public class ReaderController : Controller
    {
        private BookReaderContext db = new BookReaderContext();

        // GET: Reader
      
        public ActionResult List(string search, string sortOrder)
        {
            string order;

            switch (sortOrder)
            {
                case "last_name":
                    order = " ORDER BY ReaderLname";
                    break;
                case "address":
                    order = " ORDER BY ReaderAddress";
                    break;
                case "phone":
                    order = " ORDER BY ReaderPhone";
                    break;
                default:
                    order = " ORDER BY ReaderFname";
                    break;
            }

            List<Reader> readers;
            if (!string.IsNullOrEmpty(search))
            {
                //readers = db.Readers.ToList();

               readers = db.Readers.SqlQuery("Select * from Readers where ReaderFname LIKE '%'+ @search + '%' OR ReaderLname LIKE '%'+ @search + '%' " +order, new SqlParameter("@search", search)).ToList();
                //readers = db.Readers.Where(s => s.ReaderFname.Contains(search)|| s.ReaderLname.Contains(search)).ToList();
            }
            else
            {
                readers = db.Readers.SqlQuery("Select * from Readers"+ order).ToList();
            }

           

            return View(readers);
        }
        // GET: Reader/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader selectedreader = db.Readers.SqlQuery("Select * from Readers where ReaderId = @id", new SqlParameter("@id", id)).First();
            
            if (selectedreader == null)
            {
                return HttpNotFound();
            }
            List<Book> readbooks = db.Books.SqlQuery("Select * from Readers INNER JOIN BookReaders ON Readers.ReaderId = BookReaders.ReaderId INNER JOIN Books ON BookReaders.BookId = Books.BookId where Readers.ReaderId = @id;", new SqlParameter("@id", id)).ToList();
            //Debug.WriteLine(books);
            List<Book> allbooks = db.Books.SqlQuery("Select * from Books").ToList();
            IEnumerable<Book> notreadbooks = allbooks.Except(readbooks);

            DetailsReader detailsreader = new DetailsReader();
            detailsreader.Reader = selectedreader;
            detailsreader.Books = readbooks;
            detailsreader.notreadBooks = notreadbooks.ToList();
            return View(detailsreader);
        }

        // GET: Reader/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public ActionResult Create(string ReaderFname, string ReaderLname, string ReaderAddress, int ReaderPhone)
        {
          

            string query = "insert into Readers (ReaderFname, ReaderLname, ReaderAddress, ReaderPhone) values (@ReaderFname, @ReaderLname, @ReaderAddress, @ReaderPhone)";
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@ReaderFname", ReaderFname);
            sqlparams[1] = new SqlParameter("@ReaderLname", ReaderLname);
            sqlparams[2] = new SqlParameter("@ReaderAddress", ReaderAddress);
            sqlparams[3] = new SqlParameter("@ReaderPhone", ReaderPhone);

            //Debug.WriteLine(query);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //db.s
            //Customer book = new Customer();
            //book.Books = db.Books.Where(BookdId = bookid);



            //Customer order = new Customer();
            //order.CustomerFname = CustomerFname;
            //order.Books = db.Books.Where( );
            //db.Customers.Add(order);
            //db.SaveChanges();


            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }

        // GET: Reader/Edit/5
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader selectedreader = db.Readers.SqlQuery("Select * from Readers where ReaderId = @id", new SqlParameter("@id", id)).First();

            if (selectedreader == null)
            {
                return HttpNotFound();
            }
            return View(selectedreader);
        }

        // POST: Reader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Update(int id, string ReaderFname, string ReaderLname, string ReaderAddress, int ReaderPhone)
        {
            string query = "update Readers set ReaderFname = @ReaderFname, ReaderLname = @ReaderLname, ReaderAddress = @ReaderAddress, ReaderPhone = @ReaderPhone where ReaderId = @id ";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@ReaderFname", ReaderFname);
            sqlparams[1] = new SqlParameter("@ReaderLname", ReaderLname);
            sqlparams[2] = new SqlParameter("@ReaderAddress", ReaderAddress);
            sqlparams[3] = new SqlParameter("@ReaderPhone", ReaderPhone);
            sqlparams[4] = new SqlParameter("@id", id);

            //Debug.WriteLine(query);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }

        // GET: Reader/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reader selectedreader = db.Readers.SqlQuery("Select * from Readers where ReaderId = @id", new SqlParameter("@id", id)).First();
            if (selectedreader == null)
            {
                return HttpNotFound();
            }
            return View(selectedreader);
        }

        // POST: Reader/Delete/5
        [HttpPost, ActionName("Delete")]
     
        public ActionResult DeleteConfirmed(int id)
        {
            string query = "delete from Readers where ReaderId= @id";
            string query2 = "delete from BookReaders where ReaderId= @id";
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
        public ActionResult deleteBook(int id, int BookId)
        {
            //List<Reader> readers = db.Readers.SqlQuery("Select * from Books INNER JOIN BookReaders ON Books.bookId = BookReaders.bookId INNER JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId where Books.BookId = @id;", new SqlParameter("@id", id)).ToList();
            string query = "delete from BookReaders where BookReaders.BookId= @BookId AND BookReaders.ReaderId = @id";

            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@BookId", BookId);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //var sample = "Select * from BookReaders INNER JOIN Books ON BookReaders.BookId = Books.BookId WHERE BookReaders.Reader = 1";

            //Debug.WriteLine("BookId = " +id + "ReaderId = "+ ReaderId);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public ActionResult addBook(int id, int BookId)
        {
            //List<Reader> readers = db.Readers.SqlQuery("Select * from Books INNER JOIN BookReaders ON Books.bookId = BookReaders.bookId INNER JOIN Readers ON BookReaders.ReaderId = Readers.ReaderId where Books.BookId = @id;", new SqlParameter("@id", id)).ToList();

            string query = "insert into BookReaders (BookId, ReaderId) values (@BookId, @id)";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@BookId", BookId);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //var sample = "Select * from BookReaders INNER JOIN Books ON BookReaders.BookId = Books.BookId WHERE BookReaders.Reader = 1";

            //Debug.WriteLine("BookId = " +id + "ReaderId = "+ ReaderId);
            //Return to the same page
            return RedirectToAction("Details", new { id });
        }
    }
}


//public ActionResult Index()
//{
//    return View();
//}
//public ActionResult List()
//{
//    List<Customer> customers = db.Customers.SqlQuery("Select * from customers").ToList();
//    return View(customers);

//}
//public ActionResult Details(int id)
//{
//    Customer selectedcustomer = db.Customers.SqlQuery("Select * from Customers where CustomerID = @id", new SqlParameter("@id", id)).First();
//    return View(selectedcustomer);
//}




//public ActionResult Create()
//{
//    return View();
//}

//[HttpPost]
//public ActionResult Create(string CustomerFname, string CustomerLname, string CustomerAddress, string CustomerPhone, int CustomerRented)
//{
//    string query = "insert into Customers (CustomerFname, CustomerLname, CustomerAddress, CustomerPhone, CustomerRented) values (@CustomerFname, @CustomerLname, @CustomerAddress, @CustomerPhone, @CustomerRented)";
//    SqlParameter[] sqlparams = new SqlParameter[5];
//    sqlparams[0] = new SqlParameter("@CustomerFname", CustomerFname);
//    sqlparams[1] = new SqlParameter("@CustomerLname", CustomerLname);
//    sqlparams[2] = new SqlParameter("@CustomerAddress", CustomerAddress);
//    sqlparams[3] = new SqlParameter("@CustomerPhone", CustomerPhone);
//    sqlparams[4] = new SqlParameter("@CustomerRented", CustomerRented);

//    //Debug.WriteLine(query);
//    db.Database.ExecuteSqlCommand(query, sqlparams);
//    //db.s
//    //Customer book = new Customer();
//    //book.Books = db.Books.Where(BookdId = bookid);



//    //Customer order = new Customer();
//    //order.CustomerFname = CustomerFname;
//    //order.Books = db.Books.Where( );
//    //db.Customers.Add(order);
//    //db.SaveChanges();


//    //run the list method to return to a list of pets so we can see our new one!
//    return RedirectToAction("List");
//}

////Update
//public ActionResult Update(int id)
//{
//    Customer selectedcustomer = db.Customers.SqlQuery("Select * from Customers where CustomerID = @id", new SqlParameter("@id", id)).First();
//    return View(selectedcustomer);
//}

//[HttpPost]
//public ActionResult Update(int id, string CustomerFname, string CustomerLname, string CustomerAddress, string CustomerPhone, int CustomerRented)
//{
//    string query = "update Customers set CustomerFname = @CustomerFname, CustomerLname = @CustomerLname, CustomerAddress = @CustomerAddress, CustomerPhone = @CustomerPhone, CustomerRented = @CustomerRented where CustomerID = @id ";
//    SqlParameter[] sqlparams = new SqlParameter[6];
//    sqlparams[0] = new SqlParameter("@CustomerFname", CustomerFname);
//    sqlparams[1] = new SqlParameter("@CustomerLname", CustomerLname);
//    sqlparams[2] = new SqlParameter("@CustomerAddress", CustomerAddress);
//    sqlparams[3] = new SqlParameter("@CustomerPhone", CustomerPhone);
//    sqlparams[4] = new SqlParameter("@CustomerRented", CustomerRented);
//    sqlparams[5] = new SqlParameter("@id", id);

//    //Debug.WriteLine(query);
//    db.Database.ExecuteSqlCommand(query, sqlparams);

//    //run the list method to return to a list of pets so we can see our new one!
//    return RedirectToAction("List");
//}

////Delete
//public ActionResult Delete(int? id)
//{
//    Customer selectedcustomer = db.Customers.SqlQuery("Select * from Customers where CustomerID = @id", new SqlParameter("@id", id)).First();
//    return View(selectedcustomer);

//}


//[HttpPost]
//public ActionResult Delete(int id)
//{
//    string query = "delete from customers where CustomerID= @id";
//    SqlParameter sqlparams = new SqlParameter("@id", id);
//    db.Database.ExecuteSqlCommand(query, sqlparams);
//    return RedirectToAction("List");
//}

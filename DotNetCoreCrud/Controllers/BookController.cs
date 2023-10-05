using DotNetCoreCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DotNetCoreCrud.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;
        BookViewModel _bookmodel = new BookViewModel();

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Connectionstring")))
            {
                conn.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("ViewAllBooks", conn);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.Fill(dt);
            }
            return View(dt);
        }
        public IActionResult CreateorEdit(int? id)
        {
            if(id>0)
            {
                _bookmodel = Fetchbook(id);
            }
            return View(_bookmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateorEdit(int id, [Bind("BookID,Title,Author,Price")] BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Connectionstring")))
                {
                    conn.Open();
                    SqlCommand sqlcom = new SqlCommand("BookCreateorEdit", conn);
                    sqlcom.CommandType = CommandType.StoredProcedure;
                    sqlcom.Parameters.AddWithValue("bookid", book.BookID);
                    sqlcom.Parameters.AddWithValue("title", book.Title);
                    sqlcom.Parameters.AddWithValue("author", book.Author);
                    sqlcom.Parameters.AddWithValue("Price", book.Price);
                    sqlcom.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(book);
        }
        public IActionResult Delete(int? id)
        {
            _bookmodel = Fetchbook(id);
            return View(_bookmodel);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn= new SqlConnection(_configuration.GetConnectionString("Connectionstring")))
            {
                conn.Open();
                SqlCommand sqlcom = new SqlCommand("Deletebookbyid", conn);
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.Parameters.AddWithValue("bookid", id);

            }
            return View(_bookmodel);
        }

        public BookViewModel Fetchbook(int? id)
        {
            DataTable dt = new DataTable();
            using(SqlConnection conn =new SqlConnection(_configuration.GetConnectionString("Connectionstring")))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("Viewbookbyid", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("Bookid", id);
                da.Fill(dt);
                if(dt.Rows.Count==1)
                {
                    _bookmodel.BookID = Convert.ToInt32(dt.Rows[0]["bookid"].ToString());
                    _bookmodel.Author = dt.Rows[0]["Author"].ToString();
                    _bookmodel.Title = dt.Rows[0]["Title"].ToString();
                    _bookmodel.Price =Convert.ToInt32(dt.Rows[0]["Price"].ToString());
                }
                return _bookmodel;
            }
        }
    }
}

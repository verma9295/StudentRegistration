using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Document = iTextSharp.text.Document;

namespace StudentRegistration.Controllers
{
    public class AdminController : Controller
    {
        public readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index(string searchName,string sortOrder)
        {
            if (searchName == null)
            {
                ViewBag.name = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                var data = (from a in _db.regForms
                            join b in _db.tblstates on a.State equals b.sid
                            join c in _db.tblcities on a.City equals c.cid
                            join d in _db.tblgenders on a.Gender equals d.gid
                            select new tbljoin
                            {
                                Id = a.Id,
                                Name = a.Name,
                                fName = a.fName,
                                Category = a.Category,
                                regno = a.regno,
                                Age = a.Age,
                                Gender = d.gname,
                                State = b.sname,
                                City = c.cname,
                                Email = a.Email,
                                ProfileImage = a.ProfileImage
                            }).ToList();
                switch (sortOrder)
                {
                    case "name_desc":
                        data = (from a in _db.regForms
                                join b in _db.tblstates on a.State equals b.sid
                                join c in _db.tblcities on a.City equals c.cid
                                join d in _db.tblgenders on a.Gender equals d.gid
                                select new tbljoin
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    fName = a.fName,
                                    Category = a.Category,
                                    regno = a.regno,
                                    Age = a.Age,
                                    Gender = d.gname,
                                    State = b.sname,
                                    City = c.cname,
                                    Email = a.Email,
                                    ProfileImage = a.ProfileImage
                                }).OrderByDescending(s => s.Name).ToList();
                        break;

                    default:
                        data = (from a in _db.regForms
                                join b in _db.tblstates on a.State equals b.sid
                                join c in _db.tblcities on a.City equals c.cid
                                join d in _db.tblgenders on a.Gender equals d.gid
                                select new tbljoin
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    fName = a.fName,
                                    Category = a.Category,
                                    regno = a.regno,
                                    Age = a.Age,
                                    Gender = d.gname,
                                    State = b.sname,
                                    City = c.cname,
                                    Email = a.Email,
                                    ProfileImage = a.ProfileImage
                                }).OrderBy(s => s.Name).ToList();
                        break;
                }
                return View(data);
            }
            else
            {
                var searchResult = (from a in _db.regForms
                                    join b in _db.tblstates on a.State equals b.sid
                                    join c in _db.tblcities on a.City equals c.cid
                                    join d in _db.tblgenders on a.Gender equals d.gid
                                    select new tbljoin
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        fName = a.fName,
                                        Category = a.Category,
                                        regno = a.regno,
                                        Age = a.Age,
                                        Gender = d.gname,
                                        State = b.sname,
                                        City = c.cname,
                                        Email = a.Email,
                                        ProfileImage = a.ProfileImage
                                    }).Where(x => x.Name.StartsWith(searchName)).ToList();
                return View(searchResult);
            }
        }


        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            ViewBag.BT = "Submit";
            tblCollection _userForm = new tblCollection();
            if (id > 0)
            {
                var data = _db.regForms.Where(x => x.Id == id).ToList();
                _userForm.Id = data[0].Id;
                _userForm.Name = data[0].Name;
                _userForm.fName = data[0].fName;
                _userForm.Category = data[0].Category;
                _userForm.regno = data[0].regno;
                _userForm.Age = data[0].Age;
                _userForm.Gender = data[0].Gender;
                _userForm.State = data[0].State;
                _userForm.City = data[0].City;
                _userForm.Email = data[0].Email;
                _userForm.ProfileImage = data[0].ProfileImage;
                TempData["img"] = data[0].ProfileImage;
                ViewBag.BT = "Update";
            }
            ViewBag.citi = (from a in _db.tblcities where a.sid == _userForm.State select a).ToList();
            ViewBag.stt = _db.tblstates.ToList();
            ViewBag.gender = _db.tblgenders.ToList();

            return View(_userForm);
        }

        [HttpPost]
        public IActionResult Create(RegForm u, IFormFile file)
        {
            string path = Path.Combine("wwwroot/Images");
            string newFN = Guid.NewGuid().ToString() + "-" + file.FileName;
            string filepath = Path.Combine(path, newFN);
            using (FileStream fs = System.IO.File.Create(filepath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            if (u.Id > 0)
            {
                if (file != null)
                {
                    string aa = TempData["img"].ToString();
                    string oldpath = Path.Combine("wwwroot/Images", aa);
                    if (System.IO.File.Exists(oldpath))
                    {
                        System.IO.File.Delete(oldpath);
                    }

                    u.ProfileImage = newFN;
                }
                _db.Entry(u).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["updateMass"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                if (file != null)
                {
                    u.ProfileImage = newFN;
                    _db.regForms.Add(u);
                    _db.SaveChanges();
                    TempData["succesMass"] = "Created Successfully";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public IActionResult Delete(int id = 0)
        {
            var data = _db.regForms.Find(id);
            var aa = data.ProfileImage;
            string path = Path.Combine("wwwroot/Images", aa);
            System.IO.File.Delete(path);
            _db.regForms.Remove(data);
            _db.SaveChanges();
            TempData["delMess"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Admin", "Admin");
        }

        public JsonResult GetCity(int A)
        {
            var data = _db.tblcities.Where(x => x.sid == A).ToList();
            return Json(data);
        }

        public IActionResult UserRecords(string searchName)
        {
            if(searchName == null)
            {
                var record = (from a in _db.Users
                              join b in _db.tblgenders on a.Gender equals b.gid
                              select new userJoin
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Age = a.Age,
                                  Gender = b.gname,
                                  Mobile = a.Mobile,
                                  Username = a.Username,
                                  Password = a.Password,
                                  ConfirmPassword = a.ConfirmPassword,
                              }).ToList();
                return View(record);
            }
            else
            {
                var record = (from a in _db.Users
                              join b in _db.tblgenders on a.Gender equals b.gid
                              select new userJoin
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Age = a.Age,
                                  Gender = b.gname,
                                  Mobile = a.Mobile,
                                  Username = a.Username,
                                  Password = a.Password,
                                  ConfirmPassword = a.ConfirmPassword,
                              }).Where(x => x.Name.StartsWith(searchName)).ToList();
                return View(record);
            }
        }

        public IActionResult CreateUser(int id = 0)
        {
            ViewBag.BT = "Create";
            UserCollection user = new UserCollection();
            if (id > 0)
            {
                var userData = _db.Users.Where(x => x.Id == id).ToList();
                user.Name = userData[0].Name;
                user.Age = userData[0].Age;
                user.Gender = userData[0].Gender;
                user.Mobile = userData[0].Mobile;
                user.Username = userData[0].Username;
                user.Password = userData[0].Password;
                user.ConfirmPassword = userData[0].ConfirmPassword;
                ViewBag.BT = "Update";
            }
            ViewBag.gender = _db.tblgenders.ToList();
            return View(user);
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (user.Id > 0)
            {
                _db.Entry(User).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("UserRecords");
            }
            else
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("LogIn");
            }
        }
        public IActionResult DeleteUser(int id=0)
        {
            var del = _db.Users.Find(id);
            _db.Users.Remove(del);
            _db.SaveChanges();
            TempData["delMass"] = "Deleted Successfully";
            return RedirectToAction("UserRecords");
        }


        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Admin(Admin admin)
        {
            var data = _db.Admins.Where(x => x.Username == admin.Username && x.Password == admin.Password).ToList();
            if (data.Count > 0)
            {
                HttpContext.Session.SetInt32("UserId", data[0].Id);
                TempData["success"] = admin.Username;

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.error = "Email and Password is not Matched";
                return View();
            }
        }

        [HttpPost]
        public IActionResult PDF()
        {

            using (MemoryStream ms = new MemoryStream())
            {
                //Document document = new Document(PageSize.HALFLETTER, 25, 25, 30, 30);
                Document document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                var image = iTextSharp.text.Image.GetInstance("wwwroot/Template/img/s2.jpg");
                image.Alignment = Element.ALIGN_CENTER;
                document.Add(image);
                ///report heading
                Paragraph para1 = new Paragraph("", new Font(Font.FontFamily.COURIER, 15));
                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);

                Paragraph para2 = new Paragraph("User Registration Report", new Font(Font.FontFamily.HELVETICA, 15));
                para2.Alignment = Element.ALIGN_CENTER;
                document.Add(para2);


                Paragraph para3 = new Paragraph("     ", new Font(Font.FontFamily.COURIER, 10));
                para3.Alignment = Element.ALIGN_CENTER;
                document.Add(para3);

                ///for table generate
                PdfPTable Tblreg = new PdfPTable(11);

                PdfPCell cell1 = new PdfPCell(new Phrase("Id", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Name", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("S/O", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Category", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("Registration", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Age", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell6);


                PdfPCell cell7 = new PdfPCell(new Phrase("Gender", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase("State", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase("City", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase("Email", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell10);

                PdfPCell cell11 = new PdfPCell(new Phrase("Photo", new Font(Font.FontFamily.HELVETICA, 13)));
                Tblreg.AddCell(cell11);



                var data = (from a in _db.regForms

                            join b in _db.tblstates
                            on a.State equals b.sid

                            join c in _db.tblcities
                            on a.City equals c.cid

                            join d in _db.tblgenders
                            on a.Gender equals d.gid


                            into Con
                            from d in Con.DefaultIfEmpty()
                            select new tbljoin
                            {
                                Id = a.Id,
                                Name = a.Name,
                                fName = a.fName,
                                Category = a.Category,
                                regno = a.regno,
                                Age = a.Age,
                                Gender = d.gname,
                                State = b.sname,
                                City = c.cname,
                                Email = a.Email,
                                ProfileImage = a.ProfileImage

                            }).ToList();
                //var data = (from a in _db.Users select a).ToList();
                for (int i = 0; i < data.Count; i++)
                {

                    PdfPCell cell_1 = new PdfPCell(new Phrase(data[i].Id.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(data[i].Name));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(data[i].fName));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(data[i].Category));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(data[i].regno));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(data[i].Age.ToString()));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(data[i].Gender));
                    PdfPCell cell_8 = new PdfPCell(new Phrase(data[i].State));
                    PdfPCell cell_9 = new PdfPCell(new Phrase(data[i].City));
                    PdfPCell cell_10 = new PdfPCell(new Phrase(data[i].Email));
                    PdfPCell cell_11 = new PdfPCell(new Phrase(data[i].ProfileImage));


                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_5.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_7.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_8.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_9.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_10.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_11.HorizontalAlignment = Element.ALIGN_CENTER;
                    // cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblreg.AddCell(cell_1);
                    Tblreg.AddCell(cell_2);
                    Tblreg.AddCell(cell_3);
                    Tblreg.AddCell(cell_4);
                    Tblreg.AddCell(cell_5);
                    Tblreg.AddCell(cell_6);
                    Tblreg.AddCell(cell_7);
                    Tblreg.AddCell(cell_8);
                    Tblreg.AddCell(cell_9);
                    Tblreg.AddCell(cell_10);
                    Tblreg.AddCell(cell_11);
                }

                document.Add(Tblreg);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "applaction/vnd", "Registration_Report.pdf");
            }

        }


    }
}

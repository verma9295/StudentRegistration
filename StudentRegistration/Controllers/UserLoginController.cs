using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentRegistration.Controllers
{
    public class UserLoginController : Controller
    {
        public readonly ApplicationDbContext _db;
        public UserLoginController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
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

        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            var userName = HttpContext.Session.GetString("Username");
            ViewBag.tt = userName;

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
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("oneUserRecords");
            }
            else
            {
                if (file != null)
                {
                    u.ProfileImage = newFN;

                    _db.regForms.Add(u);
                    _db.SaveChanges();
                    TempData["succesMasse"] = "Created Successfully";
                    return RedirectToAction("Create");
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
            TempData["delMass"] = "Deleted Successfully";
            return RedirectToAction("Create");
        }

        public JsonResult GetCity(int A)
        {
            var data = _db.tblcities.Where(x => x.sid == A).ToList();
            return Json(data);
        }
        public IActionResult CreateUser(int id=0)
        {
            ViewBag.BT = "Create";
            UserCollection user = new UserCollection();
            if(id > 0)
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
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            if (user.Id > 0)
            {
                _db.Entry(User).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            return View();
        }
        [AcceptVerbs("Post","Get")]
        public IActionResult UserNameValidate(string userName)  //username validation
        {
            var data = _db.Users.Where(u=>u.Username == userName).SingleOrDefault();
            if(data != null)
            {
                return Json($"Username {userName} already in use!");
            }
            else
            {
                return Json(true);
            }
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(User log)
        {
            var data = _db.Users.Where(x => x.Username == log.Username && x.Password == log.Password).ToList();
            if (data.Count > 0)
            {
                HttpContext.Session.SetString("Username", log.Username); //one row found User record
                HttpContext.Session.SetInt32("UserId", data[0].Id);
                TempData["success"] = log.Username;

                return RedirectToAction("Create", "UserLogin");
            }
            else
            {
                ViewBag.error = "Email and Password is not Matched";
                return View();
            }
        }
        public IActionResult Reset()
        {
            ModelState.Clear();
            return RedirectToAction("Create", "UserLogin");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("LogIn", "UserLogin");
        }
        [HttpGet]
        public IActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePass(ChangePassword _cp)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var data = _db.Users.Find(userId);
            if(data.Password == _cp.OldPass)
            {
                if(_cp.NewPass == _cp.ConfirmPass)
                {
                    data.Password = _cp.NewPass;
                    data.ConfirmPassword = _cp.ConfirmPass;
                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
                    TempData["changePass"] = "Change password Successfully";
                    return RedirectToAction("ChangePass", "UserLogin");
                }
                else
                {
                    ViewBag.msg = "Password is incorrect";
                }
            }
            else
            {
                ViewBag.msg = "Password are not matched";

            }
            return View();
        }



        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Forgot(User user)
        {
            if (user != null)
            {
                var data = _db.Users.Where(x => x.Username == user.Username).ToList();
                if (data.Count > 0)
                {
                    HttpContext.Session.SetInt32("UserId", data[0].Id);
                    return RedirectToAction("ForgotPass");
                }
                else
                {
                    ViewBag.forgotError = "Username is not matched";
                }

            }
            else
            {
                ViewBag.forgotError = "Username is not matched";
            }
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPass(User _user)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var data = _db.Users.Find(userId); 
            if (_user.Password == _user.ConfirmPassword)
                {
                    data.Password = _user.Password;
                    data.ConfirmPassword = _user.ConfirmPassword;
                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
                    TempData["changePass"] = "Change password Successfully";
                    return RedirectToAction("ForgotPass", "UserLogin");
                }
            else
                {
                    ViewBag.msg = "Password is incorrect";
                }
            return View();
        }

        public IActionResult oneUserRecords()
        {
            var userName = HttpContext.Session.GetString("Username");
            var data1 = (from a in _db.regForms
                            join b in _db.tblstates on a.State equals b.sid
                            join c in _db.tblcities on a.City equals c.cid
                            join d in _db.tblgenders on a.Gender equals d.gid

                            where a.Email == userName
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
                return View(data1);
           
        }

    }
}

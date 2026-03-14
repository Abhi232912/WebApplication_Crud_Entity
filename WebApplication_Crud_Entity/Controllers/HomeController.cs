using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebApplication_Crud_Entity.Models;
using System.Web.Security;

namespace WebApplication_Crud_Entity.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                PractiseEntities pe =new PractiseEntities();
                
                 pe.Registers.Add(register);
                pe.SaveChanges();
            }
            


                return View();
        }




        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            PractiseEntities pe = new PractiseEntities();

            var login=pe.Users.FirstOrDefault(u =>u.User1==user.User1&&u.password==user.password);

            if (login != null)
            {
             FormsAuthentication.SetAuthCookie(login.User1, false);
                Session["Role_id"]=login.Role_id;
                Session["user"] = login.User1;

                var role = pe.Roles_Table.Find(login.Role_id);
                Session["Role"] = role.Role;

                ViewBag.loginError = "Logined";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.loginError = "Enter Details Correctly";
            }

                return View();
        }


        public ActionResult Index()
        {
            PractiseEntities pe = new PractiseEntities();
           
            List<EMp> emp=pe.EMps.ToList();
           foreach(var i in emp)
            {
                i.photo = string.IsNullOrEmpty(i.photo) ? "NO-Photo.jpg" : i.photo;

            }
            return View(emp);
        }


       [Authorize]

        [HttpGet]
        public ActionResult Insert()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        

       [HttpPost]
        public ActionResult Insert(EMp eMp,HttpPostedFileBase photo) 
        {
            try
            {
                PractiseEntities pe = new PractiseEntities();
                string filename = ""; string path = "";

                if (photo != null && photo.ContentLength > 0)
                {
                    filename = Path.GetFileName(photo.FileName);

                    path = Path.Combine(Server.MapPath("~/Uploads/photo"), filename);
                    photo.SaveAs(path);
                    eMp.photo = filename;
                }
                else
                {
                    eMp.photo = "NO-Photo.jpg";
                }


                    
                pe.EMps.Add(eMp);

                int save = pe.SaveChanges();

                if (save == 1)
                {
                    ViewBag.msg = "Record Inserted";
                }
                else
                {
                    ViewBag.msg = "Record ! Inserted";

                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                return View("Insert");
            }
           
        }
    }
}
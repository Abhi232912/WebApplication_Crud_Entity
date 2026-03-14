using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using WebApplication_Crud_Entity.Models;

namespace WebApplication_Crud_Entity.Controllers
{
    public class otherController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            PractiseEntities pe =new PractiseEntities();
            var emp = pe.EMps.Where(i => i.Id == id).FirstOrDefault();
          
            EMp e = new EMp();
            e.Id = emp.Id;
            e.Name = emp.Name;
            e.sal=emp.sal;   
           return View(emp);    
        }

        [HttpPost]
        public ActionResult Edit(EMp e) 
        { 
            PractiseEntities pe=new PractiseEntities();
            var emp = pe.EMps.Find(e.Id);
            emp.Name=e.Name;
            emp.sal = e.sal;
            pe.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public ActionResult Delete(int id)
        {
            PractiseEntities pe = new PractiseEntities();
            var emp = pe.EMps.Find(id);
            return View(emp);
        }


        [HttpPost]
        public ActionResult Delete(EMp eMp)
        {
            PractiseEntities pe = new PractiseEntities();
            var emp = pe.EMps.Find(eMp.Id);
            pe.EMps.Remove(emp);
            pe.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}
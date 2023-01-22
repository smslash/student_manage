using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManage.Models;

namespace StudentManage.Controllers {
  public class HomeController : Controller {
    DBStudentManageEntities1 db = new DBStudentManageEntities1();
    public ActionResult Index() {
      return View();
    }

    [HttpGet]
    public ActionResult InsertClassRoom() {
      return View();
    }

    [HttpPost]
    public ActionResult InsertClassRoom(ClassRoom classRoom) {
      db.ClassRooms.Add(classRoom);
      db.SaveChanges();
      return View();
    }


    public PartialViewResult PrintClassRoom() {
      var listClassRoom = db.ClassRooms.ToList();
      return PartialView(listClassRoom);
    }

    public ActionResult DeleteClassRoom(int id) {
      try {
        var DeleteClassroom = db.ClassRooms.Where(x => x.IdCr == id).FirstOrDefault();
        db.ClassRooms.Remove(DeleteClassroom);
        db.SaveChanges();
        return RedirectToAction("Index");

      } catch {
        return View();
      }

    }

    [HttpGet]
    public ActionResult AddStudent(int id) {
      ViewBag.Test = id;
      return View();
    }

    [HttpPost]
    public ActionResult AddStudent(Student student, int id) {
      db.Students.Add(student);
      db.SaveChanges();
      return View();
    }

    public ActionResult ListStudent( int id) {
      var listStudent = (from cr in db.ClassRooms
                         from st in db.Students
                         where cr.IdCr == st.IdCr && id == st.IdCr
                         select st).ToList();
      return View(listStudent);
    }

    public ActionResult DeleteStudent(int id) {
      try {
        var DeleteStudent= db.Students.Where(x => x.IdSt == id).FirstOrDefault();
        db.Students.Remove(DeleteStudent);
        db.SaveChanges();
        return RedirectToAction("Index");

      } catch {
        return View();
      }

    }

    public ActionResult ShowAllStudent() {
      var ShowAllStudent = db.Students.ToList();
      return View(ShowAllStudent);

    }

    public ActionResult OrderByAscId() {
      var OrderByAscId = db.Students.OrderBy(z=>z.IdSt).ToList();
      return View(OrderByAscId);

    }

    public ActionResult OrderByDsccId() {
      var OrderByDscId = db.Students.OrderByDescending(z => z.IdSt).ToList();
      return View(OrderByDscId);

    }

    public ActionResult IdMax() {
      var IdMax = db.Students.OrderByDescending(z => z.IdSt).Take(1).ToList();
      return View(IdMax);

    }
    public ActionResult IdMin() {
      var IdMin = db.Students.OrderBy(z => z.IdSt).Take(1).ToList();
      return View(IdMin);

    }

    public ActionResult SearchByName(FormCollection f) {
      String KeySearchName = f["SearchValue"].ToString();
      List<Student> ListSearch = db.Students.Where(x => x.NameSt.Contains(KeySearchName)).ToList();
      if (ListSearch.Count == 0) {
        ViewBag.Notification = "Нет результатов поиска";
      } else {
        ViewBag.Notification = "Найдено"+ListSearch.Count+"Результатов";
      }
      return View(ListSearch.OrderBy(x => x.NameSt));
    }

    public ActionResult Edit(int id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Student student = db.Students.Find(id);
      if (student== null) {
        return HttpNotFound();
      }
      
      return View(student);
    }
  }
}
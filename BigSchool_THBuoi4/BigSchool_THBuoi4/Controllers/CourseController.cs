using BigSchool_THBuoi4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool_THBuoi4.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course x = new Course();
            x.listCategory = context.Category.ToList();
            return View(x);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course x)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                x.listCategory = context.Category.ToList();
                return View("Create", x);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            x.LecturerId = user.Id;
            context.Course.Add(x);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser =
System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName =
               System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser =
           System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Course.Where(c => c.LecturerId == currentUser.Id && c.Date_Time > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LectureName = currentUser.Name; //Name la cot da them vao Aspnetuser  } 
            }
            return View(courses);

        }

        public ActionResult Edit(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course x = context.Course.FirstOrDefault(p => p.Id == id);
            x.listCategory = context.Category.ToList();
            return View(x);
        }

        [HttpPost,ActionName("Edit")]
        public ActionResult xoa1Course(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course x = context.Course.FirstOrDefault(P => P.Id == id);
            UpdateModel(x);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course x = context.Course.FirstOrDefault(P => P.Id == id);

            //Trước tiên, kiểm tra khóa ngoại ở bảng Attendances...
            Attendance y = context.Attendance.FirstOrDefault(P => P.CourseId == id);

            if (y != null)
                return HttpNotFound("Khong the xoa khoa hoc hien tai, vui long kiem tra khoa ngoai tu bang Attendance !");

            context.Course.Remove(x);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
           System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()); BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId == 
            currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký 
            var listAttendances = context.Attendance.Where(p => p.Attendee == 
            currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course; objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name; courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }
    }
}
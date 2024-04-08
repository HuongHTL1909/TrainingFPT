using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? search)
        {
            CoursesViewModel course = new CoursesViewModel();
            course.CourseDetailList = new List<CourseDetail>();
            var dataCourses = new CourseQuery().GetAllDataCourses(search);
            foreach (var data in dataCourses)
            {
                course.CourseDetailList.Add(new CourseDetail
                {
                    Id = data.Id,
                    NameCourse = data.NameCourse,
                    CategoryId = data.CategoryId,
                    Description = data.Description,
                    Status = data.Status,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ViewImageCourse = data.ViewImageCourse,
                    viewCategoryName = data.viewCategoryName,
                    ViewStartDate = data.ViewStartDate,
                    ViewEndDate = data.ViewEndDate
                });
            }
            ViewBag.keyword = search;
            return View(course);
        }
        [HttpGet]
        public IActionResult Add()
        {
            CourseDetail course = new CourseDetail();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseDetail course, IFormFile Image)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string imageCourse = UploadFileHelper.UploadFile(Image, "images");
                    int idCourse = new CourseQuery().InsetDataCourse(
                        course.NameCourse,
                        course.CategoryId,
                        course.Description,
                        course.StartDate,
                        course.EndDate,
                        course.Status,
                        imageCourse
                    );
                    if(idCourse > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(CoursesController.Index), "Courses");
                }
                catch (Exception ex)
                {
                    //neu co loi
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);

        }


        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            CourseDetail detail = new CourseQuery().GetDetailCourseById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(detail);
        }

        [HttpPost]
        public IActionResult Update(CourseDetail courseDetail, IFormFile Image)
        {
            try
            {
                var infoCourse = new CourseQuery().GetDetailCourseById(courseDetail.Id);
                string imageCourse = infoCourse.ViewImageCourse;
                //kiểm tra xem người dùng có muốn thay đổi ảnh hay ko
                if (courseDetail.Image != null)
                {
                    //có thay đổi ảnh
                    imageCourse = UploadFileHelper.UploadFile(Image, "images");
                }
                bool update = new CourseQuery().UpdateCourseById(
                    courseDetail.NameCourse,
                    courseDetail.CategoryId,
                    courseDetail.Description,
                    imageCourse,
                    courseDetail.Status,
                    courseDetail.StartDate,
                    courseDetail.EndDate,
                    courseDetail.Id
                );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CoursesController.Index), "Courses");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(courseDetail);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new CourseQuery().DeleteItemCourse(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CoursesController.Index), "Courses");
        }

    }
}

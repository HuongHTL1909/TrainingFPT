using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Models.Queries;
using TrainingFPT.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPT.Migrations;
using TrainingFPT.Helpers;

namespace TrainingFPT.Controllers
{
    public class TopicsController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? search, string? status)
        {
            TopicsViewModel topic = new TopicsViewModel();
            topic.TopicDetailList = new List<TopicDetail>();
            var dataTopics = new TopicQuery().GetAllDataTopics(search, status);
            foreach (var data in dataTopics)
            {
                topic.TopicDetailList.Add(new TopicDetail
                {
                    Id = data.Id,
                    NameTopic = data.NameTopic,
                    CourseId = data.CourseId,
                    Description = data.Description,
                    NameVideo = data.NameVideo,
                    NameAudio = data.NameAudio,
                    NameDocumentTopic = data.NameDocumentTopic,
                    Status = data.Status,
                    viewCourseName = data.viewCourseName,   
                });
            }
            ViewData["keyword"] = search;
            ViewBag.Status = status;
            return View(topic);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course. Id.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(topic);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TopicDetail topic, IFormFile Video, IFormFile Audio, IFormFile DocumentTopic)
        {

            // Kiểm tra xem ít nhất một trong các tệp tin đã được chọn
            if (Video == null && Audio == null && DocumentTopic == null)
            {
                ModelState.AddModelError("", "Please choose at least one file (Video, Audio, or Document).");
            }
            //return Ok(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    string videoTopic = UploadFileHelper.UploadFile(Video, "videos");
                    string audioTopic = UploadFileHelper.UploadFile(Audio, "audios");
                    string documentTopic = UploadFileHelper.UploadFile(DocumentTopic, "documents");
                    int idTopic = new TopicQuery().InsetDataTopic(
                        topic.NameTopic,
                        topic.CourseId,
                        topic.Description,
                        videoTopic,
                        audioTopic,
                        documentTopic,
                        topic.Status
                    );
                    if (idTopic > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(TopicsController.Index), "Topics");
                }
                catch (Exception ex)
                {
                    //neu co loi
                    return Ok(ex.Message);
                }
                
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(topic);

        }


        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            TopicDetail detail = new TopicQuery().GetDetailTopicById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.NameCourse
                });
            }
            ViewBag.Courses = items;
            return View(detail);
        }

        [HttpPost]
        public IActionResult Update(TopicDetail topicDetail, IFormFile Video, IFormFile Audio, IFormFile DocumentTopic)
        {
            try
            {
                var infoTopic = new TopicQuery().GetDetailTopicById(topicDetail.Id);
                string videoTopic = infoTopic.NameVideo;
                string audioTopic = infoTopic.NameAudio;
                string documentTopic = infoTopic.NameDocumentTopic;
                //kiểm tra xem người dùng có muốn thay đổi ảnh hay ko
                if (topicDetail.Video != null)
                {
                    //có thay đổi ảnh
                    videoTopic = UploadFileHelper.UploadFile(Video, "videos");
                }
                if (topicDetail.Audio != null)
                {
                    audioTopic = UploadFileHelper.UploadFile(Audio, "audios");
                }

                if (topicDetail.DocumentTopic != null)
                {
                    documentTopic = UploadFileHelper.UploadFile(DocumentTopic, "documents");
                }
                bool update = new TopicQuery().UpdateTopicById(
                    topicDetail.NameTopic,
                    topicDetail.CourseId,
                    topicDetail.Description,
                    videoTopic,
                    audioTopic,
                    documentTopic,
                    topicDetail.Status,
                    topicDetail.Id
                );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(TopicsController.Index), "Topics");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new TopicQuery().DeleteItemTopic(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(TopicsController.Index), "Topics");
        }
    }
}

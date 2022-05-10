#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using whatsappProject.Data;
using whatsappProject.Models;
namespace whatsappProject.Controllers
{
    public interface IFeedBackService
    {
        IEnumerable<FeedBack> GetAllFeedBacks();
        FeedBack GetFeedBack(int id);

        void DeleteFeedBack(int id);

        void UpdateFeedBack(int id, int score, string name, string feedback);

        void CreateFeedBack(FeedBack feedBack);

        float getAverage();
    }
    public class FeedBackService : IFeedBackService
    {
        private static List<FeedBack> _feeds = new List<FeedBack>();
        private static int _id = 1;
        public float getAverage()
        {
            float sum = 0;
            float size = _feeds.Count();
            foreach (FeedBack feedback in _feeds)
            {
                sum += feedback.Score;
            }
            if (size > 0)
            {
                return sum / size;
            }
            return 0;

        }
        public void CreateFeedBack(FeedBack feedBack)
        {
            feedBack.Date = DateTime.Now.ToString();
            feedBack.Id= _id;
            _id++;
            _feeds.Add(feedBack);
        }

        public void DeleteFeedBack(int id)
        {
            FeedBack f = _feeds.Find(x => x.Id == id);
            if (f != null)
            {
                _feeds.Remove(_feeds.Find(x => x.Id == id));
            }
        }

        public IEnumerable<FeedBack> GetAllFeedBacks()
        {
            return _feeds;
        }

        public FeedBack GetFeedBack(int id)
        {
            return _feeds.Find(x => x.Id == id);

        }
            public void UpdateFeedBack(int id,int score,string name,string feedback)
        {
            FeedBack f = GetFeedBack(id);
            if (f != null)
            {
                f.Score = score;
                f.Name = name;
                f.FeedbackContent = feedback;
                f.Date = DateTime.Now.ToString();
            }
        }
    }
    public class FeedBacksController : Controller
    {
        private readonly IFeedBackService _service;


        public FeedBacksController(IFeedBackService service)
        {
            _service = service;
        }

        // GET: FeedBacks
        public IActionResult Index()
        {
            List<FeedBack> feedBacks = (List<FeedBack>)_service.GetAllFeedBacks();
            ViewBag.avg = _service.getAverage();
            return View(feedBacks);
        }

        public IActionResult Search(string query)
        {
            var q = from feedback in _service.GetAllFeedBacks()
                    where feedback.Name.Contains(query)||
                    feedback.FeedbackContent.Contains(query)
                    select feedback;
            return PartialView(q);
        }

        // GET: FeedBacks/Details/5
        public IActionResult Details(int id)
        {
            var feedBack = _service.GetFeedBack(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // GET: FeedBacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Score,Name,FeedbackContent")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                _service.CreateFeedBack(feedBack);
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Edit/5
        public IActionResult Edit(int id)
        {


            var feedBack = _service.GetFeedBack(id);
            if (feedBack == null)
            {
                return NotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Score,Name,FeedbackContent")] FeedBack feedBack)
        {
            if (id != feedBack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateFeedBack(id,feedBack.Score, feedBack.Name, feedBack.FeedbackContent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedBackExists(feedBack.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public IActionResult Delete(int id)
        {

            var feedBack = _service.GetFeedBack(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteFeedBack(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FeedBackExists(int id)
        {
            return _service.GetAllFeedBacks().Any(e => e.Id == id);
        }
    }
}

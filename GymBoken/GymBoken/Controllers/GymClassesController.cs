using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymBoken.Models;

namespace GymBoken.Controllers
{
	public class GymClassesController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: GymClasses
		public ActionResult Index()
		{
			var newClasses = db.GymClasses.ToList().Where(g => g.EndTime >= DateTime.Now).ToList();
			return View(newClasses);
		}

		public ActionResult History(int? id)
		{
			var oldClasses = db.GymClasses.ToList().Where(g => g.EndTime < DateTime.Now).ToList();
			if (id == null)
			{
				return View(oldClasses);
			}
			else if (User.Identity.IsAuthenticated)
			{
				var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
				return View(oldClasses.Where(g => g.AttendingMembers.Contains(user)).ToList());
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult CurrentBookings()
		{
			var myUser = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
			var myClasses = db.GymClasses.ToList().Where(g => g.EndTime >= DateTime.Now && g.AttendingMembers.Contains(myUser)).ToList();
			return View(myClasses);
		}

		// GET: GymClasses/Details/5
		[Authorize]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			GymClass gymClass = db.GymClasses.Find(id);
			if (gymClass == null)
			{
				return HttpNotFound();
			}
			return View(gymClass);
		}

		// GET: GymClasses/Create
		[Authorize(Roles ="Admin")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: GymClasses/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
		{
			if (ModelState.IsValid)
			{
				db.GymClasses.Add(gymClass);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(gymClass);
		}

		[Authorize]
		public ActionResult BookingToggle(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			GymClass CurrentClass = db.GymClasses.Where(m => m.Id == id).FirstOrDefault();
			ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

			if (CurrentClass.AttendingMembers.Contains(CurrentUser))
			{
				CurrentClass.AttendingMembers.Remove(CurrentUser);
				db.SaveChanges();
			}
			else
			{
				CurrentClass.AttendingMembers.Add(CurrentUser);
				db.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		// GET: GymClasses/Edit/5
		[Authorize(Roles ="Admin")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			GymClass gymClass = db.GymClasses.Find(id);
			if (gymClass == null)
			{
				return HttpNotFound();
			}
			return View(gymClass);
		}

		// POST: GymClasses/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
		{
			if (ModelState.IsValid)
			{
				db.Entry(gymClass).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(gymClass);
		}

		// GET: GymClasses/Delete/5
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			GymClass gymClass = db.GymClasses.Find(id);
			if (gymClass == null)
			{
				return HttpNotFound();
			}
			return View(gymClass);
		}

		// POST: GymClasses/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			GymClass gymClass = db.GymClasses.Find(id);
			db.GymClasses.Remove(gymClass);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

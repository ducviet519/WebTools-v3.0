using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebTools.Models;
using System.Linq;
using WebTools.Services;
using WebTools.Context;

namespace WebTools.Controllers
{
    public class ReportController : Controller
    {
        
        private readonly DatabaseContext _context;
        private readonly IReportListServices _reportpListServices;

        public ReportController(DatabaseContext context, IReportListServices reportpListServices)
        {
            _context = context;
            _reportpListServices = reportpListServices;
        }

        public IActionResult Index()
        {
            List<ReportList> reportList = _reportpListServices.GetReportItems();// _context.ReportLists.ToList();
            return View(reportList);
        }

        public IActionResult Create()
        {
            ReportList reportList = new ReportList();
            return View(reportList);
        }

        [HttpPost]
        public IActionResult Create(ReportList reportList)
        {
            try
            {
                _context.ReportLists.Add(reportList);
                _context.SaveChanges();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            ReportList reportList = GetReportList(id);
            return View(reportList);
        }

        public IActionResult Edit(string id)
        {
            ReportList reportList = GetReportList(id);
            return View(reportList);
        }
        [HttpPost]
        public IActionResult Edit(ReportList reportList)
        {
            try
            {
                _context.ReportLists.Attach(reportList);
                _context.Entry(reportList).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Delete(string id)
        {
            ReportList reportList = GetReportList(id);
            return View(reportList);
        }
        [HttpPost]
        public IActionResult Delete(ReportList reportList)
        {
            try
            {
                _context.ReportLists.Attach(reportList);
                _context.Entry(reportList).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }


        private ReportList GetReportList(string id)
        {
            ReportList reportList = _context.ReportLists.Where(u => u.IDBieuMau == id).FirstOrDefault();
            return reportList;
        }
    }
}

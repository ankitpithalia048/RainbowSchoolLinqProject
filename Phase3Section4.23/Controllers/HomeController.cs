using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phase3Section4._23.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Phase3Section4._23.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult StudentListView()
        {
            StudentDAL dal = new StudentDAL();
            List<Student> students = (List<Student>)dal.GetAllStudents();
            ViewData["students"] = students;

            SubjectDAL dal2 = new SubjectDAL();
            List<Subject> subjects = (List<Subject>)dal2.GetAllSubjects();

            MarksDAL dal3 = new MarksDAL();
            List<Marks> marks = (List<Marks>)dal3.GetAllMarks();


            var TotalStudent = students.Count();
            ViewData["TotalStudents"] = TotalStudent;

            var TotalSubjects = subjects.Count();
            ViewData["TotalSubjects"] = TotalSubjects;

            //MiniMarks in each Subject
            var innerjoin = (from st in marks
                             join sub in subjects on st.Subject_ID equals sub.ID
                             select new
                             {

                                 Subject = sub.Name,
                                 Marks = st.Value
                             }).ToList();
            var MinMarks = (from ij in innerjoin
                            group ij by ij.Subject into ijGroup
                            select new
                            {
                                Subject = ijGroup.Key,
                                MinMarks = ijGroup.Min(x => x.Marks)
                            });

            ViewData["MiniMumMarks"] = MinMarks;

            //MaximumMarks in each Subject

            var MaxMarks = (from ij in innerjoin
                            group ij by ij.Subject into ijGroup
                            select new
                            {
                                Subject = ijGroup.Key,
                                MaxMarks = ijGroup.Max(x => x.Marks)
                            });

            ViewData["MaxMarks"] = MaxMarks;

            //Avergae in each Subject

            var AvgMarks = (from ij in innerjoin
                            group ij by ij.Subject into ijGroup
                            select new
                            {

                                Subject = ijGroup.Key,
                                AvgMarks = ijGroup.Average(x => x.Marks)
                            });

            ViewData["AvgMarks"] = AvgMarks;

           

            var crossjoin = (from st in students
                             from sub in subjects

                             select new
                             {
                                 st.ID,
                                 st.Name,
                                 st.Email,
                                 st.Address,
                                 st.Class,
                                 Subject = sub.Name,
                                 Subject_ID = sub.ID
                             }).ToList();
            var mark = (from m in marks
                        from cs in crossjoin
                        where m.Student_ID == cs.ID && m.Subject_ID == cs.Subject_ID
                        select new
                        {
                            cs.ID,
                            cs.Name,
                            cs.Email,
                            cs.Address,
                            cs.Class,
                            cs.Subject_ID,
                            Marks = m.Value,
                            cs.Subject
                        }

                        ).ToList();

            ViewData["crossjoin"] = mark;
            
            
            //Highestmarks

            var stud = from s in mark
                       group s by s.Subject into stugrp
                       let topp = stugrp.Max(x => x.Marks)
                       select new
                       {
                           Subject = stugrp.Key,
                           TopStudent = stugrp.First(y => y.Marks == topp).ID,
                           MaximumMarks = topp
                       }
                    ;

            var HighestMarks = (from h in mark
                               from st in stud
                                where h.ID == st.TopStudent && h.Subject == st.Subject
                               select new
                               {
                                   Name = h.Name,
                                   Email = h.Email,
                                   Address = h.Address,
                                   Subject = st.Subject,
                                   Marks = st.MaximumMarks
                               }).ToList();

            ViewData["HighestMarks"] = HighestMarks;

            //Lowest Marks
            var Loweststud = from s in mark
                       group s by s.Subject into Loweststudgrp
                             let low = Loweststudgrp.Min(x => x.Marks)
                       select new
                       {
                           Subject = Loweststudgrp.Key,
                           TopStudent = Loweststudgrp.First(y => y.Marks == low).ID,
                           LowestMarks = low
                       }
                    ;

            var lowestMarks = (from h in mark
                                from st in Loweststud
                               where h.ID == st.TopStudent && h.Subject == st.Subject
                                select new
                                {
                                    Name = h.Name,
                                    Email = h.Email,
                                    Address = h.Address,
                                    Subject = st.Subject,
                                    Marks = st.LowestMarks
                                }).ToList();

            ViewData["lowestMarks"] = lowestMarks;



            return View();
        }


    }
}

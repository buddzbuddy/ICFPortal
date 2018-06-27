using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using CISSAPortal.Models;

namespace CISSAPortal.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Patient);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            var model = new QuestionViewModel
            {
                Question = question
            };

            model.Answers1 = db.Answers1.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers2 = db.Answers2.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers3 = db.Answers3.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers4 = db.Answers4.Where(x => x.QuestionId == question.Id).ToList();

            ViewBag.Answer1Values = db.Answer1Values.ToList();
            ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
            ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
            ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
            ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
            ViewBag.Answer4Values = db.Answer4Values.ToList();
            return View(model);
        }
        public ActionResult DetailsOnlyTable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            var model = new QuestionViewModel
            {
                Question = question
            };

            model.Answers1 = db.Answers1.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers2 = db.Answers2.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers3 = db.Answers3.Where(x => x.QuestionId == question.Id).ToList();
            model.Answers4 = db.Answers4.Where(x => x.QuestionId == question.Id).ToList();

            ViewBag.Answer1Values = db.Answer1Values.ToList();
            ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
            ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
            ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
            ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
            ViewBag.Answer4Values = db.Answer4Values.ToList();
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult CreateWithModel(int patientId)
        {
            var patient = db.Patients.Find(patientId);
            if (patient == null)
                throw new ApplicationException("Пациент не найден!");
            var newQuestion = new Question { PatientId = patientId, Patient = patient };

            var codes1 = db.Codes1.ToList();
            var codes2 = db.Codes2.ToList();
            var codes3 = db.Codes3.ToList();
            var codes4 = db.Codes4.ToList();

            var answers1 = new List<Answer1>();
            var answers2 = new List<Answer2>();
            var answers3 = new List<Answer3>();
            var answers4 = new List<Answer4>();

            foreach(var code in codes1)
            {
                answers1.Add(new Answer1 { CodeId = code.Id, Code = code, Question = newQuestion });
            }
            foreach (var code in codes2)
            {
                answers2.Add(new Answer2 { CodeId = code.Id, Code = code, Question = newQuestion });
            }
            foreach (var code in codes3)
            {
                answers3.Add(new Answer3 { CodeId = code.Id, Code = code, Question = newQuestion });
            }
            foreach (var code in codes4)
            {
                answers4.Add(new Answer4 { CodeId = code.Id, Code = code, Question = newQuestion });
            }

            var model = new QuestionViewModel
            {
                Question = newQuestion
            };

            model.Answers1 = answers1;
            model.Answers2 = answers2;
            model.Answers3 = answers3;
            model.Answers4 = answers4;

            ViewBag.Answer1Values = db.Answer1Values.ToList();
            ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
            ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
            ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
            ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
            ViewBag.Answer4Values = db.Answer4Values.ToList();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult CreateWithModel(FormCollection collection)
        {
            var patientId = int.Parse(collection["Question.PatientId"]);
            var patient = db.Patients.Find(patientId);
            var newQuestion = new Question { PatientId = patientId, Patient = patient };

            var codes1 = db.Codes1.ToList();
            var codes2 = db.Codes2.ToList();
            var codes3 = db.Codes3.ToList();
            var codes4 = db.Codes4.ToList();
            

            var answers1 = new List<Answer1>();
            var answers2 = new List<Answer2>();
            var answers3 = new List<Answer3>();
            var answers4 = new List<Answer4>();

            foreach (var code in codes1)
            {
                var codeVal = "";
                if (collection.AllKeys.Contains(code.Name))
                    codeVal = collection[code.Name];
                int valId = 0;
                if (!string.IsNullOrEmpty(codeVal))
                {
                    valId = int.Parse(codeVal);
                }
                else
                    ModelState.AddModelError(code.Name, "Значение для кода \"" + code.Name + "\" не указан.");
                var sourceIsCaseHistory = bool.Parse(collection[code.Name + "_SourceIsCaseHistory"].Split(',')[0]);
                var sourceIsPatientAnswers = bool.Parse(collection[code.Name + "_SourceIsPatientAnswers"].Split(',')[0]);
                var sourceIsClinicalExamination = bool.Parse(collection[code.Name + "_SourceIsClinicalExamination"].Split(',')[0]);
                var sourceIsTechnicalSurvey = bool.Parse(collection[code.Name + "_SourceIsTechnicalSurvey"].Split(',')[0]);
                var problemDescription = collection[code.Name + "_ProblemDescription"];
                answers1.Add(new Answer1
                {
                    CodeId = code.Id,
                    Code = code,
                    Question = newQuestion,
                    ValueId = valId,
                    ProblemDescription = problemDescription,
                    SourceIsCaseHistory = sourceIsCaseHistory,
                    SourceIsPatientAnswers = sourceIsPatientAnswers,
                    SourceIsClinicalExamination = sourceIsClinicalExamination,
                    SourceIsTechnicalSurvey = sourceIsTechnicalSurvey
                });
            }
            foreach (var code in codes2)
            {
                var codeVal1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1"))
                    codeVal1 = collection[code.Name + "_v1"];
                int valId1 = 0;
                if (!string.IsNullOrEmpty(codeVal1))
                {
                    valId1 = int.Parse(codeVal1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1", "Значение для кода \"" + code.Name + "_v1" + "\" не указан.");
                var codeVal2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2"))
                    codeVal2 = collection[code.Name + "_v2"];
                int valId2 = 0;
                if (!string.IsNullOrEmpty(codeVal2))
                {
                    valId2 = int.Parse(codeVal2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2", "Значение для кода \"" + code.Name + "_v2" + "\" не указан.");
                var sourceIsCaseHistory = bool.Parse(collection[code.Name + "_SourceIsCaseHistory"].Split(',')[0]);
                var sourceIsPatientAnswers = bool.Parse(collection[code.Name + "_SourceIsPatientAnswers"].Split(',')[0]);
                var sourceIsClinicalExamination = bool.Parse(collection[code.Name + "_SourceIsClinicalExamination"].Split(',')[0]);
                var sourceIsTechnicalSurvey = bool.Parse(collection[code.Name + "_SourceIsTechnicalSurvey"].Split(',')[0]);
                var problemDescription = collection[code.Name + "_ProblemDescription"];
                answers2.Add(new Answer2
                {
                    CodeId = code.Id,
                    Code = code,
                    Question = newQuestion,
                    ValueId1 = valId1,
                    ValueId2 = valId2,
                    ProblemDescription = problemDescription,
                    SourceIsCaseHistory = sourceIsCaseHistory,
                    SourceIsPatientAnswers = sourceIsPatientAnswers,
                    SourceIsClinicalExamination = sourceIsClinicalExamination,
                    SourceIsTechnicalSurvey = sourceIsTechnicalSurvey
                });
            }
            foreach (var code in codes3)
            {
                var codeVal1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1"))
                    codeVal1 = collection[code.Name + "_v1"];
                int valId1 = 0;
                if (!string.IsNullOrEmpty(codeVal1))
                {
                    valId1 = int.Parse(codeVal1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1", "Значение для кода \"" + code.Name + "_v1" + "\" не указан.");
                var codeVal2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2"))
                    codeVal2 = collection[code.Name + "_v2"];
                int valId2 = 0;
                if (!string.IsNullOrEmpty(codeVal2))
                {
                    valId2 = int.Parse(codeVal2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2", "Значение для кода \"" + code.Name + "_v2" + "\" не указан.");
                var sourceIsCaseHistory = bool.Parse(collection[code.Name + "_SourceIsCaseHistory"].Split(',')[0]);
                var sourceIsPatientAnswers = bool.Parse(collection[code.Name + "_SourceIsPatientAnswers"].Split(',')[0]);
                var sourceIsClinicalExamination = bool.Parse(collection[code.Name + "_SourceIsClinicalExamination"].Split(',')[0]);
                var sourceIsTechnicalSurvey = bool.Parse(collection[code.Name + "_SourceIsTechnicalSurvey"].Split(',')[0]);
                var problemDescription = collection[code.Name + "_ProblemDescription"];
                answers3.Add(new Answer3
                {
                    CodeId = code.Id,
                    Code = code,
                    Question = newQuestion,
                    ValueId1 = valId1,
                    ValueId2 = valId2,
                    ProblemDescription = problemDescription,
                    SourceIsCaseHistory = sourceIsCaseHistory,
                    SourceIsPatientAnswers = sourceIsPatientAnswers,
                    SourceIsClinicalExamination = sourceIsClinicalExamination,
                    SourceIsTechnicalSurvey = sourceIsTechnicalSurvey
                });
            }
            foreach (var code in codes4)
            {
                var codeVal = "";
                if (collection.AllKeys.Contains(code.Name))
                    codeVal = collection[code.Name];
                int valId = 0;
                if (!string.IsNullOrEmpty(codeVal))
                {
                    valId = int.Parse(codeVal);
                }
                else
                    ModelState.AddModelError(code.Name, "Значение для кода \"" + code.Name + "\" не указан.");
                var sourceIsCaseHistory = bool.Parse(collection[code.Name + "_SourceIsCaseHistory"].Split(',')[0]);
                var sourceIsPatientAnswers = bool.Parse(collection[code.Name + "_SourceIsPatientAnswers"].Split(',')[0]);
                var sourceIsClinicalExamination = bool.Parse(collection[code.Name + "_SourceIsClinicalExamination"].Split(',')[0]);
                var sourceIsTechnicalSurvey = bool.Parse(collection[code.Name + "_SourceIsTechnicalSurvey"].Split(',')[0]);
                var problemDescription = collection[code.Name + "_ProblemDescription"];
                answers4.Add(new Answer4
                {
                    CodeId = code.Id,
                    Code = code,
                    Question = newQuestion,
                    ValueId = valId,
                    ProblemDescription = problemDescription,
                    SourceIsCaseHistory = sourceIsCaseHistory,
                    SourceIsPatientAnswers = sourceIsPatientAnswers,
                    SourceIsClinicalExamination = sourceIsClinicalExamination,
                    SourceIsTechnicalSurvey = sourceIsTechnicalSurvey
                });
            }
            if (ModelState.IsValid)
            {
                db.Questions.Add(newQuestion);
                db.SaveChanges();
                answers1.ForEach(x =>
                {
                    x.QuestionId = newQuestion.Id;
                });
                answers2.ForEach(x =>
                {
                    x.QuestionId = newQuestion.Id;
                });
                answers3.ForEach(x =>
                {
                    x.QuestionId = newQuestion.Id;
                });
                answers4.ForEach(x =>
                {
                    x.QuestionId = newQuestion.Id;
                });
                db.Answers1.AddRange(answers1);
                db.Answers2.AddRange(answers2);
                db.Answers3.AddRange(answers3);
                db.Answers4.AddRange(answers4);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var model = new QuestionViewModel
            {
                Question = newQuestion
            };

            model.Answers1 = answers1;
            model.Answers2 = answers2;
            model.Answers3 = answers3;
            model.Answers4 = answers4;

            ViewBag.Answer1Values = db.Answer1Values.ToList();
            ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
            ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
            ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
            ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
            ViewBag.Answer4Values = db.Answer4Values.ToList();

            return View(model);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName", question.PatientId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PatientId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "Id", "LastName", question.PatientId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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

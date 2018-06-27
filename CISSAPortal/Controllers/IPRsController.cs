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
    public class IPRsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IPRs
        public ActionResult Index()
        {
            var iprs = db.Iprs.Include(i => i.ParentCase);
            return View(iprs.ToList());
        }

        // GET: IPRs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IPR iPR = db.Iprs.Find(id);
            if (iPR == null)
            {
                return HttpNotFound();
            }
            return View(iPR);
        }

        // GET: IPRs/Create
        public ActionResult Create(int patientId)
        {
            var parentCase = db.ParentCases.Include(x => x.Patient).FirstOrDefault(x => x.PatientId == patientId);
            var ipr = new IPR { ParentCaseId = parentCase.Id, ParentCase = parentCase };
            var targets = db.RehabilitationTargets.ToList();
            targets.Insert(0, new RehabilitationTarget());
            ViewBag.TargetSelected = new MultiSelectList(targets, "Id", "Name");
            var model = new CreateIPRViewModel { IPR = ipr };
            return View(model);
        }

        // POST: IPRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            var parentCaseId = int.Parse(collection["IPR.ParentCaseId"]);
            var parentCase = db.ParentCases.Include(x => x.Patient).FirstOrDefault(x => x.Id == parentCaseId);
            var ipr = new IPR { ParentCaseId = parentCaseId, ParentCase = parentCase };
            var model = new CreateIPRViewModel { IPR = ipr, Targets = new List<RehabilitationTargetToIPR>() };
            if (collection.AllKeys.Contains("TargetSelected"))
            {
                var TargetSelectedStr = collection["TargetSelected"];
                if (string.IsNullOrEmpty(TargetSelectedStr))
                {
                    ModelState.AddModelError("TargetSelected", "Это поле обязательное!");
                }
                else
                    model.TargetSelected = TargetSelectedStr.Split(',');
            }
            else
                ModelState.AddModelError("TargetSelected", "Это поле обязательное!");
            if (collection.AllKeys.Contains("IPR.StartDate"))
            {
                var startDateStr = collection["IPR.StartDate"];
                DateTime startDate = DateTime.MinValue;
                if (!DateTime.TryParse(startDateStr, out startDate))
                {
                    ModelState.AddModelError("IPR.StartDate", "Это поле обязательное!");
                }
                else
                    model.IPR.StartDate = startDate;
            }
            if (collection.AllKeys.Contains("IPR.EndDate"))
            {
                var endDateStr = collection["IPR.EndDate"];
                DateTime endDate = DateTime.MinValue;
                if (!DateTime.TryParse(endDateStr, out endDate))
                {
                    ModelState.AddModelError("IPR.EndDate", "Это поле обязательное!");
                }
                else
                    model.IPR.EndDate = endDate;
            }
            if (collection.AllKeys.Contains("IPR.MedicalActNo"))
            {
                var medicalActNoStr = collection["IPR.MedicalActNo"];
                if (string.IsNullOrEmpty(medicalActNoStr))
                {
                    ModelState.AddModelError("IPR.MedicalActNo", "Это поле обязательное!");
                }
                else
                    model.IPR.MedicalActNo = medicalActNoStr;
            }
            if (collection.AllKeys.Contains("IPR.NextDateExamination"))
            {
                var NextDateExaminationStr = collection["IPR.NextDateExamination"];
                DateTime NextDateExamination = DateTime.MinValue;
                if (!DateTime.TryParse(NextDateExaminationStr, out NextDateExamination))
                {
                    ModelState.AddModelError("IPR.NextDateExamination", "Это поле обязательное!");
                }
                else
                    model.IPR.NextDateExamination = NextDateExamination;
            }
            if (ModelState.IsValid)
            {
                //db.Iprs.Add(model.IPR);
                //db.SaveChanges();
                var targetNo = 1;
                foreach(var targetId in model.TargetSelected.Select(x => int.Parse(x)))
                {
                    model.Targets.Add(new RehabilitationTargetToIPR
                    {
                        RehabilitationTarget = db.RehabilitationTargets.Find(targetId),
                        TargetId = targetId,
                        No = targetNo
                    });
                    targetNo++;
                }

                var question = db.Questions.FirstOrDefault(x => x.PatientId == model.IPR.ParentCase.PatientId);
                if (question == null)
                {
                    return HttpNotFound("Question not found!");
                }
                model.QuestionViewModel = new QuestionViewModel
                {
                    Question = question
                };

                model.QuestionViewModel.Answers1 = db.Answers1.Where(x => x.QuestionId == question.Id).ToList();
                model.QuestionViewModel.Answers2 = db.Answers2.Where(x => x.QuestionId == question.Id).ToList();
                model.QuestionViewModel.Answers3 = db.Answers3.Where(x => x.QuestionId == question.Id).ToList();
                model.QuestionViewModel.Answers4 = db.Answers4.Where(x => x.QuestionId == question.Id).ToList();

                ViewBag.Answer1Values = db.Answer1Values.ToList();
                ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
                ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
                ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
                ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
                ViewBag.Answer4Values = db.Answer4Values.ToList();

                model.AnswerTargets1 = new List<AnswerTarget1>();
                model.AnswerTargets2 = new List<AnswerTarget2>();
                model.AnswerTargets3 = new List<AnswerTarget3>();
                model.AnswerTargets4 = new List<AnswerTarget4>();

                model.SelectedTargets1 = new List<SelectedTarget1>();
                model.SelectedTargets2 = new List<SelectedTarget2>();
                model.SelectedTargets3 = new List<SelectedTarget3>();
                model.SelectedTargets4 = new List<SelectedTarget4>();

                model.SelectedServices1 = new List<SelectedService1>();
                model.SelectedServices2 = new List<SelectedService2>();
                model.SelectedServices3 = new List<SelectedService3>();
                model.SelectedServices4 = new List<SelectedService4>();

                model.ServiceTypes = db.ServiceTypes.ToList();
                
                return View("CreateMain", model);
            }
            var targets = db.RehabilitationTargets.ToList();
            targets.Insert(0, new RehabilitationTarget());
            ViewBag.TargetSelected = new MultiSelectList(targets, "Id", "Name", model.TargetSelected);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateMain(FormCollection collection)
        {
            var parentCaseId = int.Parse(collection["IPR.ParentCaseId"]);
            var parentCase = db.ParentCases.Include(x => x.Patient).FirstOrDefault(x => x.Id == parentCaseId);
            var ipr = new IPR { ParentCaseId = parentCaseId, ParentCase = parentCase };
            var model = new CreateIPRViewModel { IPR = ipr, Targets = new List<RehabilitationTargetToIPR>() };
            if (collection.AllKeys.Contains("TargetSelected"))
            {
                var TargetSelectedStr = collection["TargetSelected"];
                if (string.IsNullOrEmpty(TargetSelectedStr))
                {
                    ModelState.AddModelError("TargetSelected", "Это поле обязательное!");
                }
                else
                    model.TargetSelected = TargetSelectedStr.Split(',');
            }
            else
                ModelState.AddModelError("TargetSelected", "Это поле обязательное!");
            if (collection.AllKeys.Contains("IPR.StartDate"))
            {
                var startDateStr = collection["IPR.StartDate"];
                DateTime startDate = DateTime.MinValue;
                if (!DateTime.TryParse(startDateStr, out startDate))
                {
                    ModelState.AddModelError("IPR.StartDate", "Это поле обязательное!");
                }
                else
                    model.IPR.StartDate = startDate;
            }
            if (collection.AllKeys.Contains("IPR.EndDate"))
            {
                var endDateStr = collection["IPR.EndDate"];
                DateTime endDate = DateTime.MinValue;
                if (!DateTime.TryParse(endDateStr, out endDate))
                {
                    ModelState.AddModelError("IPR.EndDate", "Это поле обязательное!");
                }
                else
                    model.IPR.EndDate = endDate;
            }
            if (collection.AllKeys.Contains("IPR.MedicalActNo"))
            {
                var medicalActNoStr = collection["IPR.MedicalActNo"];
                if (string.IsNullOrEmpty(medicalActNoStr))
                {
                    ModelState.AddModelError("IPR.MedicalActNo", "Это поле обязательное!");
                }
                else
                    model.IPR.MedicalActNo = medicalActNoStr;
            }
            if (collection.AllKeys.Contains("IPR.NextDateExamination"))
            {
                var NextDateExaminationStr = collection["IPR.NextDateExamination"];
                DateTime NextDateExamination = DateTime.MinValue;
                if (!DateTime.TryParse(NextDateExaminationStr, out NextDateExamination))
                {
                    ModelState.AddModelError("IPR.NextDateExamination", "Это поле обязательное!");
                }
                else
                    model.IPR.NextDateExamination = NextDateExamination;
            }

            var codes1 = db.Codes1.ToList();
            var codes2 = db.Codes2.ToList();
            var codes3 = db.Codes3.ToList();
            var codes4 = db.Codes4.ToList();


            var answers1 = new List<AnswerTarget1>();
            var answers2 = new List<AnswerTarget2>();
            var answers3 = new List<AnswerTarget3>();
            var answers4 = new List<AnswerTarget4>();

            var selectedTargets1 = new List<SelectedTarget1>();
            var selectedTargets2 = new List<SelectedTarget2>();
            var selectedTargets3 = new List<SelectedTarget3>();
            var selectedTargets4 = new List<SelectedTarget4>();

            var selectedServices1 = new List<SelectedService1>();
            var selectedServices2 = new List<SelectedService2>();
            var selectedServices3 = new List<SelectedService3>();
            var selectedServices4 = new List<SelectedService4>();

            foreach (var code in codes1)
            {
                //parsing the selected target values
                var codeVal = "";
                if (collection.AllKeys.Contains(code.Name + "_target"))
                    codeVal = collection[code.Name + "_target"];
                int valId = 0;
                if (!string.IsNullOrEmpty(codeVal))
                {
                    valId = int.Parse(codeVal);
                }
                else
                    ModelState.AddModelError(code.Name + "_target", "Значение цели для кода \"" + code.Name + "\" не указано.");
                answers1.Add(new AnswerTarget1
                {
                    CodeId = code.Id,
                    Code = code,
                    ValueId = valId,
                    IPR = model.IPR
                });

                //parsing the selected target no
                var codeTarget = "";
                if (collection.AllKeys.Contains(code.Name + "_target_id"))
                    codeTarget = collection[code.Name + "_target_id"];
                int codeTargetValId = 0;
                if (!string.IsNullOrEmpty(codeTarget))
                {
                    codeTargetValId = int.Parse(codeTarget);
                }
                else
                    ModelState.AddModelError(code.Name + "_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                selectedTargets1.Add(new SelectedTarget1
                {
                    CodeId = code.Id,
                    Code = code,
                    TargetId = codeTargetValId,
                    IPR = model.IPR
                });

                //parsing the selected service
                var codeService = "";
                if (collection.AllKeys.Contains(code.Name + "_service_id"))
                    codeService = collection[code.Name + "_service_id"];
                int[] codeServiceValIds = { 0 };
                if (!string.IsNullOrEmpty(codeService))
                {
                    codeServiceValIds = codeService.Split(',').Select(x => int.Parse(x)).ToArray();
                }
                else
                    ModelState.AddModelError(code.Name + "_service_id", "Услуга для кода \"" + code.Name + "\" не указан.");
                foreach (var codeServiceValId in codeServiceValIds)
                    selectedServices1.Add(new SelectedService1
                    {
                        CodeId = code.Id,
                        Code = code,
                        ServiceTypeId = codeServiceValId,
                        IPR = model.IPR
                    });
            }
            foreach (var code in codes2)
            {
                var codeVal1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1_target"))
                    codeVal1 = collection[code.Name + "_v1_target"];
                int valId1 = 0;
                if (!string.IsNullOrEmpty(codeVal1))
                {
                    valId1 = int.Parse(codeVal1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1_target", "Значение цели для кода \"" + code.Name + "_v1" + "\" не указан.");
                var codeVal2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2_target"))
                    codeVal2 = collection[code.Name + "_v2_target"];
                int valId2 = 0;
                if (!string.IsNullOrEmpty(codeVal2))
                {
                    valId2 = int.Parse(codeVal2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2_target", "Значение цели для кода \"" + code.Name + "_v2" + "\" не указан.");
                answers2.Add(new AnswerTarget2
                {
                    CodeId = code.Id,
                    Code = code,
                    ValueId1 = valId1,
                    ValueId2 = valId2,
                    IPR = model.IPR
                });

                //parsing the selected targets
                var codeTarget1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1_target_id"))
                    codeTarget1 = collection[code.Name + "_v1_target_id"];
                int codeTargetValId1 = 0;
                if (!string.IsNullOrEmpty(codeTarget1))
                {
                    codeTargetValId1 = int.Parse(codeTarget1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                var codeTarget2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2_target_id"))
                    codeTarget2 = collection[code.Name + "_v2_target_id"];
                int codeTargetValId2 = 0;
                if (!string.IsNullOrEmpty(codeTarget2))
                {
                    codeTargetValId2 = int.Parse(codeTarget2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                selectedTargets2.Add(new SelectedTarget2
                {
                    CodeId = code.Id,
                    Code = code,
                    TargetId1 = codeTargetValId1,
                    TargetId2 = codeTargetValId2,
                    IPR = model.IPR
                });
            }
            foreach (var code in codes3)
            {
                var codeVal1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1_target"))
                    codeVal1 = collection[code.Name + "_v1_target"];
                int valId1 = 0;
                if (!string.IsNullOrEmpty(codeVal1))
                {
                    valId1 = int.Parse(codeVal1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1_target", "Значение цели для кода \"" + code.Name + "_v1" + "\" не указан.");
                var codeVal2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2_target"))
                    codeVal2 = collection[code.Name + "_v2_target"];
                int valId2 = 0;
                if (!string.IsNullOrEmpty(codeVal2))
                {
                    valId2 = int.Parse(codeVal2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2_target", "Значение цели для кода \"" + code.Name + "_v2" + "\" не указан.");
                answers3.Add(new AnswerTarget3
                {
                    CodeId = code.Id,
                    Code = code,
                    ValueId1 = valId1,
                    ValueId2 = valId2,
                    IPR = model.IPR
                });

                //parsing the selected targets
                var codeTarget1 = "";
                if (collection.AllKeys.Contains(code.Name + "_v1_target_id"))
                    codeTarget1 = collection[code.Name + "_v1_target_id"];
                int codeTargetValId1 = 0;
                if (!string.IsNullOrEmpty(codeTarget1))
                {
                    codeTargetValId1 = int.Parse(codeTarget1);
                }
                else
                    ModelState.AddModelError(code.Name + "_v1_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                var codeTarget2 = "";
                if (collection.AllKeys.Contains(code.Name + "_v2_target_id"))
                    codeTarget2 = collection[code.Name + "_v2_target_id"];
                int codeTargetValId2 = 0;
                if (!string.IsNullOrEmpty(codeTarget2))
                {
                    codeTargetValId2 = int.Parse(codeTarget2);
                }
                else
                    ModelState.AddModelError(code.Name + "_v2_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                selectedTargets3.Add(new SelectedTarget3
                {
                    CodeId = code.Id,
                    Code = code,
                    TargetId1 = codeTargetValId1,
                    TargetId2 = codeTargetValId2,
                    IPR = model.IPR
                });
            }
            foreach (var code in codes4)
            {
                var codeVal = "";
                if (collection.AllKeys.Contains(code.Name + "_target"))
                    codeVal = collection[code.Name + "_target"];
                int valId = 0;
                if (!string.IsNullOrEmpty(codeVal))
                {
                    valId = int.Parse(codeVal);
                }
                else
                    ModelState.AddModelError(code.Name + "_target", "Значение цели для кода \"" + code.Name + "\" не указан.");
                answers4.Add(new AnswerTarget4
                {
                    CodeId = code.Id,
                    Code = code,
                    ValueId = valId,
                    IPR = model.IPR
                });


                //parsing the selected targets
                var codeTarget = "";
                if (collection.AllKeys.Contains(code.Name + "_target_id"))
                    codeTarget = collection[code.Name + "_target_id"];
                int codeTargetValId = 0;
                if (!string.IsNullOrEmpty(codeTarget))
                {
                    codeTargetValId = int.Parse(codeTarget);
                }
                else
                    ModelState.AddModelError(code.Name + "_target_id", "Номер цели для кода \"" + code.Name + "\" не указан.");
                selectedTargets4.Add(new SelectedTarget4
                {
                    CodeId = code.Id,
                    Code = code,
                    TargetId = codeTargetValId,
                    IPR = model.IPR
                });
            }
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("", "success");
            }
            model.AnswerTargets1 = answers1;
            model.AnswerTargets2 = answers2;
            model.AnswerTargets3 = answers3;
            model.AnswerTargets4 = answers4;

            model.SelectedTargets1 = selectedTargets1;
            model.SelectedTargets2 = selectedTargets2;
            model.SelectedTargets3 = selectedTargets3;
            model.SelectedTargets4 = selectedTargets4;

            model.SelectedServices1 = selectedServices1;

            var targetNo = 1;
            foreach (var targetId in model.TargetSelected.Select(x => int.Parse(x)))
            {
                model.Targets.Add(new RehabilitationTargetToIPR
                {
                    RehabilitationTarget = db.RehabilitationTargets.Find(targetId),
                    TargetId = targetId,
                    No = targetNo
                });
                targetNo++;
            }

            var question = db.Questions.FirstOrDefault(x => x.PatientId == model.IPR.ParentCase.PatientId);
            if (question == null)
            {
                return HttpNotFound("Question not found!");
            }
            model.QuestionViewModel = new QuestionViewModel
            {
                Question = question
            };

            model.QuestionViewModel.Answers1 = db.Answers1.Where(x => x.QuestionId == question.Id).ToList();
            model.QuestionViewModel.Answers2 = db.Answers2.Where(x => x.QuestionId == question.Id).ToList();
            model.QuestionViewModel.Answers3 = db.Answers3.Where(x => x.QuestionId == question.Id).ToList();
            model.QuestionViewModel.Answers4 = db.Answers4.Where(x => x.QuestionId == question.Id).ToList();

            ViewBag.Answer1Values = db.Answer1Values.ToList();
            ViewBag.Answer2Values1 = db.Answer2Values1.ToList();
            ViewBag.Answer2Values2 = db.Answer2Values2.ToList();
            ViewBag.Answer3Values1 = db.Answer3Values1.ToList();
            ViewBag.Answer3Values2 = db.Answer3Values2.ToList();
            ViewBag.Answer4Values = db.Answer4Values.ToList();

            model.ServiceTypes = db.ServiceTypes.ToList();

            return View(model);
        }

        // GET: IPRs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IPR iPR = db.Iprs.Find(id);
            if (iPR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentCaseId = new SelectList(db.ParentCases, "Id", "FullName", iPR.ParentCaseId);
            return View(iPR);
        }

        // POST: IPRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentCaseId,StartDate,EndDate,MedicalActNo,NextDateExamination")] IPR iPR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iPR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCaseId = new SelectList(db.ParentCases, "Id", "FullName", iPR.ParentCaseId);
            return View(iPR);
        }

        // GET: IPRs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IPR iPR = db.Iprs.Find(id);
            if (iPR == null)
            {
                return HttpNotFound();
            }
            return View(iPR);
        }

        // POST: IPRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IPR iPR = db.Iprs.Find(id);
            db.Iprs.Remove(iPR);
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

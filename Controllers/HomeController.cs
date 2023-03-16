using CrudUsingTempData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CrudUsingTempData.Controllers
{
    public class HomeController : Controller
    {

        #region Variables
        const string TempDataKey = "EmployeeTempData";
        #endregion

        #region Constructor
        public HomeController()
        {
        }
        #endregion

        #region Action Methods
        public IActionResult Index()
        {
            List<EmployeeModel> list = GetListFromTempData();
            return View(list);
        }
        public IActionResult Add(int id)
        {
            List<EmployeeModel> list = GetListFromTempData();
            EmployeeModel employee = new EmployeeModel() { EmployeeId = 0 };
            if (id > 0)
            {
                var emp = list.FirstOrDefault(x => x.EmployeeId == id);
                if (emp != null)
                    employee = emp;
                ViewBag.Title = "Edit Employee";
            }
            else
                ViewBag.Title = "Add Employee";
            return View(employee);
        }
        public IActionResult Save(EmployeeModel model)
        {
            try
            {
                if (model.EmployeeId > 0)
                    ViewBag.Title = "Edit Employee";
                else
                    ViewBag.Title = "Add Employee";
                if (ModelState.IsValid)
                {
                    List<EmployeeModel> list = GetListFromTempData();
                    if (list.Any(x => x.FirstName == model.FirstName && x.LastName == model.LastName && x.EmployeeId != model.EmployeeId))
                    {
                        TempData["ErrorMessage"] = "Employee already exists with this name";
                        return View("Add", model);
                    }
                    if (model.EmployeeId > 0)
                    {
                        int index = list.FindIndex(x => x.EmployeeId == model.EmployeeId);
                        if (index >= 0 && list.Count > index)
                            list[index] = model;
                        TempData["SuccessMessage"] = "Employee Updated Successfully";
                    }
                    else
                    {
                        model.EmployeeId = list.Count > 0 ? list.Max(x => x.EmployeeId) + 1 : 1;
                        list.Add(model);
                        TempData["SuccessMessage"] = "Employee Inserted Successfully";
                    }
                    SetListInTempData(list);
                    return RedirectToAction("Index");
                }
                else
                    return View("Add", model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went worng";
                return View("Add", model);
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                List<EmployeeModel> list = GetListFromTempData();
                var emp = list.FirstOrDefault(x => x.EmployeeId == id);
                if (emp != null)
                {
                    list = list.Where(x => x.EmployeeId != id).ToList();
                    SetListInTempData(list);
                    TempData["SuccessMessage"] = "Employee Deleted Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Employee not exists with this id: " + id;
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went worng";
            }
            return RedirectToAction("Index");
        }
        public string RandomNegativeNumber()
        {
            Random generator = new Random();
            string value = generator.Next(1, 999).ToString();
            return "-" + value;
        }
        #endregion

        #region Temp Data Methods
        public void SetListInTempData(List<EmployeeModel> list)
        {
            TempData[TempDataKey] = null;
            TempData[TempDataKey] = JsonConvert.SerializeObject(list);
            KeepTempData();
        }
        public List<EmployeeModel> GetListFromTempData()
        {
            List<EmployeeModel> list = new List<EmployeeModel>();
            if (TempData[TempDataKey] != null)
            {
                var data = TempData[TempDataKey].ToString();
                KeepTempData();
                list = JsonConvert.DeserializeObject<List<EmployeeModel>>(data);
            }
            return list;
        }
        public void KeepTempData()
        {
            if (TempData[TempDataKey] != null)
                TempData.Keep();
        }
        public void DeleteTempData()
        {
            TempData[TempDataKey] = null;
        }
        #endregion
    }
}
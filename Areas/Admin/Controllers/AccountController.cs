using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using Newtonsoft.Json;
using RealState.Areas.Admin.Models;
using System.Web.Security;
using System.Data;
using Newtonsoft.Json;
using RealState.Security;
using System.Text.RegularExpressions;
//using System.Web.UI.WebControls;

namespace RealState.Areas.Admin.Controllers
{

    public class AccountController : Controller
    {

        ResultModel ResultModel = new ResultModel();
        public string _baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
        // GET: Account
        public ActionResult Index()
        {
            AccountModel obj = new AccountModel();
            return View(obj);
        }
        public async Task<ActionResult> UserList()
        {
            List<AccountModel> model = new List<AccountModel>();
            if (Request.IsAuthenticated)
            {


                using (HttpClient client = new HttpClient())
                {
                    string url = _baseUrl + "Account/AllUser";
                    HttpResponseMessage response = await client.GetAsync(url);
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string strRes = response.Content.ReadAsStringAsync().Result;
                            model = JsonConvert.DeserializeObject<List<AccountModel>>(strRes);
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View("UserList", model);
        }
        [HttpPost]
        public async Task<ActionResult> Login(AccountModel login)
        {

            if (ModelState.IsValid)
            {

                using (HttpClient Client = new HttpClient())
                {
                    string _Url = _baseUrl + "Account/GetUserLogin";
                    HttpResponseMessage response = await Client.PostAsJsonAsync(_Url, login);
                    if (response.IsSuccessStatusCode)
                    {
                        string res = response.Content.ReadAsStringAsync().Result;
                        login = JsonConvert.DeserializeObject<AccountModel>(res);
                    }

                }

                if (login != null)
                {
                    if (login.resultModel.Status)
                    {

                        LoginAccessSecurity(login);

                    }
                    else
                    {
                        TempData["Message"] = login.resultModel.Message;
                        return RedirectToAction("Index");
                    }
                }
                if (login.Role == "A")
                {
                    string Name = login.FirstName + " " + login.LastName;
                    Session["UserName"] = Name;
                    return RedirectToAction("UserList");

                }
                if (login.Role == "U")
                {
                    string Name = login.FirstName + " " + login.LastName;
                    Session["UserName"] = Name;

                    // _loginLogMaintain(Name, User.UserID);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Incorrect UserName or passsword");
            return RedirectToAction("Index", "Account", new { msg = "Incorrect username or password" });
        }

        public async Task<ActionResult> EditUser(long Id)
        {
            AccountModel model = new AccountModel();
            string Url = _baseUrl + "Account/EditUser?id=" + Id + "";
            if (Id != null)
            {
                using (HttpClient client = new HttpClient())
                {


                    HttpResponseMessage response = await client.GetAsync(Url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<AccountModel>(result);
                        return View(model);
                    }
                }
            }
            else
            {
                return RedirectToAction("UserList");
            }

            return RedirectToAction("Index");


        }
        [HttpPost]
        public async Task<ActionResult> UpdateUser(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient Client = new HttpClient())
                {
                    string Url = _baseUrl + "Account/UpdateUser";
                    HttpResponseMessage response = await Client.PostAsJsonAsync(Url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        ResultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                        TempData["Message"] = ResultModel.Message;
                    }
                }
            }
            else
            {

                return View("EditUser");
            }
            return RedirectToAction("UserList");
        }
        //[RealStateAuthorize(Roles = "A")]
        [HttpGet]
        public ActionResult CreateUser()
        {
            AccountModel obj = new AccountModel();

            return View(obj);

            //return RedirectToAction("UserList");
        }
        //[RealStateAuthorize(Roles = "A")]
        [HttpPost]
        public async Task<ActionResult> CreateUser(AccountModel model)
        {

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {

                    string Url = _baseUrl + "Account/Adduser";
                    HttpResponseMessage response = await client.PostAsJsonAsync(Url, model);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        ResultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                        TempData["Message"] = ResultModel.Message;
                        return RedirectToAction("UserList");
                    }
                }
            }
            else
            {
                return View();
            }

            return RedirectToAction("CreateUser");
        }
        public async Task<ActionResult> DeletUser(long Id)
        {
            //ResultModel model = new ResultModel();
            string Url = _baseUrl + "Account/DeleteUser?id=" + Id + "";
            if (Id != null)
            {
                using (HttpClient client = new HttpClient())
                {


                    HttpResponseMessage response = await client.GetAsync(Url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        ResultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                        TempData["Message"] = ResultModel.Message;

                        return RedirectToAction("UserList");
                    }
                }
            }
            else
            {
                return RedirectToAction("UserList");
            }

            return RedirectToAction("Index");


        }

        private void LoginAccessSecurity(AccountModel User)
        {
            RealstatePrincipal.PrincipalSerializeModel model = new RealstatePrincipal.PrincipalSerializeModel();
            model.UserID = User.UserID;
            model.FirstName = User.FirstName;
            model.LastName = User.LastName;
            model.Roles = new string[] { User.Role };
            string UserData = JsonConvert.SerializeObject(model);
            FormsAuthenticationTicket AuthTicket = new FormsAuthenticationTicket(1, User.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, UserData);
            string Encticket = FormsAuthentication.Encrypt(AuthTicket);
            HttpCookie Facookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encticket);
            Response.Cookies.Add(Facookie);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using skillforum.Models;


namespace skillforum.Controllers
{
    public class HomeController : Controller
    {

        // Home Page of skill forum //

        [HttpGet]
        public ActionResult home1()

        {
            ViewBag.result = "";
            return View();

        }




        [HttpGet]
        public ActionResult login()

        {
            ViewBag.result = "";
            return View();

        }

        [HttpPost]
        public ActionResult login(string contactno)
        {
            /*like '+91%' and LEN(@contactno)=13*/
            string query = "select * from UserDetail where contactno=@contactno ";
            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("contactno", contactno);

            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                if (sdr.Read())
                {
                    // after validating the user id -system  is redirecting  to upload.cshtml  to upload the study material //

                    return RedirectToAction("Dashboard"); 

                }

            }
            else
            {

                // if user is not  registered  then redirecting to user on Registration Page // 

                return RedirectToAction("Registration");
            }
            con.Close();
            return View();

        }



        //-----------------Registration Pgae -----------------------//

        [HttpGet]
        public ActionResult Registration()

        {
            ViewBag.result = "";
            return View();

        }

        [HttpPost]
        public ActionResult Registration(UserRegister model)
        {
            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            String query = "insert into UserDetail(username,emailid,contactno,institute,city,state,pincode) values(@username,@emailid,@contactno,@institute,@city,@state,@pincode)";


            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("username", model.username);
            cmd.Parameters.AddWithValue("emailid", model.emailid);
            cmd.Parameters.AddWithValue("contactno", model.contactno);
            cmd.Parameters.AddWithValue("institute", model.institute);
            cmd.Parameters.AddWithValue("city", model.city);
            cmd.Parameters.AddWithValue("state", model.state);
            cmd.Parameters.AddWithValue("pincode", model.pincode);

            cmd.CommandText = query;
            cmd.Connection = con;
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {

                return RedirectToAction("login");
            }
            else
            {
                ViewBag.result = "Insert failed";
            }

            con.Close();



            return View();
        }


        //------------------ Dashboard Page  ----------------// 

        [HttpGet]
        public ActionResult Dashboard()

        {
            ViewBag.result = "";
            return View();

        }



        [HttpPost]
        public ActionResult Dashboard(Dashboard model, HttpPostedFileBase ImageFile)
        {

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            String query = "insert into ViewTable(pkCourse_ID,pkTrade_ID,pkContent_ID,pkDescription,pkContent_locat,keyword,filesize,language,publisher,Embedd) values(@pkCourse_ID,@pkTrade_ID,@pkContent_ID,@pkDescription,@pkContent_locat,@keyword,@filesize,@language,@publisher,@Embedd)";

            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();          
            cmd.Parameters.AddWithValue("pkCourse_ID", model.pkCourse_ID);
            cmd.Parameters.AddWithValue("pkTrade_ID", model.pkTrade_ID);
            cmd.Parameters.AddWithValue("pkContent_ID", model.pkContent_ID);
            cmd.Parameters.AddWithValue("pkDescription", model.pkDescription);
            cmd.Parameters.AddWithValue("filesize", model.filesize);
            cmd.Parameters.AddWithValue("keyword", model.keyword);
            cmd.Parameters.AddWithValue("publisher", model.publisher);
            cmd.Parameters.AddWithValue("language", model.language);
            cmd.Parameters.AddWithValue("Embedd", model.Embedd);
            





            if (ImageFile == null || ImageFile.ContentLength <= 0)
            {
                cmd.Parameters.AddWithValue("pkContent_locat", model.pkContent_locat);
            }
            else
            {
                //saving file on DashboardUploadFolder
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string randomString = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                if (ImageFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(ImageFile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/DashBoadUpload"), randomString + _FileName);
                    ImageFile.SaveAs(_path);

                    cmd.Parameters.AddWithValue("pkContent_locat", Path.Combine(("/DashBoadUpload"), randomString + _FileName));
                }
            }

            cmd.CommandText = query;
            cmd.Connection = con;
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.result = "Insert failed";
            }

            con.Close();

            return View();
        }


        // -----------------view page  video -------------------------//


        [HttpGet]

        public ActionResult video()
        {
            string strQuery = "";

            //string querydata = "video";
            //if (querydata != "")
            //{
            //    strQuery = " where pkContent_ID = '" + querydata + "'";
            //}

            // "+ strQuery + "

            video objuser = new video();
            string query = "select * from ViewTable " + strQuery + " order by pkTrade_ID  asc ";

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand(strcon,con);
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            //----------------------------------
            ViewBag.pkTrade_ID = ds.Tables[0];

            List<SelectListItem> getpkTrade_ID = new List<SelectListItem>();

            foreach (System.Data.DataRow dr in ViewBag.pkTrade_ID.Rows)
            {
                getpkTrade_ID.Add(new SelectListItem { Text = @dr["pkTrade_ID"].ToString(), Value = @dr["pkTrade_ID"].ToString() });
            }
            ViewBag.pkTrade_ID = getpkTrade_ID;

            //ViewBag.pkTrade_ID = new SelectList(db.Audits.Select(m => m.Audit_Status).Distinct(), "Audit_Status", "Audit_Status");
            //-------------------------------------

            List<video> userlist = new List<video>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                video uobj = new video();
                uobj.pkCourse_ID = ds.Tables[0].Rows[i]["pkCourse_ID"].ToString();
                uobj.pkTrade_ID = ds.Tables[0].Rows[i]["pkTrade_ID"].ToString();
                uobj.pkContent_ID = ds.Tables[0].Rows[i]["pkContent_ID"].ToString();
                uobj.pkDescription = ds.Tables[0].Rows[i]["pkDescription"].ToString();
                uobj.pkContent_locat = ds.Tables[0].Rows[i]["pkContent_locat"].ToString();
                uobj.Embedd = ds.Tables[0].Rows[i]["Embedd"].ToString();
                uobj.filesize = ds.Tables[0].Rows[i]["filesize"].ToString();
                uobj.keyword = ds.Tables[0].Rows[i]["keyword"].ToString();
                uobj.publisher = ds.Tables[0].Rows[i]["publisher"].ToString();
                uobj.language = ds.Tables[0].Rows[i]["language"].ToString();


                userlist.Add(uobj);
            }

            objuser.usersinfo = userlist;

            con.Close();

            return View(objuser);

        }


       





        //------------------------view page e-book ----------------------//

        [HttpGet]

        public ActionResult ebook()
        {
            string strQuery = "";

            //string querydata = "video";
            //if (querydata != "")
            //{
            //    strQuery = " where pkContent_ID = '" + querydata + "'";
            //}

            // "+ strQuery + "

            ebook objuser = new ebook();
            string query = "select * from ViewTable " + strQuery + " order by pkTrade_ID  asc ";

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            List<ebook> userlist = new List<ebook>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ebook uobj = new ebook();
                uobj.pkCourse_ID = ds.Tables[0].Rows[i]["pkCourse_ID"].ToString();
                uobj.pkTrade_ID = ds.Tables[0].Rows[i]["pkTrade_ID"].ToString();
                uobj.pkContent_ID = ds.Tables[0].Rows[i]["pkContent_ID"].ToString();
                uobj.pkDescription = ds.Tables[0].Rows[i]["pkDescription"].ToString();
                uobj.pkContent_locat = ds.Tables[0].Rows[i]["pkContent_locat"].ToString();
                uobj.Embedd = ds.Tables[0].Rows[i]["Embedd"].ToString();
                uobj.filesize = ds.Tables[0].Rows[i]["filesize"].ToString();
                uobj.keyword = ds.Tables[0].Rows[i]["keyword"].ToString();
                uobj.publisher = ds.Tables[0].Rows[i]["publisher"].ToString();
                uobj.language = ds.Tables[0].Rows[i]["language"].ToString();


                userlist.Add(uobj);
            }

            objuser.usersinfo = userlist;

            con.Close();

            return View(objuser);

        }




        //----------------------view page for Question bank ----------------------------// 

        [HttpGet]

        public ActionResult questionbank()
        {
            string strQuery = "";

            //string querydata = "video";
            //if (querydata != "")
            //{
            //    strQuery = " where pkContent_ID = '" + querydata + "'";
            //}

            // "+ strQuery + "

            questionbank objuser = new questionbank();
            string query = "select * from ViewTable " + strQuery + " order by pkTrade_ID  asc ";

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            List<questionbank> userlist = new List<questionbank>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                questionbank uobj = new questionbank();
                uobj.pkCourse_ID = ds.Tables[0].Rows[i]["pkCourse_ID"].ToString();
                uobj.pkTrade_ID = ds.Tables[0].Rows[i]["pkTrade_ID"].ToString();
                uobj.pkContent_ID = ds.Tables[0].Rows[i]["pkContent_ID"].ToString();
                uobj.pkDescription = ds.Tables[0].Rows[i]["pkDescription"].ToString();
                uobj.pkContent_locat = ds.Tables[0].Rows[i]["pkContent_locat"].ToString();
                uobj.Embedd = ds.Tables[0].Rows[i]["Embedd"].ToString();
                uobj.filesize = ds.Tables[0].Rows[i]["filesize"].ToString();
                uobj.keyword = ds.Tables[0].Rows[i]["keyword"].ToString();
                uobj.publisher = ds.Tables[0].Rows[i]["publisher"].ToString();
                uobj.language = ds.Tables[0].Rows[i]["language"].ToString();

                userlist.Add(uobj);
            }

            objuser.usersinfo = userlist;

            con.Close();

            return View(objuser);

        }

        //-------------------------//



        [HttpGet]

        
        public ActionResult GetDataDropDown()

        {

            string strQuery = "";

          

            questionbank objuser = new questionbank();
            string query = "select * from ViewTable " ;

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            List<questionbank> userlist = new List<questionbank>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                questionbank uobj = new questionbank();
                uobj.pkTrade_ID = ds.Tables[0].Rows[i]["pkTrade_ID"].ToString();
               // ViewBag.dropdown = uobj;
                SelectList obj = new SelectList(userlist, "pkTrade_ID", "pkTrade_ID");
                // obj.Text= "pkTrade_ID"
                ViewBag.dropdown = obj;


                //userlist.Add(uobj);
            }

           // objuser.usersinfo = userlist;

            con.Close();

            return View();

        }



        //------------------------ Edit Profile -------------//


        [HttpGet]
        public ActionResult editprofile()

        {
            ViewBag.result = "";
            return View();

        }

        [HttpPost]
        public ActionResult editprofile(UserRegister model)
        {
            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            String query = "insert into UserDetail(username,emailid,contactno,institute,city,state,pincode) values(@username,@emailid,@contactno,@institute,@city,@state,@pincode)";


            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("username", model.username);
            cmd.Parameters.AddWithValue("emailid", model.emailid);
            cmd.Parameters.AddWithValue("contactno", model.contactno);
            cmd.Parameters.AddWithValue("institute", model.institute);
            cmd.Parameters.AddWithValue("city", model.city);
            cmd.Parameters.AddWithValue("state", model.state);
            cmd.Parameters.AddWithValue("pincode", model.pincode);

            cmd.CommandText = query;
            cmd.Connection = con;
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {

                return RedirectToAction("login");
            }
            else
            {
                ViewBag.result = "Insert failed";
            }

            con.Close();



            return View();
        }















        //--------------------------------------------------// 
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult GetCourse()
        {
            string query = "select * from ViewTable2 ";

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand(strcon, con);
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            //------------
            string courses = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                courses += ds.Tables[0].Rows[i]["pkCourse_ID"].ToString()+ ";";

               
            }
            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVideosTrade(string course)
        {
            string query = "select * from ViewTable where pkContent_Id = 'video' and pkcourse_Id = '" +course+"'" ;

            string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand(strcon, con);
            DataSet ds = new DataSet();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            string courses = "";
            try
            {
                da.Fill(ds);
                //------------
               
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    courses += ds.Tables[0].Rows[i]["pkTrade_ID"].ToString() + ";";


                }
            }
            catch { }
            return Json(courses, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skillforum.Models
{
    public class UserRegister
    {
        public string username { get; set; }
        public string emailid { get; set; }
        public string contactno { get; set; }
        public string institute { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }


    }
    public class login
    {
        public string contactno { get; set; }
    }

    //Dashboard page model

    public class Dashboard
    {
        public string pkCourse_ID { get; set; }

        public string pkTrade_ID { get; set; }

        public string pkContent_ID { get; set; }

        public string pkDescription { get; set; }

        public string pkContent_locat { get; set; }

        public string filesize { get; set; }

      

        public string keyword { get; set; }

        public string publisher { get; set; }

        public string language { get; set; }

        public string Embedd { get; set; }

        
        public HttpPostedFile ImageFile { get; set; }



    }
    public class video
    {
        public string pkCourse_ID { get; set; }

        public string pkTrade_ID { get; set; }

        public string pkContent_ID { get; set; }

        public string pkDescription { get; set; }

        public string pkContent_locat { get; set; }

        public string Embedd { get; set; }

        public string filesize { get; set; }

        public string keyword { get; set; }

        public string publisher { get; set; }

        public string language { get; set; }

        public List<video> usersinfo { get; set; }

    }



    public class ebook
    {
        public string pkCourse_ID { get; set; }

        public string pkTrade_ID { get; set; }

        public string pkContent_ID { get; set; }

        public string pkDescription { get; set; }

        public string pkContent_locat { get; set; }

        public string Embedd { get; set; }

        public string filesize { get; set; }

        public string keyword { get; set; }

        public string publisher { get; set; }

        public string language { get; set; }

        public List<ebook> usersinfo { get; set; }

    }




    public class questionbank
    {
        public string pkCourse_ID { get; set; }

        public string pkTrade_ID { get; set; }

        public string pkContent_ID { get; set; }

        public string pkDescription { get; set; }

        public string pkContent_locat { get; set; }

        public string Embedd { get; set; }

        public string filesize { get; set; }

        public string keyword { get; set; }

        public string publisher { get; set; }

        public string language { get; set; }

        public List<questionbank> usersinfo { get; set; }

    }
}

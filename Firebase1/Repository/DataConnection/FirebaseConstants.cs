using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Firebase1.Repository.DataConnection
{
    public static class FirebaseConstants
    {
        public static string AuthorizationSecret = "PZjMJiuNemCXxW5XrTQez1BueCUQKDFOd2swtOGM";
        public static string FirebaseDatabaseAddress = "https://sod316c-cae14-default-rtdb.firebaseio.com/";
        public static string Web_ApiKey = "AIzaSyChJAGNad3PVW59JDt8BedR0FeLrxH6Cts";
        public static string FromMail = "SOD316C@gmail.com";
        public static string FromPsw = "123456";
        public static string ResponseMessageEmailTemplate = @"/EmailTemplate/ResponseMessageTemplate.txt";
        public static string Bucket = "sod316c-cae14.appspot.com";

        #region "Category Types"
        public static string Juice = "Juice";
        public static string Chicken = "Chicken";
        #endregion
    }
}